using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_FALLING = "IsFall";
    private Animator animator;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_GROUNDED, player.IsGrounded());
        animator.SetBool(IS_FALLING, player.IsFall());
    }
}
