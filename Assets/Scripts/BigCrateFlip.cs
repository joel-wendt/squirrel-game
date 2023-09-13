using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCrateFlip : MonoBehaviour
{
    private Animator anim;
    private bool boxFlipDone = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !boxFlipDone)
        {
            boxFlipDone = true;
            anim.SetTrigger("TipOverBox");
        }
    }
}
