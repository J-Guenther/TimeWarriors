/*
BulletDamage is attached to all bullets. It flies with the bullet and checks for colliders.
If they find colliders and their parent objects have a method called ApplyDamage() then damage
will be applied to them.
*/

using UnityEngine;

public class BulletDamage : MonoBehaviour {

    private float damage = 0f; // damage value with the init value of 0 damage. The real damage value is provided with the setter Method
    private string tagNotToHit = "Untagged";

    const int HITBOX_LAYER = 11;
    const int ATTACK_COLLIDER_LAYER = 12;
    const int BULLETS = 13;


    // Checks if the Bullet hits a collider
    private void OnTriggerEnter2D (Collider2D other) {
        if (!other.CompareTag(tagNotToHit) && other.gameObject.layer != HITBOX_LAYER && other.gameObject.layer != ATTACK_COLLIDER_LAYER && other.gameObject.layer != BULLETS) { 
            // TODO get rid of string references
            other.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

    // Setter is called by whatever instantiates this objects (usually the RangeAttack Class)
    public void SetTagNotToHit(string s) {tagNotToHit = s;}
    public void SetDamage(float d) {damage = d;}
}
