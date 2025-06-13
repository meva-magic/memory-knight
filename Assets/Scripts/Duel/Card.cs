using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    [Header("Card Settings")]
    public string cardID;
    public GameObject frontFace;
    public GameObject backFace;

    [Header("Card Visuals")]
    public Sprite frontSprite;

    [System.NonSerialized] public bool IsMatched;
    [System.NonSerialized] public bool IsFlipped;

    private Image frontImage;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Flip);
        frontImage = frontFace.GetComponent<Image>();
    }

    public void InitializeCard()
    {
        IsMatched = false;
        IsFlipped = false;
        
        // Set the front face sprite
        if (frontSprite != null && frontImage != null)
        {
            frontImage.sprite = frontSprite;
        }
        
        UpdateVisuals();
    }

    public void Flip()
    {
        if (!IsMatched && DuelManager.Instance.CanAcceptInput)
        {
            DuelManager.Instance.ProcessCardFlip(this);
        }
    }

    public void UpdateVisuals()
    {
        if (frontFace != null) frontFace.SetActive(IsFlipped || IsMatched);
        if (backFace != null) backFace.SetActive(!IsFlipped && !IsMatched);
    }
}