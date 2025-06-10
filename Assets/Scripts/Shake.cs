using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator camAnim;

    public static Shake instance;

    private void Awake()
    {
        instance = this;
    }

    public void DoShake()
    {
        camAnim.SetTrigger("shake");
    }
}
