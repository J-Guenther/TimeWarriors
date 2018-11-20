using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerCharacter.UserControl))]
public class PlayerHealth : Health {

    public int currentHealth;
    public Slider healthBar;
    public bool isDead = false;  // Whether the player is dead.
    public GameObject blackHolePrefab;


    private PlayerCharacter.UserControl userControl;
    private bool damaged = false;  
    private bool block = false;
    private GameManager gameManager;
    private UImanager uiManager;
    private AudioManager audioManager;
    public GameObject spawnEffectPrefab;


    void Awake() {
        // Setting up the references
        userControl = GetComponent<PlayerCharacter.UserControl>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        uiManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UImanager>();
        //Get AudioManager
        audioManager = GameObject.Find("_AudioManager").GetComponent<AudioManager>();

    }

    void Start() {
        // Healthbar needs to be asigned in start because during Awake the Objecttag has not been set
        if (gameObject.tag == "Player2") {
            healthBar = uiManager.p2Healthbar;
        }
        else {
            healthBar = uiManager.p1Healthbar;
        }
        // Set the inital health of the player
        currentHealth = health;
        healthBar.value = CalculateHealth();
        GameObject spawnEffect = (GameObject)Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity);
    }

    public override void ApplyDamage(int damage) {
        if (block)
            return;
        // Set the damaged flag (for screen flash or camera shake)
        damaged = true;

        // Damage
        currentHealth -= damage;

        // UI
        healthBar.value = CalculateHealth();

        audioManager.Play("Damage");

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }

        damaged = false;
    }

    private float CalculateHealth() {
        return (float)currentHealth / (float)health;
    }

    void Death() {
        isDead = true;
        userControl.DisableControl();

        GameObject blackHole = (GameObject)Instantiate(blackHolePrefab, transform.position, Quaternion.identity);
        audioManager.Play("Death");
        Destroy(gameObject);
        gameManager.TimeTravel(userControl.GetPlayerID());
    }

    public void ApplyHealth(int heal) {
        audioManager.Play("Health");

        //Heal
        currentHealth += heal;

        if (currentHealth > health)
        {
            currentHealth = health;
        }
        healthBar.value = CalculateHealth();
    }
    public void SetBlockStatus(bool isBlocking) { block = isBlocking; }
}

