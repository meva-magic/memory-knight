using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class Card : MonoBehaviour
{
    public Image cardImage;
    public Image backImage;
    public Button cardButton;
    
    public string cardID;
    public Sprite frontSprite;
    
    private bool isMatched = false;
    private bool isFlipped = false;
    private DuelManager duelManager;

    public bool IsMatched => isMatched;
    public bool IsFlipped => isFlipped;

    void Awake()
    {
        if (cardImage == null) cardImage = GetComponent<Image>();
        if (backImage == null) backImage = transform.Find("Back")?.GetComponent<Image>();
        if (cardButton == null) cardButton = GetComponent<Button>();

        duelManager = FindObjectOfType<DuelManager>();
        if (duelManager == null)
        {
            Debug.LogError("Card: DuelManager not found!");
            enabled = false;
        }
    }

    void Start()
    {
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(OnCardClicked);
        }
        else
        {
            Debug.LogWarning("Card: Button not assigned", gameObject);
        }

        ResetCard();
    }

    public void OnCardClicked()
    {
        if (duelManager != null)
        {
            duelManager.OnCardSelected(this);
        }
    }

    public void Flip()
    {
        if (isMatched) return;

        isFlipped = !isFlipped;

        if (backImage != null) backImage.gameObject.SetActive(!isFlipped);
        if (cardImage != null) cardImage.gameObject.SetActive(isFlipped);
    }

    public void SetMatched()
    {
        isMatched = true;
        if (cardButton != null) cardButton.interactable = false;
    }

    public void ResetCard()
    {
        isMatched = false;
        isFlipped = false;

        if (cardButton != null) cardButton.interactable = true;
        if (backImage != null) backImage.gameObject.SetActive(true);
        if (cardImage != null)
        {
            cardImage.gameObject.SetActive(false);
            if (frontSprite != null) cardImage.sprite = frontSprite;
        }
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (frontSprite != null && cardImage != null)
        {
            cardImage.sprite = frontSprite;
            cardImage.gameObject.SetActive(false);
        }
    }
    #endif
}
