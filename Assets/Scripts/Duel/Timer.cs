using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Slider timerSlider;    // Ползунок, отображающий остаток времени
    public float startTime = 60f; // Начальное время таймера (60 секунд)

    private float remainingTime;  // Остаточное время

    void Start()
    {
        // Устанавливаем начальное время
        remainingTime = startTime;
        timerSlider.maxValue = startTime;
        timerSlider.value = startTime;
    }

    void Update()
    {
        // Уменьшаем время каждую секунду
        remainingTime -= Time.deltaTime;

        // Обновляем слайдер
        timerSlider.value = remainingTime;

        // Завершаем обратный отсчёт, если время вышло
        if (remainingTime <= 0f)
        {
            StopTimer();
        }
    }

    void StopTimer()
    {
        // Останавливаем таймер
        enabled = false;
        Debug.Log("Время истекло!");
    }
}