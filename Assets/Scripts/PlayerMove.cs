using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMove : MonoBehaviour
{
    public Transform player;
    [SerializeField] float moveSpeed, frequency, magnitude;
    Vector3 dir;
    Vector3 targetPos = new Vector3(0,1.6f,0);
    [SerializeField] LayerMask clickableLM;
    [SerializeField] bool keyboard, mouse;
    [HideInInspector] public bool cutscene;
    public bool gameLive;
    Manager manager;
    Vector3 myBounds;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    Rigidbody rb;
    public Vector3 minLimit, maxLimit;
    [SerializeField] Transform visibleMesh;
    [SerializeField]List<Vector3> aroundMe = new List<Vector3>();
    public Queue<Vector3> offsetPositions = new Queue<Vector3>();

    void Awake(){
        manager = GetComponent<Manager>();

        for (int i = 0;i<aroundMe.Count;i++){
            offsetPositions.Enqueue(aroundMe[i] * 2);
        }
        // Debug.Log(_target.GetComponent<BoxCollider>().bounds.max + "max");
        // Debug.Log(_target.GetComponent<BoxCollider>().bounds.min + "min");
        // Debug.Log(_target.GetComponent<BoxCollider>().bounds.extents + "extents");
        //myBounds = player.GetComponent<BoxCollider>().bounds.size;
    }

    public void StopControl(bool isLive = false){
        gameLive = isLive;
        rb.velocity = Vector3.zero;
    }
    
    void Update()
    {
        if (gameLive){
            if (keyboard){
                dir = Vector3.zero;
                if (Input.GetKey(KeyCode.W)){
                    dir += Vector3.forward;
                }
                if (Input.GetKey(KeyCode.S)){
                    dir += Vector3.back;
                }
                if (Input.GetKey(KeyCode.A)){
                    dir += Vector3.left;
                }
                if (Input.GetKey(KeyCode.D)){
                    dir += Vector3.right;   
                }
                if (dir == Vector3.zero){
                    rb.velocity = dir;
                } else {
                    MovePlayer();
                }
            }
            if (mouse){
                if (Input.GetMouseButtonDown(0)){
                    MoveMouse();
                }
                if (player.position != targetPos){
                    player.position = Vector3.MoveTowards(player.position, targetPos, moveSpeed * Time.deltaTime);
                    player.position = player.position + transform.up * Mathf.Sin(Time.time * frequency * moveSpeed) * magnitude;
                }
            }
        }
        if (cutscene){
            rb.isKinematic = true;
            player.position = player.position + transform.up * Mathf.Sin(Time.time * 5 * 3) * 0.015f;
            player.position += Vector3.forward * 13.3f * Time.deltaTime;
        }
    }

    void MovePlayer(){
        rb.velocity = (dir * moveSpeed);
        //rb.AddForce(dir * moveSpeed);
        // player.position += dir * Time.deltaTime * moveSpeed;
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
                //hit.collider.gameObject.GetComponent<CharacterMove>().moving = false;
            } else if (hit.collider.gameObject.layer == 9){ // a party member, freeze game and show their stats

            }
            targetPos.y = 1.1f;
            // if (manager.wooing){
            //     manager.WooFail();
            // }
        }
    }

    public void SetNewTarget(Transform target){
        player = target;
        rb = player.GetComponent<Rigidbody>();
        CreatureLogic playerStats = target.GetComponent<CreatureLogic>();
        moveSpeed = playerStats.Speed;
        virtualCamera.m_Follow = target;
        //virtualCamera.m_Lens.OrthographicSize = newStats.brains * .8f;
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 zoom = new Vector3(0, 20, -15);
        //Vector3 zoom = new Vector3(0, newStats.brains * .8f, -newStats.brains * .5f);
        transposer.m_FollowOffset = zoom;
        target.position = new Vector3(target.position.x,1.1f,target.position.z);
    }

    public void OpeningCutsceneStart(){
        cutscene = true;
    }

    public void OpeningCutsceneGiveControl(){
        SetNewTarget(player);
        virtualCamera.m_Priority = 11;
        cutscene = false;
        gameLive = true;
        rb.isKinematic = false;
    }

}
