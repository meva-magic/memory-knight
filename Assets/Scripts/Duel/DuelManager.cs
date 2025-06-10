using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DuelManager : MonoBehaviour
{
    [Header("Settings")]
    public List<string> cardTags;          // Card matching pairs
    public float flipBackDelay = 1.5f;     // Time before unmatched cards flip back
    
    [Header("References")]
    public Transform cardGrid;             // Parent object holding all cards
    private List<Card> allCards = new List<Card>();
    
    // Game state
    private Card firstCard;
    private Card secondCard;
    private bool canSelect = true;
    private Coroutine flipBackRoutine;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // Get all card components
        allCards.Clear();
        foreach (Transform child in cardGrid)
        {
            var card = child.GetComponent<Card>();
            if (card != null) allCards.Add(card);
        }

        AssignCardPairs();
        ShuffleCards();
    }

    void AssignCardPairs()
    {
        // Create pairs from available tags
        var availablePairs = new List<string>();
        foreach (var tag in cardTags)
        {
            availablePairs.Add(tag);
            availablePairs.Add(tag); // Add twice for pairs
        }

        // Assign randomly to cards
        for (int i = 0; i < allCards.Count; i++)
        {
            if (availablePairs.Count == 0) break;
            
            int randomIndex = Random.Range(0, availablePairs.Count);
            string pairTag = availablePairs[randomIndex];
            
            allCards[i].Initialize(
                pairTag, 
                Resources.Load<Sprite>("Sprites/" + pairTag)
            );
            
            availablePairs.RemoveAt(randomIndex);
        }
    }

    void ShuffleCards()
    {
        // Randomize card positions in grid
        for (int i = 0; i < allCards.Count; i++)
        {
            int randomPos = Random.Range(0, allCards.Count);
            allCards[i].transform.SetSiblingIndex(randomPos);
        }
    }

    public void OnCardClicked(Card clickedCard)
    {
        if (!canSelect || clickedCard.IsMatched || clickedCard.IsFaceUp)
            return;

        // If two cards are already selected and unmatched
        if (firstCard != null && secondCard != null && !firstCard.IsMatched)
        {
            // Third card click flips previous pair back
            StopCoroutine(flipBackRoutine);
            FlipUnmatchedCards();
        }

        // Select new card
        clickedCard.Flip();

        if (firstCard == null)
        {
            firstCard = clickedCard;
        }
        else
        {
            secondCard = clickedCard;
            canSelect = false;
            flipBackRoutine = StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        if (firstCard.cardID == secondCard.cardID)
        {
            // Match found
            firstCard.SetMatched();
            secondCard.SetMatched();
            
            // Check win condition
            if (allCards.TrueForAll(c => c.IsMatched))
            {
                Debug.Log("You Win!");
            }
        }
        else
        {
            // No match - wait then flip back
            yield return new WaitForSeconds(flipBackDelay);
            FlipUnmatchedCards();
        }

        ResetSelection();
    }

    void FlipUnmatchedCards()
    {
        if (firstCard != null && !firstCard.IsMatched) firstCard.Flip();
        if (secondCard != null && !secondCard.IsMatched) secondCard.Flip();
        ResetSelection();
    }

    void ResetSelection()
    {
        firstCard = null;
        secondCard = null;
        canSelect = true;
    }
}
