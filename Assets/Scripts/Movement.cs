using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int moveSpeed;
    [SerializeField] private float changeCooldown;
    [SerializeField] private bool canInteract, canTeleport;
    [SerializeField] private Interactible interactibleScript = null;
    [SerializeField] private Volume ppProfile;
    [SerializeField] private Vignette vignette;
    //[SerializeField] private BoxCollider2D interactCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        ppProfile = GameManager.instance.GetPPVolume();
        if (ppProfile.profile.TryGet<Vignette>(out var vig))
        {
            vignette = vig;
        }
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
/*
        if (Input.GetKeyDown(KeyCode.I))
        {
            LevelManager.instance.ChangeLayout();
        }*/
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
        }else if (other.CompareTag("hol"))
        {
            print("Entered trigger");
            canTeleport = true;
            var chance = Random.Range(1, 101);
            var sanity = 0f;
            if (GameManager.instance.getSanity() > 80)
            {
                sanity = 80f;
            }
            else
            {
                sanity = GameManager.instance.getSanity();
            }
            if (chance <= 50 + ((80 - sanity) / 80f) * 20)
            {
                if (canTeleport)
                {
                    LeanTween.delayedCall(Random.Range(.25f, .5f), () => {
                        if (changeCooldown <= Time.time)
                        {
                            GameManager.instance.VigAnim(false);
                                LeanTween.value(gameObject, vignette.intensity.value, 1, 0.25f).setEaseOutExpo().setOnUpdate(
                                    (value) =>
                                    {
                                        vignette.intensity.value = value;
                                    }).setOnComplete(() =>
                                {
                                    LeanTween.value(gameObject, 1, Mathf.Lerp(0, .5f, (80 - sanity) / 80f), 0.25f).setEaseOutExpo().setOnUpdate(
                                        (value) =>
                                        {
                                            vignette.intensity.value = value;
                                        }).setOnComplete(()=>{GameManager.instance.VigAnim(true);});
                                });
                            LevelManager.instance.ChangeLayout();
                        }});
                }
                
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
        }else if (other.CompareTag("hol"))
        {
            print("Exit trigger");
            changeCooldown = Time.time + 1f;
            canTeleport = false;
        }
    }
    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("hol"))
        {
            Debug.Log("Still inside hol trigger");
        }
    }*/
}
