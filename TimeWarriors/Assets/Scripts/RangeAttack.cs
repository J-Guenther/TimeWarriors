/*
RangeAttack inherits from Attack and controls all range attacks. Its methods FirstAttack and SecondAttack
are for creating bullet objects and applying force on them (so basically they are for shooting).
The methods will be called by UserControl or AiControl.
*/
using UnityEngine;


public class RangeAttack : Attack {

    private Animator anim;

    [SerializeField] private float bulletSpeed = 500f;                  // Speed of the bullet
    [SerializeField]
    private float secondSpeed = 300f;
    [SerializeField] private float timeBetweenBullets = 0.15f;          // The time between each shot.
    [SerializeField] private GameObject bulletPrefabFirst;              // Prefab of the bullet
    [SerializeField] private GameObject bulletPrefabSecond;             // Prefab of the bullet
    

    private float timer;                                    // A timer to determine when to fire.
    private Transform firePoint;                                        // This is where the bullet 'comes out'
    private PlayerCharacter.PlayerController playerController;  // 
    private AudioManager audioManager;

    public bool isGrenade = false;

    // Sounds
    public string shoot1;
    public string shoot2;


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
        // If player is attacking
        if (isAttacking) {
            anim.SetBool("Shoot", isAttacking);
            audioManager.Play(shoot1);
            // Instantiate the bullet
            GameObject bullet = (GameObject)Instantiate(bulletPrefabFirst, firePoint.position, Quaternion.identity);
            // Set the damage of the bullet
            bullet.GetComponent<BulletDamage>().SetTagNotToHit(gameObject.tag);
            bullet.GetComponent<BulletDamage>().SetDamage(damageFirst);

            // Aply force
            if (playerController.getFacingRight()) {
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.right * bulletSpeed);
            }
            else {
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.left * bulletSpeed);
            }
            anim.SetBool("Shoot", false);
        }
    }


    // Secondary Attack for range weapons
    public override void SecondAttack(bool isAttacking) {

        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        if (isAttacking && timer >= timeBetweenBullets) {
            // Reset the timer.
            timer = 0f;
            anim.SetTrigger("Second");
            // anim.SetTrigger("Attack");          
        }
    }

    public void LaunchSecond() {
        // Instantiate the bullet
        GameObject bullet = (GameObject)Instantiate(bulletPrefabSecond, firePoint.position, Quaternion.identity);

        if (isGrenade) {
            // Set the damage of the bullet
            bullet.GetComponent<Grenade>().setDamage(damageSecond);

            // Aply force
            if (playerController.getFacingRight()) {
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.right * secondSpeed);
            }
            else {
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.left * secondSpeed);
            }
        }
        else {
            audioManager.Play(shoot2);
            bullet.GetComponent<BulletDamage>().SetTagNotToHit(gameObject.tag);
            bullet.GetComponent<BulletDamage>().SetDamage(damageSecond);

            if (playerController.getFacingRight()) {
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.right * secondSpeed);
            }
            else {
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.left * secondSpeed);
            }
        }

        
    }
}
