using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class NPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    [Header("Events")]
    public UnityEvent OnDialogueStart;
    public UnityEvent OnDialogueEnd;
    public UnityEvent OnDialogueLineComplete;

    private AudioSource audioSource;
    private Interaction interactionSystem;
    private Collider2D interactionCollider;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private AudioSourcePool audioSourcePool;

    public bool IsDialogueActive => isDialogueActive;
    public bool HasMoreDialogue => currentLineIndex < dialogueData.dialogueLines.Length - 1;
    public bool IsTyping => isTyping;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        interactionCollider = GetComponentInChildren<Collider2D>();
        audioSourcePool = gameObject.AddComponent<AudioSourcePool>();
        
        if (dialoguePanel == null) Debug.LogError("Dialogue Panel not assigned!", this);
        if (dialogueText == null) Debug.LogError("Dialogue Text not assigned!", this);
        if (nameText == null) Debug.LogError("Name Text not assigned!", this);
    }

    public void Initialize(Interaction interaction, bool sceneChanger)
    {
        interactionSystem = interaction;
    }

    public void StartDialogue()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("No dialogue data assigned!", this);
            return;
        }

        currentLineIndex = 0;
        nameText.text = dialogueData.Name;
        dialoguePanel.SetActive(true);
        isDialogueActive = true;
        
        OnDialogueStart?.Invoke();
        TypeCurrentLine();
    }

    private void TypeCurrentLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLineCoroutine());
    }

    private IEnumerator TypeLineCoroutine()
    {
        isTyping = true;
        interactionSystem?.UpdateButtons(false, false);
        
        // Clear text before starting new line
        dialogueText.text = "";
        string currentLine = dialogueData.dialogueLines[currentLineIndex];

        foreach (char letter in currentLine)
        {
            dialogueText.text += letter;

            if (dialogueData.voice != null)
            {
                AudioSource voiceSource = audioSourcePool.GetAvailableAudioSource();
                if (voiceSource != null)
                {
                    voiceSource.clip = dialogueData.voice;
                    voiceSource.volume = dialogueData.voiceVolume;
                    voiceSource.Play();
                }
            }

            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        OnDialogueLineComplete?.Invoke();
        
        // Update buttons based on remaining lines
        bool hasMore = currentLineIndex < dialogueData.dialogueLines.Length - 1;
        interactionSystem?.UpdateButtons(hasMore, !hasMore);
    }

    public void AdvanceDialogue()
    {
        // Don't advance if still typing (should be handled by CompleteCurrentLine)
        if (isTyping) return;

        currentLineIndex++;
        if (currentLineIndex < dialogueData.dialogueLines.Length)
        {
            TypeCurrentLine(); // Start typing next line
        }
        else
        {
            EndDialogue();
        }
    }

    public void CompleteCurrentLine()
    {
        if (!isTyping) return;

        StopCoroutine(typingCoroutine);
        dialogueText.text = dialogueData.dialogueLines[currentLineIndex];
        isTyping = false;
        OnDialogueLineComplete?.Invoke();
        
        // Update buttons after completing line
        bool hasMore = currentLineIndex < dialogueData.dialogueLines.Length - 1;
        interactionSystem?.UpdateButtons(hasMore, !hasMore);
    }

    public void SkipDialogue()
    {
        if (isDialogueActive)
        {
            CompleteCurrentLine();
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        interactionCollider.enabled = false;
        
        OnDialogueEnd?.Invoke();
        interactionSystem?.OnDialogueComplete();
    }

    public void ResetDialogue()
    {
        isDialogueActive = false;
        interactionCollider.enabled = true;
        currentLineIndex = 0;
        isTyping = false;
    }
}

public class AudioSourcePool : MonoBehaviour
{
    private List<AudioSource> audioSources = new List<AudioSource>();
    private int poolSize = 5;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject($"AudioSource_{i}");
            go.transform.SetParent(transform);
            AudioSource source = go.AddComponent<AudioSource>();
            audioSources.Add(source);
        }
    }

    public AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        
        // Dynamic expansion if all sources are busy
        GameObject go = new GameObject($"AudioSource_{audioSources.Count}");
        go.transform.SetParent(transform);
        AudioSource newSource = go.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        return newSource;
    }
}