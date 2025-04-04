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
    [SerializeField] private bool canInteract;
    [SerializeField] private Interactible interactibleScript = null;
    [SerializeField] private BoxCollider2D interactCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract && interactibleScript != null)
        {
            interactibleScript.Interact();
        }
    }

    public void Move()
    {
        Vector2 moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveAxis.Normalize();
        //print(moveAxis.y);
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        //BoxCollider2D[] col = other.gameObject.GetComponents<BoxCollider2D>();
        if (other.CompareTag("Interactible"))
        {
            /*foreach(var ind in col)
            {
                if (ind.isTrigger == true)
                    Debug.Log("vede butonu");///Destroy(ind.gameObject);
            }*/
            CanvasAnims.instance.AnimateInteract();
            interactibleScript = other.GetComponent<Interactible>();
            canInteract = true;
            if (!interactibleScript.CanInteract())
            {
                canInteract = false;
                CanvasAnims.instance.SetInteractText(interactibleScript.GetRestriction());
            }
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactible"))
        {
            canInteract = false;
            interactibleScript = null;
            CanvasAnims.instance.AnimateEndInteract();
        }
    }
}
