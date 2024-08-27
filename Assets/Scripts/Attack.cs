using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private CustomInput input = null;
    public WeaponSwap weaponSwap;
    private Rigidbody2D rb = null;

    [Header("Attacking Parameters")]
    public GameObject Melee;
    [SerializeField] private float attackTimer = 0.1f;
    [SerializeField] private float attackCooldown = 0.05f;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool canAttack = true;

    
    [Header("Shooting Parameters")]
    public GameObject Bullet;
    public GameObject gunPoint;
    [SerializeField] private float shootTimer = 0.1f;
    // [SerializeField] private float shootCooldown = 0.05f;
    [SerializeField] public float fireForce = 10f;
    [SerializeField] public float recoilForce = 50f;
    [SerializeField] private bool isShooting;
    [SerializeField] private bool canShoot = true;

    private void Awake(){
        input = new CustomInput();
        Melee.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable(){
        input.Enable(); 
        input.Player.Attack.performed += OnAttackPerformed;
        input.Player.Shoot.performed += OnShootPerformed;
    }
    private void OnDisable(){
        input.Disable();
        input.Player.Attack.performed -= OnAttackPerformed;
        input.Player.Shoot.performed -= OnShootPerformed;
    }
    private void OnAttackPerformed(InputAction.CallbackContext value){
        if(canAttack && !isAttacking){
            if(weaponSwap.gunForm == false){
                StartCoroutine(DoAttack());
            }
            else{
                StartCoroutine(DoShoot());
            }
            
        }
    }
    private void OnShootPerformed(InputAction.CallbackContext value){
        Debug.Log("RIGHT CLICK");
        if(canShoot && !isShooting){
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
        Debug.Log("BANG");
        isShooting = true;
        canShoot = false;
        GameObject intBullet = Instantiate(Bullet, gunPoint.transform.position, gunPoint.transform.rotation);
        // MouseRotation mouseRotation = gunPoint.GetComponent<MouseRotation>();
    
        if(weaponSwap.gunForm == true){
            intBullet.GetComponent<Rigidbody2D>().AddForce(gunPoint.transform.right * fireForce, ForceMode2D.Impulse);
        } else {
            intBullet.GetComponent<Rigidbody2D>().AddForce(-gunPoint.transform.right * fireForce, ForceMode2D.Impulse);
    
            // Log the applied velocity for debugging
            Vector2 recoilVelocity = gunPoint.transform.right * recoilForce;
            
            // Apply recoil velocity to the player and set recoil flag
            rb.velocity += recoilVelocity;
            GetComponent<PlayerMovement>().isRecoiling = true;
            
            // Reset recoil flag after a short delay
            yield return new WaitForSeconds(0.1f);  // Adjust this duration as needed
            GetComponent<PlayerMovement>().isRecoiling = false;
        }
        
        Destroy(intBullet, 2f);
        yield return new WaitForSeconds(shootTimer);
        isShooting = false;
        canShoot = true;
    }


}
