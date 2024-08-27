using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MouseRotation : MonoBehaviour
{
    public GameObject player;
    public WeaponSwap weaponSwap; // Reference to the WeaponSwap script

    private CustomInput input = null;
    private Vector2 mousePosition;
    public float distanceFromPlayer = 2.0f;
    [SerializeField] private int reverse = 1;

    private void Awake(){
        input = new CustomInput();
    }

    private void OnEnable(){
        input.Enable();
        input.Player.MousePosition.performed += OnMousePositionPerformed;
    }
    private void OnDisable()
    {
        input.Disable();
        input.Player.MousePosition.performed -= OnMousePositionPerformed;
    }

    private void OnMousePositionPerformed(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        RotateSquare();
    }

    private void RotateSquare()
    {
    // Get the mouse position in screen coordinates
    Vector2 screenMousePosition = mousePosition;
    // Convert the screen coordinates to world coordinates
    Vector3 worldMousePosition3D = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePosition.x, screenMousePosition.y, Camera.main.nearClipPlane));
    // Convert to Vector2, ignoring the z-axis
    Vector2 worldMousePosition = new Vector2(worldMousePosition3D.x, worldMousePosition3D.y);
    // Calculate the direction from the player to the mouse position
    Vector2 direction = (worldMousePosition - (Vector2)player.transform.position).normalized;
    // Calculate the target position at the specified distance from the player
    Vector2 targetPosition;
    if(weaponSwap.gunForm){
        targetPosition = (Vector2)player.transform.position + (direction*reverse) * distanceFromPlayer;
    }
    else{
        targetPosition = (Vector2)player.transform.position - (direction*reverse) * distanceFromPlayer;
    }
    // Set the square's position to the target position
    transform.position = targetPosition;

    // Set the square's rotation to face the direction
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

