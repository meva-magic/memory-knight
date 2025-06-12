using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    private NPC currentNPC;
    private bool isSceneChanger;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentNPC != null) return;

        if (other.CompareTag(regularNPCTag) || other.CompareTag(sceneChangeNPCTag))
        {
            NPC npc = other.GetComponentInParent<NPC>();
            if (npc != null)
            {
                currentNPC = npc;
                isSceneChanger = other.CompareTag(sceneChangeNPCTag);
                StartCoroutine(StartDialogue());
                other.enabled = false;
            }
        }
    }

    private IEnumerator StartDialogue()
    {
        AudioManager.instance.Play("StartDialogue");
        DisablePlayerMovement();
        yield return new WaitForSeconds(dialogueStartDelay);
        
        currentNPC.StartDialogue(this); // Pass reference to Interaction
    }

    public void OnDialogueComplete()
    {
        if (isSceneChanger && !string.IsNullOrEmpty(postDialogueSceneToLoad))
        {
            StartCoroutine(LoadSceneAfterDelay(0.1f));
        }
        else
        {
            EnablePlayerMovement();
        }
        currentNPC = null;
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