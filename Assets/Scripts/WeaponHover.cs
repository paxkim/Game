using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHover : MonoBehaviour
{
    public GameObject player;
    public WeaponSwap weaponSwap; // Reference to the WeaponSwap script

    public Vector2 positionOffset = Vector2.zero; // Offset from the player's position
    public float gunFormRotation = 0.0f; // Additional rotation in degrees when gunForm is true
    public float meleeFormRotation = 0.0f; // Additional rotation in degrees when gunForm is false

    private CustomInput input = null;
    private Vector2 mousePosition;

    private void Awake()
    {
        input = new CustomInput();
    }

    private void OnEnable()
    {
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
        RotateToMouse();
    }

    private void RotateToMouse()
    {
        // Update the GameObject's position to the player's position with offset
        Vector3 offsetPosition = new Vector3(positionOffset.x, positionOffset.y, 0);
        transform.position = player.transform.position + offsetPosition;

        // Get the mouse position in screen coordinates
        Vector2 screenMousePosition = mousePosition;
        // Convert the screen coordinates to world coordinates
        Vector3 worldMousePosition3D = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePosition.x, screenMousePosition.y, Camera.main.nearClipPlane));
        // Convert to Vector2, ignoring the z-axis
        Vector2 worldMousePosition = new Vector2(worldMousePosition3D.x, worldMousePosition3D.y);
        // Calculate the direction from the player to the mouse position
        Vector2 direction = (worldMousePosition - (Vector2)player.transform.position).normalized;

        // Determine the rotation offset based on gunForm
        float rotationOffset = weaponSwap.gunForm ? gunFormRotation : meleeFormRotation;

        // Calculate the angle with the rotation offset
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}