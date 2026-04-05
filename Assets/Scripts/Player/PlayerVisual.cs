using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("isright", Player.Instance.IsRunningRight());
        animator.SetBool("isleft", Player.Instance.IsRunningLeft());
        animator.SetBool("isIdle", Player.Instance.IsRunningRight() == Player.Instance.IsRunningLeft());
    }

}
