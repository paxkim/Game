using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;

    [SerializeField] private float moveSpeed = 10f;

    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 50f;
    [SerializeField] private float dashingTime = 0.15f;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool canDash = true;
    [SerializeField] private float dashCooldown = 3f; // Cooldown time between dashes

    private void Awake() {
        input = new CustomInput();
        rb = GetComponent<Rigidbody2D>();

    }
    private void OnEnable() {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Dash.performed += OnDashPerformed; 
    }

    private void OnDisable() {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Dash.performed -= OnDashPerformed;
    }
    
    private void FixedUpdate() {
        float speed = moveVector.magnitude;
        animator.SetFloat("Speed", speed);

        // Flip the character based on movement direction
        if (moveVector.x != 0) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(moveVector.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        if (!isDashing) {
            rb.velocity = moveVector * moveSpeed;
        }
    }    
    private void OnMovementPerformed(InputAction.CallbackContext value){
        moveVector = value.ReadValue<Vector2>();
    }
    private void OnMovementCancelled(InputAction.CallbackContext value){
        moveVector = Vector2.zero;
    }
    private void OnDashPerformed(InputAction.CallbackContext value){
        if (canDash && !isDashing){
            StartCoroutine(Dash());
        }
    }
    private IEnumerator Dash(){
        isDashing = true;
        canDash = false;
        Debug.Log("Can't Dash");
        Vector2 originalVelocity = rb.velocity;
        rb.velocity = moveVector.normalized * dashingVelocity;
        yield return new WaitForSeconds(dashingTime);
        rb.velocity = originalVelocity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
        Debug.Log("Can Dash");
    }
}
