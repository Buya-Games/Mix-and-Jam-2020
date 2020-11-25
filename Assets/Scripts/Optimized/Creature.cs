using System.Collections;
using UnityEngine;
public class Creature : MonoBehaviour, IDamageable {
    // public struct Statz {
    //     public string MyName;
    //     public float Strength, Speed, Range, Tech, Heal, MaxHealth;
    // }

    protected Stats myStats;
    protected float health;
    protected bool dead;
    protected IEnumerator currentAction;

    protected virtual void Start(){
        health = myStats.MaxHealth;
    }

    public void TakeHit (float damage){
        health -= damage;

        if (health <= 0 && !dead){
            Die();
        }
    }

    public void Die(){
        dead = true;
    }
}
