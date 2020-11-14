using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLogic : MonoBehaviour
{
    float timeLimit, counter, mySpeed, myBrains;
    float minWaitTime = 0.1f;
    float maxWaitTime = 0.3f;
    float minMoveTime = 2;
    float maxMoveTime = 10;
    Vector3 myDir;
    public bool moving, wooAble, partyMember, retired, isPlayer;
    Transform partyTarget;

    void Start(){
        Stats myStats = GetComponent<Stats>();
        mySpeed = myStats.speed/10;
        myBrains = myStats.brains/100;
        StartCoroutine(NewDirection());
        wooAble = true;

    }

    IEnumerator NewDirection(bool wait = true){
        moving = false;
        myDir = new Vector3(Random.Range(-1,1f),0,Random.Range(myBrains,1f));//bias toward moving UP on the Z toward the finish line
        transform.position = new Vector3(transform.position.x, 1.6f, transform.position.z); //to prevent them from bobbing above/below base level
        counter = 0;
        timeLimit = Random.Range(minMoveTime,maxMoveTime);
        if (wait){
            yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
        } else {
            yield return null;
            StartCoroutine(ReadyToTryAgain());
        }
        moving = true;
    }

    IEnumerator ReadyToTryAgain(){
        yield return new WaitForSeconds(3);
        wooAble = true;
    }
    void Update()
    {
        if (!isPlayer){
            if (retired){
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + transform.forward * Mathf.Sin(Time.time * 3 * mySpeed));
            // } else if (partyMember && transform.position != myDir){
            //     myDir = partyTarget.position - new Vector3(1,0,1);
            //     transform.position = Vector3.MoveTowards(transform.position, myDir, mySpeed * Time.deltaTime);
            //     transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
            } else {
                counter+=Time.deltaTime;
                if (counter>timeLimit){
                    StartCoroutine(NewDirection());
                }
                if (moving){
                    transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
                    transform.position += myDir * mySpeed * Time.deltaTime;
                }
            }
        }
    }

    void OnTriggerEnter(Collider col){
        if (!isPlayer & !retired && wooAble){
            if (col.gameObject.layer == 8){
                wooAble = false;
                FindObjectOfType<Manager>().WooCPU(this.gameObject,col.gameObject);
            } else if (col.gameObject.layer == 9){
                wooAble = false;
                FindObjectOfType<Manager>().BeginWoo(this.gameObject);
            } else if (col.gameObject.layer == 9 && !wooAble){
                DontTalkToMe();
            }
        }

        // if (gameObject.layer == 8 && !FindObjectOfType<Manager>().wooing){
        //     if (col.gameObject.layer == 9 && wooAble){
        //         StopAllCoroutines();
        //         StartCoroutine(WaitForResponse());
        //         moving = false;
        //         FindObjectOfType<Manager>().BeginWoo(this.gameObject);
        //     } else if (!wooAble){
        //         DontTalkToMe();
        //     }
        // }
    }

    public void WooFail(){
        StopAllCoroutines();
        StartCoroutine(NewDirection(false));
    }

    void DontTalkToMe(){
        Debug.Log("don't talk to me!");
    }

    IEnumerator WaitForResponse(){ //player has 5 seconds to successfully woo this character
        yield return new WaitForSeconds(5);
        FindObjectOfType<Manager>().WooFail();
    }

    // public void JoinParty(){
    //     partyTarget = FindObjectOfType<PlayerMove>().selectedPartyMember;
    //     gameObject.layer = 9;
    //     partyMember = true;
    // }
}
