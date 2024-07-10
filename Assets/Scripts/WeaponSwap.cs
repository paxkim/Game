using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwap : MonoBehaviour
{
    [SerializeField] Sprite[] weaponSprites;
    private Sprite newSprite;
    private CustomInput input = null;
    public bool gunForm = false;

    // Reference to MouseRotation script
    public MouseRotation mouseRotation;

    private void Awake(){
        input = new CustomInput();
    }

    private void OnEnable(){
        input.Enable();
        input.Player.SwitchForm.performed += OnSwitchFormPerformed;
    }

    private void OnDisable(){
        input.Disable();
        input.Player.SwitchForm.performed -= OnSwitchFormPerformed;
    }

    private void OnSwitchFormPerformed(InputAction.CallbackContext context){
        gunForm = !gunForm;

        // Update the weapon sprite
        newSprite = gunForm ? weaponSprites[1] : weaponSprites[0];
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;

        // Update gunForm in MouseRotation
        if(mouseRotation != null){
            mouseRotation.gunForm = gunForm;
        }
    }
}
