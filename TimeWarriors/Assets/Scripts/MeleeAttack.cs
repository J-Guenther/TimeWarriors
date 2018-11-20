// https://www.youtube.com/watch?v=xwPahXLpNh8&t=488s
// Final choice: https://www.youtube.com/watch?v=mvVM1RB4HXk

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack {

    private Animator anim;
    
    public Collider2D attackCollider;

    [SerializeField]
    private float timeBetweenAttacks = 0.01f;
    private float timer;

    private bool block;
    [SerializeField]
    private float timeBetweenBlocks = 2.0f;
    private float timerBlock;
    [SerializeField]
    private float blockLength = 3.0f;
    private float timerBlockLength = 0.0f;

    private PlayerHealth playerHealth;
    private AudioManager audioManager;

    // Sounds
    public string swishSound;
    public string hitSound;
    public string blockSound;


    // Use this for initialization
    void Awake () {
        anim = transform.GetComponent<Animator>();
        playerHealth = transform.GetComponent<PlayerHealth>();
        //Get AudioManager
        audioManager = GameObject.Find("_AudioManager").GetComponent<AudioManager>();
    }
	

    // Primary Attack for melee weapons
    override public void FirstAttack(bool isAttacking) {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If player is attacking
        if (isAttacking && timer >= timeBetweenAttacks && !block)
        {
            anim.SetTrigger("Melee");
            audioManager.Play(swishSound);
            // Reset the timer.
            timer = 0f;
            Collider2D[] cols = Physics2D.OverlapBoxAll(attackCollider.bounds.center, attackCollider.bounds.extents, 0.0f ,LayerMask.GetMask("Hitbox"));
            foreach(Collider2D c in cols)
            {
                if (c.transform.root == transform)
                    continue;
                c.SendMessageUpwards("ApplyDamage", damageFirst, SendMessageOptions.DontRequireReceiver);
                audioManager.Play(hitSound);
            }
        } 
    }

    // Block
    public override void SecondAttack(bool isAttacking) {
        timerBlock += Time.deltaTime;

        if(!block && timerBlock >= timeBetweenBlocks && isAttacking){
            block = true;
            playerHealth.SetBlockStatus(block);
            timerBlockLength = blockLength;
            anim.SetBool("Block", block);
            audioManager.Play(blockSound);
        }
        if (block){
            timerBlockLength -= Time.deltaTime;
            if(timerBlockLength <= 0.0f){
                block = false;
                playerHealth.SetBlockStatus(block);
                timerBlock = 0.0f;
                anim.SetBool("Block", block);
            }
        }
    }
}
