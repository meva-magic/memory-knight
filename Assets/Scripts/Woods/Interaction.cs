using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("References")]
    public PlayerMove playerMoveScript;
    public WoodsUI woodsUI;
    public Button nextButton;
    public Button endButton;
    public Button skipButton;
    public GameObject dialogueIndicator;

    [Header("Settings")]
    public float dialogueStartDelay = 0.3f;
    public string regularNPCTag = "NPC";
    public string sceneChangeNPCTag = "SceneChangeNPC";
    public string postDialogueSceneToLoad;

    [Header("Input Settings")]
    public InputActionReference nextAction;
    public InputActionReference endAction;
    public InputActionReference skipAction;

    [Header("Events")]
    public UnityEvent OnDialogueStarted;
    public UnityEvent OnDialogueEnded;

    private bool isInteractingWithSceneChanger = false;
    private NPC currentNPC;
    private bool isInDialogue = false;
    private float lastInputTime;
    public float inputCooldown = 0.2f;

    public static Interaction instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeButtons();
            InitializeInput();
            if (dialogueIndicator != null) dialogueIndicator.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeButtons()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextPressed);
            nextButton.gameObject.SetActive(false);
        }

        if (endButton != null)
        {
            endButton.onClick.RemoveAllListeners();
            endButton.onClick.AddListener(OnEndPressed);
            endButton.gameObject.SetActive(false);
        }

        if (skipButton != null)
        {
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(OnSkipPressed);
            skipButton.gameObject.SetActive(false);
        }
    }

    private void InitializeInput()
    {
        if (nextAction != null)
            nextAction.action.performed += ctx => OnNextPressed();

        if (endAction != null)
            endAction.action.performed += ctx => OnEndPressed();

        if (skipAction != null)
            skipAction.action.performed += ctx => OnSkipPressed();
    }

    private void OnEnable()
    {
        if (nextAction != null) nextAction.action.Enable();
        if (endAction != null) endAction.action.Enable();
        if (skipAction != null) skipAction.action.Enable();
    }

    private void OnDisable()
    {
        if (nextAction != null) nextAction.action.Disable();
        if (endAction != null) endAction.action.Disable();
        if (skipAction != null) skipAction.action.Disable();
    }

    private void Start()
    {
        if (playerMoveScript == null)
            playerMoveScript = FindAnyObjectByType<PlayerMove>(FindObjectsInactive.Include);
        
        if (woodsUI == null)
            woodsUI = FindAnyObjectByType<WoodsUI>(FindObjectsInactive.Include);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInDialogue || other == null) return;

        if (other.CompareTag(regularNPCTag) || other.CompareTag(sceneChangeNPCTag))
        {
            NPC npc = other.GetComponentInParent<NPC>();
            if (npc != null)
            {
                // Disable movement immediately
                DisablePlayerMovement();
                
                // Show indicator
                if (dialogueIndicator != null) dialogueIndicator.SetActive(true);
                
                StartCoroutine(StartInteractionAfterDelay(npc, other.CompareTag(sceneChangeNPCTag)));
            }
        }
    }

    private IEnumerator StartInteractionAfterDelay(NPC npc, bool isSceneChanger)
    {
        yield return new WaitForSeconds(dialogueStartDelay);
        
        // Hide indicator when dialogue starts
        if (dialogueIndicator != null) dialogueIndicator.SetActive(false);
        
        StartInteraction(npc, isSceneChanger);
        npc.GetComponentInChildren<Collider2D>().enabled = false;
    }

    private void StartInteraction(NPC npc, bool isSceneChanger)
    {
        isInDialogue = true;
        isInteractingWithSceneChanger = isSceneChanger;
        currentNPC = npc;
        
        currentNPC.ResetDialogue();
        currentNPC.Initialize(this, isSceneChanger);
        
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        OnDialogueStarted?.Invoke();
        
        currentNPC.StartDialogue();
        
        while (currentNPC.IsDialogueActive)
        {
            yield return null;
        }
        
        HandleDialogueCompletion();
    }

    public void OnNextPressed()
    {
        if (Time.time < lastInputTime + inputCooldown) return;
        lastInputTime = Time.time;
        
        if (currentNPC == null || !isInDialogue) return;

        if (currentNPC.IsTyping)
        {
            currentNPC.CompleteCurrentLine();
        }
        else
        {
            currentNPC.AdvanceDialogue();
        }
    }

    public void OnEndPressed()
    {
        if (Time.time < lastInputTime + inputCooldown) return;
        lastInputTime = Time.time;
        
        if (currentNPC != null && isInDialogue)
            currentNPC.EndDialogue();
    }

    public void OnSkipPressed()
    {
        if (Time.time < lastInputTime + inputCooldown) return;
        lastInputTime = Time.time;
        
        if (currentNPC != null && isInDialogue)
            currentNPC.SkipDialogue();
    }

    public void UpdateButtons(bool showNext, bool showEnd)
    {
        if (nextButton != null) nextButton.gameObject.SetActive(showNext);
        if (endButton != null) endButton.gameObject.SetActive(showEnd);
        if (skipButton != null) skipButton.gameObject.SetActive(true);
    }

    private void HandleDialogueCompletion()
    {
        isInDialogue = false;
        OnDialogueEnded?.Invoke();
        
        if (isInteractingWithSceneChanger && !string.IsNullOrEmpty(postDialogueSceneToLoad))
            LoadNextScene();
        else
            EnablePlayerMovement();
    }

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(postDialogueSceneToLoad))
            SceneManager.LoadScene(postDialogueSceneToLoad);
    }

    public void DisablePlayerMovement()
    {
        if (playerMoveScript != null) playerMoveScript.DisableMovement();
        if (woodsUI != null) woodsUI.DisableJoystick();
    }

    public void EnablePlayerMovement()
    {
        if (playerMoveScript != null) playerMoveScript.EnableMovement();
        if (woodsUI != null) woodsUI.EnableJoystick();
    }

    public void OnDialogueComplete()
    {
        HandleDialogueCompletion();
    }
}