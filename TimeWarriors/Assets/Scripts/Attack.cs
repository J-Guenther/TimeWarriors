/*
https://unity3d.com/de/learn/tutorials/topics/scripting/overriding
https://unity3d.com/de/learn/tutorials/topics/scripting/inheritance
This is the base class for all attacks. In TimeWarriors the player can make an attack of type FirstAttack
and an attack of type SecondAttack. Both methods are virtual and will be overwritten by the child classes
RangeAttack and MeleeAttack.
*/

using UnityEngine;

// Abstract base class of Attacks
abstract public class Attack : MonoBehaviour {

    public int damageFirst;
    public int damageSecond;
    //[SerializeField] private LayerMask notToHit;

    virtual public void FirstAttack(bool isAttacking) { }
    virtual public void SecondAttack(bool isAttacking) { }
}
