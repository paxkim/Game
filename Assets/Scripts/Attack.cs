using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private CustomInput input = null;

    [Header("Attacking Parameters")]
    public GameObject Melee;
    [SerializeField] private float attackTimer = 0.1f;
    [SerializeField] private float attackCooldown = 0.05f;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool canAttack = true;

    
    [Header("Shooting Parameters")]
    public GameObject bullet;
    [SerializeField] private float shootTimer = 0.1f;
    [SerializeField] private float shootCooldown = 0.05f;
    [SerializeField] private bool isShooting;
    [SerializeField] private bool canShoot = true;

    private void Awake(){
        input = new CustomInput();
        Melee.SetActive(false);
    }

    private void OnEnable(){
        input.Enable(); 
        input.Player.Attack.performed += OnAttackPerformed;
        input.Player.Attack.performed += OnShootPerformed;
    }
    private void OnDisable(){
        input.Disable();
        input.Player.Attack.performed -= OnAttackPerformed;
        input.Player.Attack.performed -= OnShootPerformed;
    }
    private void OnAttackPerformed(InputAction.CallbackContext value){
        if(canAttack && !isAttacking){
            StartCoroutine(DoAttack());
        }
    }
    private void OnShootPerformed(InputAction.CallbackContext value){
        if(canAttack && !isAttacking){
            StartCoroutine(DoShoot());
        }
    }

    private IEnumerator DoAttack(){
        Debug.Log("SCHWING");
        isAttacking = true;
        canAttack = false;
        Melee.SetActive(true);
        Melee.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.25f);
        yield return new WaitForSeconds(attackTimer);
        Melee.SetActive(false);
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;


    }
    private IEnumerator DoShoot(){
        yield return new WaitForSeconds(attackTimer);
    }


}
