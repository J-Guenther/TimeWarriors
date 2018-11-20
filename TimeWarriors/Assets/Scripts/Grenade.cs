using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {
    
    [SerializeField]
    private float damage = 3f; // damage value with the default of 0 damage. The real damage value is provided with the setter Method
    public float explosiondelay = 3;
    public CircleCollider2D explosionradius;
    public float fallMultiplier0 = 2.5f;
    private Rigidbody2D rb2d;
    public float camShakeAmt = 0.1f; // Handle Camera shaking
    CameraShake camShake;
    public GameObject ExplosionPrefab;
    private AudioManager audioManager;
    private GameManager gameManager;
    public string explosionSound;

    

    private void Start() {

        explosionradius = GetComponent<CircleCollider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        audioManager = GameObject.Find("_AudioManager").GetComponent<AudioManager>();

        //Get AudioManager
        gameManager = GameObject.Find("_GM").GetComponent<GameManager>();
        camShake = gameManager.GetComponent<CameraShake>();

        StartCoroutine(Explosion());
    }

    private void FixedUpdate() {
        if (rb2d.velocity.y < 0) {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier0 - 1) * Time.deltaTime;
        }
    }

    

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosiondelay);

        // http://answers.unity3d.com/questions/634320/how-to-get-correct-scale-for-circlecollider2d-radi.html
        Collider2D[] cols = Physics2D.OverlapCircleAll(new Vector2(explosionradius.transform.position.x, explosionradius.transform.position.y), (transform.localScale.x*explosionradius.radius), LayerMask.GetMask("Player"));
        // Instantiate the bullet
        GameObject bullet = (GameObject)Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        foreach (Collider2D c in cols)
        {
            if (c == null)
                continue;
            if (c.transform.root == transform)
                continue;
            Debug.Log(c.name);
            c.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }

        audioManager.Play(explosionSound);
        camShake.Shake(camShakeAmt, 0.2F);

        //Destroy PowerUp
        Destroy(gameObject);
    }

    public void setDamage(float d) { damage = d; }
}