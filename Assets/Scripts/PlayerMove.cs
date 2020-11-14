using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMove : MonoBehaviour
{
    [HideInInspector] public Transform selectedPartyMember;
    float moveSpeed;
    [SerializeField] float frequency;
    [SerializeField] float magnitude;
    Vector3 dir;
    Vector3 targetPos = new Vector3(0,1.1f,0);
    [SerializeField] LayerMask clickableLM;
    [SerializeField] bool keyboard, mouse;
    Manager manager;
    Vector3 myBounds;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    

    void Start(){
        manager = GetComponent<Manager>();
        // Debug.Log(_target.GetComponent<BoxCollider>().bounds.max + "max");
        // Debug.Log(_target.GetComponent<BoxCollider>().bounds.min + "min");
        // Debug.Log(_target.GetComponent<BoxCollider>().bounds.extents + "extents");
        myBounds = selectedPartyMember.GetComponent<BoxCollider>().bounds.size;
    }
    
    void Update()
    {
        if (keyboard){
            dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)){
                selectedPartyMember.position += Vector3.forward * Time.deltaTime * moveSpeed;
                dir.z = 1;
            }
            if (Input.GetKey(KeyCode.S)){
                selectedPartyMember.position += Vector3.back * Time.deltaTime * moveSpeed;
                dir.z = -1;
            }
            if (Input.GetKey(KeyCode.A)){
                selectedPartyMember.position += Vector3.left * Time.deltaTime * moveSpeed;
                dir.x = -1;
            }
            if (Input.GetKey(KeyCode.D)){
                selectedPartyMember.position += Vector3.right * Time.deltaTime * moveSpeed;
                dir.x = 1;
            }
            if (dir != Vector3.zero){
                selectedPartyMember.position = selectedPartyMember.position + transform.up * Mathf.Sin(Time.time * frequency * moveSpeed) * magnitude;
            }
        }
        if (mouse){
            if (Input.GetMouseButtonDown(0) && !manager.wooing){
                MoveMouse();
            }
            if (selectedPartyMember.position != targetPos){
                selectedPartyMember.position = Vector3.MoveTowards(selectedPartyMember.position, targetPos, moveSpeed * Time.deltaTime);
                selectedPartyMember.position = selectedPartyMember.position + transform.up * Mathf.Sin(Time.time * frequency * moveSpeed) * magnitude;
            }
        }
    }

    void MoveMouse(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit, 100)){
            if (hit.collider.gameObject.layer == 4){ // if hit ground then move there
                targetPos = hit.point;
            } else if (hit.collider.gameObject.layer == 8){ // another character, move there and start talking
                targetPos = hit.transform.position - new Vector3(1,0,1);
                Debug.Log(hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<CharacterMove>().moving = false;
            } else if (hit.collider.gameObject.layer == 9){ // a party member, freeze game and show their stats

            }
            targetPos.y = 1.1f;
            // if (manager.wooing){
            //     manager.WooFail();
            // }
        }
    }

    public void SetNewTarget(Transform target){
        selectedPartyMember = target;
        Stats newStats = target.GetComponent<Stats>();
        moveSpeed = newStats.speed/10;
        virtualCamera.m_Follow = target;
    }

}
