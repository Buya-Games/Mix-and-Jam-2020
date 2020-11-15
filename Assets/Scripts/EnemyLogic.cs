using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    Manager manager;
    [HideInInspector] public Transform myTarget;
    Vector3 patrolTarget;
    float mySpeed, myStrength;
    [HideInInspector] public bool patrol, chase, attack;
    float attackTimer, chaseTimer, patrolTimer, counter;
    Stats myStats;
    EnemyFOV myFOV;
    // Start is called before the first frame update
    void Awake()
    {
        manager = FindObjectOfType<Manager>();
        myStats = GetComponent<Stats>();
        myFOV = GetComponentInChildren<EnemyFOV>();
    }

    public void PlaceEnemy(Vector3 where){
        transform.position = where;

        SetStats(manager.difficulty);
        patrol = true;
        myFOV.patrol = true;
        patrolTarget = new Vector3(Random.Range(-1,1f),0,Random.Range(-1f,1f));
    }

    public void OpeningSequence(Transform tgt){
        SetStats(1);
        chaseTimer = 100;
        myTarget = tgt;
        chase = true;
        
    }

    public void SetStats(float difficulty){
        float totalStats = 100 * difficulty;
        myStats.brains = Random.Range(0,70);
        totalStats-=myStats.brains;
        myStats.speed = Random.Range(10,totalStats);
        totalStats-=myStats.speed;
        myStats.strength = Random.Range(10,totalStats);

        mySpeed = myStats.speed/10;
        myStrength = myStats.strength/10;

        patrolTimer = (100 - myStats.speed)/5;
        attackTimer = (100 - myStats.speed)/10;
        chaseTimer = (myStats.brains)/8;
        float mySize = Mathf.Clamp(myStrength/2,0.1f,10);
        transform.localScale = new Vector3(mySize,mySize*2,mySize);
        myStats.health = 100 * difficulty * mySize;
        myFOV.SetFOV(myStats.brains);
    }

    // Update is called once per frame
    void Update()
    {
        if (patrol){
            counter+=Time.deltaTime;
            Patrol();
        }
        if (chase){
            counter+=Time.deltaTime;
            ChaseTarget();
        }
        if (attack){
            AttackTarget();
        }
        if (myStats.health <= 0){
            Death();
        }
    }

    void Death(){
        Debug.Log(name + " died");
    }

    void Patrol(){
        if (counter > patrolTimer){
            counter = 0;
            patrolTarget = new Vector3(Random.Range(-1,1f),0,Random.Range(-1f,1f));
        }
        transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
        transform.position += patrolTarget * mySpeed * Time.deltaTime;
    }
    
    void ChaseTarget(){
        float dist = Mathf.Abs(Vector3.Distance(transform.position, myTarget.position));
        if (dist < 2){
            ChangeState(false,false,true);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, myTarget.position, mySpeed * Time.deltaTime);
            transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
        }
        if (counter > chaseTimer){
            ChangeState(true,false,false);
        }
    }

    void AttackTarget(){
        if (Mathf.Abs(Vector3.Distance(transform.position, myTarget.position)) > 3){
            ChangeState(false,true,false);
        } else {
            counter+=Time.deltaTime;
        }
        if (counter > attackTimer){
            counter = 0;
            Attack();
        }
    }

    void Attack(){
        float rando = Random.Range(.8f,1.2f);
        bool critical = (rando >= 1.1f);
        float hitPoint = myStrength * rando;
        myTarget.GetComponent<Stats>().health -= hitPoint;
        manager.ui.HitGUI(myTarget.position, hitPoint, critical);
    }

    public void ChangeState(bool p, bool c, bool a){
        if (p){
            patrol = true;
            myFOV.patrol = true;
            chase = false;
            attack = false;
            myFOV.LowerAlert();
            //Debug.Log(name + ": p");
        }
        if (c){
            patrol = false;
            myFOV.patrol = false;
            chase = true;
            attack = false;
            //Debug.Log(name + ": c");
        }
        if (a){
            patrol = false;
            myFOV.patrol = false;
            chase = false;
            attack = true;
            //Debug.Log(name + ": a");
        }
        counter = 0;
    }
}
