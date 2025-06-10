using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 60f;
    public List<Card> allCards = new List<Card>();

    [Header("Game State")]
    public bool canSelect = true;
    private Card firstSelectedCard;
    private Card secondSelectedCard;

    private DuelUI duelUI;

    void Awake()
    {
        duelUI = GetComponent<DuelUI>();
        if (duelUI == null)
        {
            duelUI = FindObjectOfType<DuelUI>();
            Debug.LogWarning("DuelUI reference not set, finding in scene");
        }
    }

    void Start()
    {
        InitializeGame();
    }

    void OnValidate()
    {
        if (allCards.Count == 0)
        {
            Card[] foundCards = FindObjectsOfType<Card>();
            if (foundCards.Length > 0)
            {
                allCards = new List<Card>(foundCards);
                Debug.Log($"Auto-assigned {allCards.Count} cards");
            }
        }
    }

    public void InitializeGame()
    {
        if (allCards.Count == 0)
        {
            Debug.LogError("No cards assigned to DuelManager!");
            return;
        }

        foreach (Card card in allCards)
        {
            if (card != null)
            {
                card.ResetCard();
            }
        }
        
        firstSelectedCard = null;
        secondSelectedCard = null;
        canSelect = true;
        
        duelUI?.InitializeGame();
    }

    public void OnCardSelected(Card card)
    {
        if (!canSelect || card == null || card.IsMatched || card.IsFlipped) return;

        if (firstSelectedCard == null)
        {
            firstSelectedCard = card;
            card.Flip();
        }
        else if (secondSelectedCard == null)
        {
            secondSelectedCard = card;
            card.Flip();
            StartCoroutine(CheckForMatch());
        }
    }

    private IEnumerator CheckForMatch()
    {
        canSelect = false;
        yield return new WaitForSeconds(0.5f);

        if (firstSelectedCard != null && secondSelectedCard != null)
        {
            if (firstSelectedCard.cardID == secondSelectedCard.cardID)
            {
                firstSelectedCard.SetMatched();
                secondSelectedCard.SetMatched();
                duelUI?.OnMatchFound();
            }
            else
            {
                firstSelectedCard.Flip();
                secondSelectedCard.Flip();
            }
        }

        firstSelectedCard = null;
        secondSelectedCard = null;
        canSelect = true;
    }

    public int GetTotalPairs()
    {
        HashSet<string> uniqueIDs = new HashSet<string>();
        foreach (Card card in allCards)
        {
            if (card != null)
            {
                uniqueIDs.Add(card.cardID);
            }
        }
        return uniqueIDs.Count;
    }
}
