using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image frontFace;           // Лицевая сторона карты
    public Image backFace;            // Обратная сторона карты
    public string tagName;            // Уникальный тег карты
    private bool isRevealed = false;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            DuelManager manager = FindObjectOfType<DuelManager>(); // находим менеджер
            manager.HandleCardSelection(this); // отправляем карту на обработку
        });
    }

    public void Reveal()
    {
        if (!isRevealed)
        {
            frontFace.gameObject.SetActive(true);
            backFace.gameObject.SetActive(false);
            isRevealed = true;
        }
    }

    public void Hide()
    {
        if (isRevealed)
        {
            frontFace.gameObject.SetActive(false);
            backFace.gameObject.SetActive(true);
            isRevealed = false;
        }
    }

    public bool Matches(string otherTag)
    {
        return tagName.Equals(otherTag);
    }

    public void SetCardData(Image front, Image back, string tag)
    {
        frontFace = front;
        backFace = back;
        tagName = tag;
    }

    public void ResetState()
    {
        Hide();
    }
}