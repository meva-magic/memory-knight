using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardID;
    public GameObject frontFace;
    public GameObject backFace;

    [System.NonSerialized] public bool IsMatched;
    [System.NonSerialized] public bool IsFlipped;

    void Start() => InitializeCard();

    public void InitializeCard()
    {
        IsMatched = false;
        IsFlipped = false;
        UpdateVisuals();
    }

    public void Flip()
    {
        if (CanFlip())
        {
            IsFlipped = !IsFlipped;
            UpdateVisuals();
            DuelManager.Instance.OnCardFlipped(this);
        }
    }

    void UpdateVisuals()
    {
        frontFace.SetActive(IsFlipped);
        backFace.SetActive(!IsFlipped);
    }

    bool CanFlip() => !IsMatched && DuelManager.Instance.IsDuelActive;
}
