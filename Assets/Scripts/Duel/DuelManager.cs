using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    public static DuelManager Instance;
    public string Theme;

    [Header("Game Settings")]
    public int pairsToWin = 5;
    public float totalTime = 60f;
    public float flipDelay = 0.5f;
    public float flipBackDelay = 0.5f;

    [Header("Card References")]
    public Transform cardsParent; // Parent object containing all cards
    public List<Card> allCards = new List<Card>();

    [Header("UI References")]
    public Slider timerSlider;
    public GameObject restartPanel;

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

    private void Start()
    {
        InitializeGame();
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.Play(Theme);
    }

    public void InitializeGame()
    {
        IsGameActive = true;
        matchedPairs = 0;
        flippedCards.Clear();

        if (restartPanel != null)
            restartPanel.SetActive(false);

        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;

        // Initialize and shuffle cards
        InitializeAllCards();
        StartCoroutine(ShuffleCardsWithDelay());
    }

    private void InitializeAllCards()
    {
        foreach (var card in allCards)
        {
            card.InitializeCard();
            card.gameObject.SetActive(true); // Ensure card is active
        }
    }

    private IEnumerator ShuffleCardsWithDelay()
    {
        // Wait one frame to ensure all cards are initialized
        yield return null;
        
        ShuffleCardPositions();
        timerRoutine = StartCoroutine(TimerCountdown());
    }

    private void ShuffleCardPositions()
    {
        // Disable any layout groups temporarily
        var layoutGroup = cardsParent.GetComponent<LayoutGroup>();
        if (layoutGroup != null) layoutGroup.enabled = false;

        // Get all card positions
        List<Vector3> positions = new List<Vector3>();
        foreach (Card card in allCards)
        {
            positions.Add(card.transform.localPosition);
        }

        // Fisher-Yates shuffle
        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(i, positions.Count);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // Apply new positions
        for (int i = 0; i < allCards.Count; i++)
        {
            allCards[i].transform.localPosition = positions[i];
        }
    }

    public void ProcessCardFlip(Card card)
    {
        if (!CanAcceptInput || card.IsFlipped || card.IsMatched) return;

        AudioManager.instance.Play("CardPress");
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
            AudioManager.instance.Play("WrongPair");

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
            timerSlider.value = timeRemaining;
            yield return null;
        }

        if (IsGameActive) EndGame(false);
    }

    public void EndGame(bool isWin)
    {
        IsGameActive = false;
        if (timerRoutine != null) StopCoroutine(timerRoutine);
        
        if (isWin)
        {
            GameSceneManager.Instance.LoadEndScene(true);
        }
        else
        {
            if (restartPanel != null)
            {
                AudioManager.instance.StopAllSounds();
                AudioManager.instance.Play("KnightLose");
                restartPanel.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        GameSceneManager.Instance.ReloadCurrentScene();
    }

    public void ReturnToMenu()
    {
        GameSceneManager.Instance.LoadMenuScene();
    }
}