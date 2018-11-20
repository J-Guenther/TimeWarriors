using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoneAgeAttack : Attack {

    private Animator anim;

    [SerializeField] private float bulletSpeed = 500f;                  // Speed of the bullet
    [SerializeField] private float timeBetweenBullets = 0.15f;          // The time between each shot.
    [SerializeField] private GameObject bulletPrefabFirst;              // Prefab of the bullet
    [SerializeField] private GameObject bulletPrefabSecond;             // Prefab of the bullet

    public Collider2D attackCollider;

    [SerializeField]
    private float timeBetweenAttacks = 0.01f;
    private float timerMelee;
    private float timer;                                    // A timer to determine when to fire.
    //private float timeToFire = 0;
    private Transform firePoint;                                        // This is where the bullet 'comes out'
    private PlayerCharacter.PlayerController playerController;  // 
    private AudioManager audioManager;

    // Sounds
    public string swishSound;
    public string hitSound;
    public string throwSound;


    void Awake() {
        // Get the firePoint
        firePoint = transform.Find("FirePoint");
        if(firePoint == null) {
            Debug.LogError("Character is missing a FirePoint Empty. It can not shoot!");
        }
        // Get the PlayerController
        playerController = GetComponent<PlayerCharacter.PlayerController>();
        // Get Animator
        anim = transform.GetComponent<Animator>();

        //Get AudioManager
        audioManager = GameObject.Find("_AudioManager").GetComponent<AudioManager>();
    }


    // Primary Attack for range weapons
    override public void FirstAttack(bool isAttacking) {
        // Add the time since Update was last called to the timer.
        timerMelee += Time.deltaTime;


        // If player is attacking
        if (isAttacking && timerMelee >= timeBetweenAttacks)
        {
            anim.SetTrigger("Melee");
            audioManager.Play(swishSound);
            // Reset the timer.
            timerMelee = 0f;
            Collider2D[] cols = Physics2D.OverlapBoxAll(attackCollider.bounds.center, attackCollider.bounds.extents, 0.0f, LayerMask.GetMask("Hitbox"));
            foreach (Collider2D c in cols)
            {
                if (c.transform.root == transform)
                    continue;
                c.SendMessageUpwards("ApplyDamage", damageFirst, SendMessageOptions.DontRequireReceiver);
                audioManager.Play(hitSound);
            }
        }
    }


    // Secondary Attack for range weapons
    public override void SecondAttack(bool isAttacking) {

        timer += Time.deltaTime;

        if (isAttacking && timer >= timeBetweenBullets) {
            // Reset the timer.
            timer = 0f;
            anim.SetTrigger("Second");            
        }
    }

    public void LaunchSecond() {
        // Instantiate the bullet
        GameObject bullet = (GameObject)Instantiate(bulletPrefabSecond, firePoint.position, Quaternion.identity);
        audioManager.Play(throwSound);
        // Set the damage of the bullet
        bullet.GetComponent<BulletDamage>().SetTagNotToHit(gameObject.tag);
        bullet.GetComponent<BulletDamage>().SetDamage(damageSecond);

        // Aply force
        if (playerController.getFacingRight()){
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.right * bulletSpeed);
        }
        else {
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.left * bulletSpeed);
        }
    }
}
