using UnityEngine;

public class PowerUpHealth : MonoBehaviour {

    public SpawnManager spawnManager;
    [SerializeField] private int healamount = 1;
    private string tagNotToHit = "Untagged";
    private bool active = false;


    private void Start() {
        spawnManager = GetComponentInParent<SpawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(tagNotToHit) && active==false && other.gameObject.layer != 11 && other.gameObject.layer != 12)
        {
            // Can't be used again after flag has ben set to true. Prevents bug where health is restored multiple times by the same health kit
            active = true;

            other.SendMessage("ApplyHealth", healamount, SendMessageOptions.DontRequireReceiver);

            spawnManager.numPU --;

            //Reset Countdown
            spawnManager.CountdownReset();

            //Destroy PowerUp
            Destroy(gameObject);            
        }
    }
}