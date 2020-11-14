using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    float timeLimit, counter, mySpeed, myDistance;
    float minWaitTime = 1;
    float maxWaitTime = 3;
    float minMoveTime = 2;
    float maxMoveTime = 10;
    Vector3 myDir, myOffset;
    public bool moving, wooAble, partyMember, isPlayer;
    Transform partyTarget;

    void Start(){
        mySpeed = GetComponent<Stats>().speed/10;
        StartCoroutine(NewDirection());
        wooAble = true;
        
    }

    IEnumerator NewDirection(bool wait = true){
        counter = 0;
        timeLimit = Random.Range(minMoveTime,maxMoveTime);
        moving = false;
        myDir = new Vector3(Random.Range(-1,1f),0,Random.Range(-0.1f,1f));//bias toward moving UP on the Z toward the finish line
        transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z); //to prevent them from bobbing above/below base level
        if (wait){
            yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
        } else {
            yield return null;
        }
        moving = true;
    }
    void Update()
    {
        if (!isPlayer){
            counter+=Time.deltaTime;
            if (partyMember){
                if (counter>timeLimit){
                    NewPartyPosition();
                }
                myDir = partyTarget.position + myOffset;
                transform.position = Vector3.MoveTowards(transform.position, myDir, mySpeed * Time.deltaTime);
                transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
            } else {
                if (moving){
                    transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
                    transform.position += myDir * mySpeed * Time.deltaTime;
                }
                if (counter>timeLimit){
                    StartCoroutine(NewDirection());
                }
            }
        }
    }

    void NewPartyPosition(){
        counter = 0;
        timeLimit = Random.Range(minMoveTime,maxMoveTime);
        myDistance = Random.Range(2,5f);
        myOffset = new Vector3(myDistance * (Random.Range(0,2)-1),0,myDistance * (Random.Range(0,2)-1));
        
    }

    void OnTriggerEnter(Collider col){
        if (gameObject.layer == 8 && !FindObjectOfType<Manager>().wooing){
            if (col.gameObject.layer == 9 && wooAble){
                StopAllCoroutines();
                StartCoroutine(WaitForResponse());
                moving = false;
                FindObjectOfType<Manager>().BeginWoo(this.gameObject);
            } else if (!wooAble){
                DontTalkToMe();
            }
        }
    }

    public void WooFail(){
        StopAllCoroutines();
        wooAble = false;
        StartCoroutine(NewDirection(false));
    }

    void DontTalkToMe(){
        Debug.Log("don't talk to me!");
    }

    IEnumerator WaitForResponse(){ //player has 5 seconds to successfully woo this character
        yield return new WaitForSeconds(5);
        FindObjectOfType<Manager>().WooFail();
    }

    public void JoinParty(){
        NewPartyPosition();
        partyTarget = FindObjectOfType<PlayerMove>().player;
        gameObject.layer = 9;
        partyMember = true;
        moving = false;
        wooAble = false;
    }
}
