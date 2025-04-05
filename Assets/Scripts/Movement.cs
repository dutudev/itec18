using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        
    }

    public void Move()
    {
        Vector2 moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAxis.Normalize();
        print(moveAxis.y);
        if (moveAxis == Vector2.zero)
        {
            animator.SetBool("Return", true);
            animator.SetBool("MoveUpDown", false);
            animator.SetBool("MoveLeftRight", false);
            animator.SetBool("MoveUp", false);
        }else if (MathF.Abs(moveAxis.x) < MathF.Abs(moveAxis.y))
        {
            if (moveAxis.y < 0)
            {
                animator.SetBool("MoveUp", false);
            }
            else
            {
                animator.SetBool("MoveUp", true);
            }
            animator.SetBool("Return", false);
            animator.SetBool("MoveUpDown", true);
            animator.SetBool("MoveLeftRight", false);
        }
        else
        {
            animator.SetBool("Return", false);
            animator.SetBool("MoveUpDown", false);
            animator.SetBool("MoveLeftRight", true);
            animator.SetBool("MoveUp", false);
            if (moveAxis.x <= 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }

        
        rb.velocity = moveAxis * moveSpeed;
    }
}
