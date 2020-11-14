using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform _target;
    float moveSpeed;
    [SerializeField] float frequency;
    [SerializeField] float magnitude;
    Vector3 dir;

    void Update()
    {
        dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)){
            _target.position += Vector3.forward * Time.deltaTime * moveSpeed;
            dir.z = 1;
        }
        if (Input.GetKey(KeyCode.S)){
            _target.position += Vector3.back * Time.deltaTime * moveSpeed;
            dir.z = -1;
        }
        if (Input.GetKey(KeyCode.A)){
            _target.position += Vector3.left * Time.deltaTime * moveSpeed;
            dir.x = -1;
        }
        if (Input.GetKey(KeyCode.D)){
            _target.position += Vector3.right * Time.deltaTime * moveSpeed;
            dir.x = 1;
        }
        if (dir != Vector3.zero){
            _target.position = _target.position + transform.up * Mathf.Sin(Time.time * frequency * moveSpeed) * magnitude;
        }
    }

    public void SetNewTarget(Transform target){
        _target = target;
        Stats newStats = target.GetComponent<Stats>();
        moveSpeed = newStats.speed/10;
    }

}
