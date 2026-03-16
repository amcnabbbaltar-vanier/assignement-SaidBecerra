using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
        CharacterMovement.OnDoubleJump += HandleDoubleJump;
    }

    void OnDestroy()
    {
        CharacterMovement.OnDoubleJump -= HandleDoubleJump;
    }

    void Update()
    {
        animator.SetFloat("CharacterSpeed", Mathf.Abs(movement.moveInput));
        animator.SetBool("isGrounded", movement.isGrounded);
    }

    void HandleDoubleJump()
    {
        animator.SetTrigger("doFlip");
    }
}