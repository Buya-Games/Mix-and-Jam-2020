using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    float timeLimit, counter, mySpeed;
    float minWaitTime = 3;
    float maxWaitTime = 6;
    float minMoveTime = 2;
    float maxMoveTime = 10;
    Vector3 myDir;
    public bool moving, wooAble, partyMember;
    Transform partyTarget;

    void Start(){
        mySpeed = GetComponent<Stats>().speed/10;
        StartCoroutine(NewDirection());
        wooAble = true;
    }

    IEnumerator NewDirection(bool wait = true){
        moving = false;
        myDir = new Vector3(Random.Range(-1,1f),0,Random.Range(-1,1f));
        transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z); //to prevent them from bobbing above/below base level
        counter = 0;
        timeLimit = Random.Range(minMoveTime,maxMoveTime);
        if (wait){
            yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
        } else {
            yield return null;
        }
        moving = true;
    }
    void Update()
    {
        if (partyMember && transform.position != myDir){
            myDir = partyTarget.position - new Vector3(1,0,1);
            transform.position = Vector3.MoveTowards(transform.position, myDir, mySpeed * Time.deltaTime);
            transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
        } else {
            counter+=Time.deltaTime;
            if (moving){
            transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
            transform.position += myDir * mySpeed * Time.deltaTime;
            }
            if (counter>timeLimit){
                StartCoroutine(NewDirection());
            }
        }
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

    IEnumerator WaitForResponse(){ //player has 10 seconds to successfully woo this character
        yield return new WaitForSeconds(10);
        FindObjectOfType<Manager>().WooFail();
    }

    public void JoinParty(){
        partyTarget = FindObjectOfType<PlayerMove>().selectedPartyMember;
        gameObject.layer = 9;
        partyMember = true;
    }
}
