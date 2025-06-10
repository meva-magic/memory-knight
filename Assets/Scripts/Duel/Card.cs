using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Components")]
    public Image frontImage;
    public Image backImage;
    public Button button;

    [Header("State")]
    public string cardID;
    private bool isFaceUp;
    private bool isMatched;

    public bool IsFaceUp => isFaceUp;
    public bool IsMatched => isMatched;

    public void Initialize(string id, Sprite frontSprite)
    {
        cardID = id;
        frontImage.sprite = frontSprite;
        ResetCard();
    }

    public void Flip()
    {
        isFaceUp = !isFaceUp;
        frontImage.enabled = isFaceUp;
        backImage.enabled = !isFaceUp;
    }

    public void SetMatched()
    {
        isMatched = true;
        button.interactable = false;
    }

    public void ResetCard()
    {
        isFaceUp = false;
        isMatched = false;
        frontImage.enabled = false;
        backImage.enabled = true;
        button.interactable = true;
    }
}
