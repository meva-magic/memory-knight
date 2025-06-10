using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [Header("UI References")]
    public Text timerText;
    public Image timerFill;
    
    [Header("Timer Settings")]
    private float currentTime;
    private float maxTime;
    private bool isRunning;

    void Awake()
    {
        if (timerText == null)
        {
            timerText = GetComponent<Text>();
            Debug.LogWarning("TimerText reference not set, getting from component");
        }
    }

    void Update()
    {
        if (!isRunning) return;
        
        currentTime -= Time.deltaTime;
        UpdateDisplay();
        
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            StopTimer();
            FindObjectOfType<DuelUI>()?.OnTimeExpired();
        }
    }

    public void ResetTimer(float duration)
    {
        maxTime = duration;
        currentTime = maxTime;
        isRunning = true;
        UpdateDisplay();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateDisplay()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
        
        if (timerFill != null)
        {
            timerFill.fillAmount = currentTime / maxTime;
        }
    }
}
