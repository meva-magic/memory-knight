using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject 
{
    public string Name;
    
    public string[] dialogueLines;
    public bool[] autoProgressLines;

    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;

    public AudioClip voice;
    public float voiceVolume = 1f;
    public float voicePitch = 1f;
}
