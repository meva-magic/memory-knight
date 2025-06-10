using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Add this for Slider component

public class DuelManager : MonoBehaviour
{
    public static DuelManager Instance;

    [Header("Game Settings")]
    public int pairsToWin = 5;
    public float totalTime = 60f;
    public float flipDelay = 0.5f;
    public float flipBackDelay = 1f;

    [Header("Card References")]
    public List<Card> allCards = new List<Card>();

    [Header("UI References")]
    public Slider timerSlider; // Reference to the UI Slider

    private readonly List<Card> flippedCards = new List<Card>();
    private int matchedPairs;
    private bool isProcessing;
    private Coroutine timerRoutine;
    public bool IsGameActive { get; private set; }
    public bool CanAcceptInput => flippedCards.Count < 2 && !isProcessing && IsGameActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start() => InitializeGame();

    public void InitializeGame()
    {
        IsGameActive = true;
        matchedPairs = 0;
        flippedCards.Clear();

        // Initialize the slider
        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;

        foreach (var card in allCards)
        {
            card.InitializeCard();
        }

        ShuffleCards();
        timerRoutine = StartCoroutine(TimerCountdown());
    }

    private void ShuffleCards()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            int randomIndex = Random.Range(i, allCards.Count);
            (allCards[i], allCards[randomIndex]) = (allCards[randomIndex], allCards[i]);
        }
    }

    public void ProcessCardFlip(Card card)
    {
        if (!CanAcceptInput || card.IsFlipped || card.IsMatched) return;

        card.IsFlipped = true;
        card.UpdateVisuals();
        flippedCards.Add(card);

        if (flippedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        isProcessing = true;
        yield return new WaitForSeconds(flipDelay);

        bool isMatch = flippedCards[0].cardID == flippedCards[1].cardID;

        if (isMatch)
        {
            flippedCards.ForEach(c => c.IsMatched = true);
            matchedPairs++;

            if (matchedPairs >= pairsToWin)
            {
                EndGame(true);
                yield break;
            }
        }
        else
        {
            yield return new WaitForSeconds(flipBackDelay);
            flippedCards.ForEach(c =>
            {
                c.IsFlipped = false;
                c.UpdateVisuals();
            });
        }

        flippedCards.Clear();
        isProcessing = false;
    }

    private IEnumerator TimerCountdown()
    {
        float timeRemaining = totalTime;
        
        while (timeRemaining > 0 && IsGameActive)
        {
            timeRemaining -= Time.deltaTime;
            timerSlider.value = timeRemaining; // Update the slider value
            yield return null;
        }

        if (IsGameActive) EndGame(false);
    }

    public void EndGame(bool isWin)
    {
        IsGameActive = false;
        if (timerRoutine != null) StopCoroutine(timerRoutine);
        GameSceneManager.Instance.LoadEndScene(isWin);
    }
}
