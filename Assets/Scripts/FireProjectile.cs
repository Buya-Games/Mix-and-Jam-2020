using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float maxPower;
    [SerializeField] float angle;
    float gravity;
    [SerializeField] float height;
    Manager _manager;
    void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
        _manager = FindObjectOfType<Manager>();
    }

    public void Fire(Vector3 from, Vector3 target){
        Initialize();
        from.y = 3;
        transform.position = from;
        rb.velocity = Vector3.zero;
        rb.velocity = CalculateTraj(target);
    }

    Vector3 CalculateTraj(Vector3 where){
        height = (transform.position - where).sqrMagnitude/10; 
        float disY = where.y - transform.position.y;
        Vector3 disXZ = new Vector3(where.x - transform.position.x, 0, where.z - transform.position.z);
        float x = height / disXZ.magnitude;

        Vector3 velY = Vector3.up * Mathf.Sqrt(-2 * gravity * x);
        Vector3 velXZ = disXZ / (Mathf.Sqrt(-2*x/gravity) + Mathf.Sqrt(2*(disY-x)/gravity));
        return velXZ + velY;
    }

    void OnCollisionEnter(Collision col){
        if (col.gameObject.layer == 12){
            CreatureLogic creature = col.gameObject.GetComponent<CreatureLogic>();
            float hitDamage = Random.Range(4f,6f);
            creature.NewHealth(-hitDamage);
            _manager.ui.HitGUI(col.transform.position, hitDamage, Color.green);
            _manager.particles.EnemyHit(col.transform.position);
            StartCoroutine(HitDisplay(col.gameObject, creature._origColor));
        } else {
            Destroy(this.gameObject);
            _manager.particles.BombHit(transform.position);
        }
    }

    IEnumerator HitDisplay(GameObject who, Color origColor){
        MeshRenderer rendy = who.GetComponentInChildren<MeshRenderer>();
        rendy.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        rendy.material.color = origColor;
        Destroy(this.gameObject);
        _manager.particles.BombHit(transform.position);
    }
}
