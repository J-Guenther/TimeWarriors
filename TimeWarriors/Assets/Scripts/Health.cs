/*
Health controls everything that has to do something with the health of a Player or an Object
*/

using UnityEngine;

public class Health : MonoBehaviour {

    public int health = 3;
    
    public virtual void ApplyDamage(int damage) {
        health -= damage;
        if(health <= 0) { Destroy(gameObject); }
    }
}
