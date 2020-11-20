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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
    }

    public void Fire(Vector3 where){
        rb.velocity = Vector3.zero;
        rb.velocity = CalculateTraj(where);
    }

    Vector3 CalculateTraj(Vector3 where){
        float disY = where.y - transform.position.y;
        Vector3 disXZ = new Vector3(where.x - transform.position.x, 0, where.z - transform.position.z);
        float x = height / disXZ.magnitude;

        Vector3 velY = Vector3.up * Mathf.Sqrt(-2 * gravity * x);
        Vector3 velXZ = disXZ / (Mathf.Sqrt(-2*x/gravity) + Mathf.Sqrt(2*(disY-x)/gravity));
        return velXZ + velY;
    }
}
