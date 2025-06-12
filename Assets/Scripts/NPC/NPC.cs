using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public Button continueButton;
    public TMP_Text dialogueText, nameText;

    private int dialogueIndex;
    private bool isTyping;
    private Interaction interactionSystem;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        continueButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue(Interaction interaction)
    {
        interactionSystem = interaction;
        dialogueIndex = 0;
        nameText.SetText(dialogueData.Name);
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    public void NextLine()
    {
        AudioManager.instance.Play("ButtonPress");
        
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            continueButton.gameObject.SetActive(true);
            return;
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");
        continueButton.gameObject.SetActive(false);

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            
            if (dialogueData.voice != null)
            {
                audioSource.PlayOneShot(dialogueData.voice, dialogueData.voiceVolume);
            }

            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        continueButton.gameObject.SetActive(true);
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        interactionSystem.OnDialogueComplete();
    }

    public void Interact()
    {
        if (!dialoguePanel.activeSelf)
        {
            StartDialogue(Interaction.instance);
        }
        else
        {
            NextLine();
        }
    }
}