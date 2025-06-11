using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("UI References")]
    public Scrollbar volumeScrollbar; // Assign in inspector
    public Button muteButton;        // Assign in inspector
    public Image muteButtonImage;    // Assign in inspector
    
    [Header("Sprites")]
    public Sprite soundOnSprite;     // Assign sound on icon
    public Sprite soundOffSprite;    // Assign sound off icon

    private bool isMuted = false;
    private float savedVolume = 1f; // Default to full volume

    void Start()
    {
        AudioManager.instance.Play("Settings");
        
        // Initialize with saved preferences or defaults
        if (PlayerPrefs.HasKey("Volume"))
        {
            savedVolume = PlayerPrefs.GetFloat("Volume");
            volumeScrollbar.value = savedVolume;
            AudioListener.volume = savedVolume;
        }
        
        if (PlayerPrefs.HasKey("IsMuted"))
        {
            isMuted = PlayerPrefs.GetInt("IsMuted") == 1;
            UpdateMuteState();
        }

        // Set up event listeners
        volumeScrollbar.onValueChanged.AddListener(ChangeVolume);
        muteButton.onClick.AddListener(ToggleMute);
    }

    public void ChangeVolume(float volume)
    {
        if (!isMuted)
        {
            AudioListener.volume = volume;
            savedVolume = volume;
            PlayerPrefs.SetFloat("Volume", volume);
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        UpdateMuteState();
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    }

    private void UpdateMuteState()
    {
        if (isMuted)
        {
            AudioListener.volume = 0;
            muteButtonImage.sprite = soundOffSprite;
            volumeScrollbar.interactable = false;
        }
        else
        {
            AudioListener.volume = savedVolume;
            muteButtonImage.sprite = soundOnSprite;
            volumeScrollbar.interactable = true;
        }
    }
}