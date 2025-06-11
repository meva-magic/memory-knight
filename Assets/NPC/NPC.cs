using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public Button continueButton; // ссылка на кнопку для продолжения диалога
    public TMP_Text dialogueText, nameText;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private AudioSource audioSource;

    public static NPC instance;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (dialogueData == null)
            return;

        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else
        {
            NextLine(); // Переход к следующей строке при взаимодействии игрока
        }
    }

    public void StartDialogue()
    {
        Interaction.instance.DisablePlayerMovement();
        
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.Name);

        WoodsUI.instance.DisableJoystick();
        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]); // Останавливаем печать и выводим полную строку
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine()); // Начинаем показывать следующую строку
        }
        else
        {
            EndDialogue(); // Закрываем диалог, если больше строк нет
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText(""); // Обнуляем текст
        continueButton.gameObject.SetActive(false); // Прячем кнопку во время анимации ввода

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            
            if (dialogueData.voice != null)
            {
                audioSource.PlayOneShot(dialogueData.voice, dialogueData.voiceVolume); // Звук озвучивания
            }

            yield return new WaitForSeconds(dialogueData.typingSpeed); // Печатаем буквы одну за другой
        }

        isTyping = false;
        continueButton.gameObject.SetActive(true); // Показываем кнопку после окончания печати строки
    }

    public void EndDialogue()
    {
        StopAllCoroutines();

        isDialogueActive = false;
        dialogueText.SetText("");
        
        dialoguePanel.SetActive(false);
        WoodsUI.instance.DisableJoystick();

        Interaction.instance.EnablePlayerMovement();
    }
}