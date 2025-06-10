using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DuelManager : MonoBehaviour
{
    public static DuelManager Instance;

    [Header("Settings")]
    public int pairsToMatch = 5;
    public float totalTime = 60f;
    public float flipDelay = 0.5f;

    [Header("References")]
    public Slider timerSlider;
    public List<Card> cards = new List<Card>();

    // Game state
    private int matchedPairs = 0;
    private Card firstSelectedCard;
    private bool isProcessing;
    public bool IsDuelActive { get; private set; }

    void Awake() => Instance = this;

    void Start()
    {
        InitializeGame();
        StartCoroutine(TimerCountdown());
    }

    void InitializeGame()
    {
        IsDuelActive = true;
        matchedPairs = 0;
        firstSelectedCard = null;
        isProcessing = false;
        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;

        foreach (Card card in cards)
        {
            card.InitializeCard();
        }

        ShuffleCards();
    }

    void ShuffleCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(i, cards.Count);
            (cards[randomIndex], cards[i]) = (cards[i], cards[randomIndex]);
        }
    }

    public void OnCardFlipped(Card card)
    {
        if (isProcessing || card.IsMatched) return;

        if (firstSelectedCard == null)
        {
            firstSelectedCard = card;
        }
        else
        {
            StartCoroutine(ProcessCardMatch(card));
        }
    }

    IEnumerator ProcessCardMatch(Card secondCard)
    {
        isProcessing = true;

        yield return new WaitForSeconds(flipDelay);

        bool isMatch = firstSelectedCard.cardID == secondCard.cardID;

        if (isMatch)
        {
            firstSelectedCard.IsMatched = true;
            secondCard.IsMatched = true;
            matchedPairs++;

            if (matchedPairs >= pairsToMatch)
            {
                GameSceneManager.Instance.EndDuel(true);
                yield break;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            firstSelectedCard.Flip();
            secondCard.Flip();
        }

        firstSelectedCard = null;
        isProcessing = false;
    }

    IEnumerator TimerCountdown()
    {
        while (timerSlider.value > 0 && IsDuelActive)
        {
            timerSlider.value -= 1;
            yield return new WaitForSeconds(1f);
        }

        if (IsDuelActive) GameSceneManager.Instance.EndDuel(false);
    }

    public void ForceEndDuel() => IsDuelActive = false;
}
