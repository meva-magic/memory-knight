using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Interaction : MonoBehaviour
{
    [Header("References")]
    public PlayerMove playerMoveScript;
    public WoodsUI woodsUI;

    [Header("Settings")]
    public float dialogueStartDelay = 0.3f;
    public string regularNPCTag = "NPC";
    public string sceneChangeNPCTag = "SceneChangeNPC";
    public string postDialogueSceneToLoad;

    private bool isInteractingWithSceneChanger = false;
    private NPC currentNPC;

    public static Interaction instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!playerMoveScript) playerMoveScript = FindObjectOfType<PlayerMove>(true);
        if (!woodsUI) woodsUI = FindObjectOfType<WoodsUI>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(regularNPCTag) || other.CompareTag(sceneChangeNPCTag))
        {
            NPC npc = other.GetComponentInParent<NPC>();
            if (npc != null)
            {
                StartInteraction(npc, other.CompareTag(sceneChangeNPCTag));
                other.enabled = false;
            }
        }
    }

    private void StartInteraction(NPC npc, bool isSceneChanger)
    {
        isInteractingWithSceneChanger = isSceneChanger;
        currentNPC = npc;
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        AudioManager.instance.Play("StartDialogue");

        DisablePlayerMovement();
        yield return new WaitForSeconds(dialogueStartDelay);
        
        currentNPC.Interact();
        
        StartCoroutine(WaitForDialogueCompletion());
    }

    private IEnumerator WaitForDialogueCompletion()
    {
        while (currentNPC.dialoguePanel.activeSelf)
        {
            yield return null;
        }
        
        HandleDialogueCompletion();
    }

    private void HandleDialogueCompletion()
    {
        if (isInteractingWithSceneChanger && !string.IsNullOrEmpty(postDialogueSceneToLoad))
        {
            StartCoroutine(LoadSceneAfterDelay(0.1f));
        }
        else
        {
            EnablePlayerMovement();
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(postDialogueSceneToLoad);
    }

    public void DisablePlayerMovement()
    {
        if (playerMoveScript) playerMoveScript.DisableMovement();
        if (woodsUI) woodsUI.DisableJoystick();
    }

    public void EnablePlayerMovement()
    {
        if (playerMoveScript) playerMoveScript.EnableMovement();
        if (woodsUI) woodsUI.EnableJoystick();
    }
}