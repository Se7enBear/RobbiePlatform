using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        GameManager.RegisterSceneFader(this);
    }
    public void Fadeout()
    {
        anim.SetTrigger("Fade");
    }
}
