using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    public List<string> cardTags;                  // Список уникальных тегов для карт
    public Sprite defaultBackFace;                 // Спрайт рубашки карты
    public Transform gridContainer;               // Контейнер с Grid Layout Group
    public ScoreKeeper scoreKeeper;                // Компонент счёта
    public int rows = 4;                          // Количество строк в сетке
    public int columns = 4;                       // Количество столбцов в сетке
    public float revealDelay = 2f;                // Задержка до автоматического закрытия карт

    private List<Card> allCards = new List<Card>();
    private List<Card> openedCards = new List<Card>(); // Открытые карты (полностью раскрытые)
    private Card firstSelectedCard;               // Первая открытая карта текущего хода
    private Card secondSelectedCard;              // Вторая открытая карта текущего хода
    private int totalPairs;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        // Управление карточками будет происходить через сам скрипт карты (Button)
    }

    void InitializeGame()
    {
        CreateDeck();
        ShuffleCards();
        totalPairs = cardTags.Count / 2;
        scoreKeeper.ResetScore(totalPairs * 2); // Максимальное количество очков
    }

    void CreateDeck()
    {
        for (int i = 0; i < gridContainer.childCount; i++)
        {
            GameObject cardObj = gridContainer.GetChild(i).gameObject;
            Card cardScript = cardObj.GetComponent<Card>();

            // Получаем соответствующий тег и устанавливаем его
            int tagIndex = i % cardTags.Count;
            cardScript.SetCardData(cardScript.frontFace, cardScript.backFace, cardTags[tagIndex]);
            allCards.Add(cardScript);
        }
    }

    void ShuffleCards()
    {
        var tempArray = new string[rows * columns];
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = cardTags[i % cardTags.Count]; // Повторяем список тегов
        }

        for (int j = 0; j < tempArray.Length; j++)
        {
            int randomIndex = Random.Range(j, tempArray.Length);
            string temp = tempArray[j];
            tempArray[j] = tempArray[randomIndex];
            tempArray[randomIndex] = temp;
        }

        for (int k = 0; k < allCards.Count; k++)
        {
            allCards[k].tagName = tempArray[k];
        }
    }

    public void HandleCardSelection(Card card)
    {
        if (openedCards.Contains(card)) return; // Если карта уже раскрыта окончательно, пропускаем

        if (firstSelectedCard == null)
        {
            // Если первая карта не выбрана, запоминаем её
            firstSelectedCard = card;
            card.Reveal();
        }
        else
        {
            // Вторая карта выбрана
            secondSelectedCard = card; // Сохраняем вторую карту
            card.Reveal();

            if (firstSelectedCard.Matches(card.tagName)) // Совпадает по тегу
            {
                // Пара совпала, раскрываем навсегда
                openedCards.Add(firstSelectedCard);
                openedCards.Add(card);
                scoreKeeper.IncrementScore();
            }
            else
            {
                // Несовпадающая пара, ждем задержки и прячем обе карты
                Invoke("HideSelectedCards", revealDelay); // Вызываем без аргументов
            }

            firstSelectedCard = null; // Сбрасываем выбранную карту
        }
    }

    void HideSelectedCards()
    {
        // Прячем обе карты, если они не входят в список раскрытых навсегда
        if (!openedCards.Contains(firstSelectedCard))
            firstSelectedCard.Hide();

        if (!openedCards.Contains(secondSelectedCard))
            secondSelectedCard.Hide();

        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    public void EndGame()
    {
        Time.timeScale = 0; // Пауза игры
        Debug.Log("Игра закончена! Ваш итоговый счёт: " + scoreKeeper.Score);
    }
}
