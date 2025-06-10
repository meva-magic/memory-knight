using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    public string cardID;
    public GameObject frontFace;
    public GameObject backFace;

    [System.NonSerialized] public bool IsMatched;
    [System.NonSerialized] public bool IsFlipped;

    private void Awake() => GetComponent<Button>().onClick.AddListener(Flip);

    public void InitializeCard()
    {
        IsMatched = false;
        IsFlipped = false;
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
        frontFace.SetActive(IsFlipped || IsMatched);
        backFace.SetActive(!IsFlipped && !IsMatched);
    }
}
