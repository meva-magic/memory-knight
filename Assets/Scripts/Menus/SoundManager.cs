using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("UI References")]
    public Scrollbar volumeScrollbar; 
    public Button muteButton;        
    public Image muteButtonImage;    
    
    [Header("Sprites")]
    public Sprite soundOnSprite;     
    public Sprite soundOffSprite;    

    private bool isMuted = false;
    private float savedVolume = 1f; 

    void Start()
    {
        AudioManager.instance.Play("Settings");
        
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

        volumeScrollbar.onValueChanged.AddListener(ChangeVolume);
        muteButton.onClick.AddListener(ToggleMute);

        AudioManager.instance.Play("ButtonPress");
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