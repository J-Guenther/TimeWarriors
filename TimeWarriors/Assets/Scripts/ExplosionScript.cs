using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{

    public SpawnManager spawnManager;
    [SerializeField] private int damageamount = 2;
    private string tagNotToHit = "Untagged";
    private bool active = false;
    public float explosiondelay = 3;
    public Collider2D explosionradius;

    //Child into variable
    public GameObject Explosionchild;

    private void Start(){
        spawnManager = GetComponentInParent<SpawnManager>();
        Explosionchild.SetActive(false);
    }

 
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(tagNotToHit) && active == false) {
            //For only using one Collider
            active = true;
            StartCoroutine(Explosion());

        }
    }


    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosiondelay);
        Explosionchild.SetActive(true);
        yield return new WaitForSeconds(1);
        Collider2D[] cols = Physics2D.OverlapBoxAll(explosionradius.bounds.center, explosionradius.bounds.extents, 0.0f);

        foreach (Collider2D c in cols) {
            if (c.transform.root == transform)
                continue;
            c.SendMessageUpwards("ApplyDamage", damageamount, SendMessageOptions.DontRequireReceiver);
        }
       
        spawnManager.numPU--; 
        
        Destroy(gameObject);
    }

    

}