// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class HumanLogic : MonoBehaviour
// {
//     float timeLimit, counter, mySpeed, myBrains, myCharm;
//     float minWaitTime = 0.1f;
//     float maxWaitTime = 0.3f;
//     float minMoveTime = 2;
//     float maxMoveTime = 10;
//     public Vector3 myDir,myTarget;
//     public bool moving, wooAble, isPlayer, dontMove;
//     [SerializeField] LayerMask attractionLM;

//     void Start(){
//         Stats myStats = GetComponent<Stats>();
//         mySpeed = myStats.speed/10;
//         myBrains = myStats.brains/100;
//         myCharm = myStats.charm;
//         StartCoroutine(NewDirection());
//         wooAble = true;

//     }
//     void Update()
//     {
//         if (!isPlayer && !dontMove){
//             if (gameObject.layer == 10){
//                 RetirementDance();
//             } else {
//                 if (myTarget != Vector3.zero) {
//                     MoveToTarget();
//                 } else {
//                     MoveInDir();
//                 }
//                 counter+=Time.deltaTime;
//                 if (counter>timeLimit){
//                     StartCoroutine(NewDirection());
//                 }
//             }
                
//         }
//     }

//     void MoveToTarget(){
//         if (Vector3.Distance(myTarget,Vector3.zero) < 0.5f){
//             myTarget = Vector3.zero;
//             StartCoroutine(NewDirection());
//         } else {
//             transform.position = Vector3.MoveTowards(transform.position, myTarget, mySpeed * Time.deltaTime);
//             transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
//         }
//     }

//     void MoveInDir(){
//         if (moving){
//             transform.position = transform.position + transform.up * Mathf.Sin(Time.time * 3 * mySpeed) * 0.015f;
//             transform.position += myDir * mySpeed * Time.deltaTime;
//         }
//     }

//     IEnumerator NewDirection(bool wait = true){
//         moving = false;
//         //CheckSurroundings();
//         if (myTarget == Vector3.zero){
//             myDir = new Vector3(Random.Range(-1,1f),0,Random.Range(myBrains,1f));//bias toward moving UP on the Z toward the finish line
//         }
//         transform.position = new Vector3(transform.position.x, 1.6f, transform.position.z); //to prevent them from bobbing above/below base level
//         counter = 0;
//         timeLimit = Random.Range(minMoveTime,maxMoveTime);
//         if (wait){
//             yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
//         } else {
//             yield return null;
//             StartCoroutine(ReadyToWoo());
//         }
//         moving = true;
//     }

//     IEnumerator ReadyToWoo(){
//         yield return new WaitForSeconds(1);
//         wooAble = true;
//         myTarget = Vector3.zero;
//     }

//     void RetirementDance(){
//         transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + transform.forward * Mathf.Sin(Time.time * 3 * mySpeed));
//     }

//     // void CheckSurroundings(){
//     //     Collider[] cols = Physics.OverlapSphere(transform.position, myBrains*50,attractionLM);
//     //     for (int i = 0;i<cols.Length;i++){
//     //         if (cols[i].gameObject.layer == 11){
//     //             myTarget = cols[i].transform.position;
//     //         } else if (cols[i].gameObject.layer == 8 && cols[i].GetComponent<Stats>().charm > myCharm && cols[i].GetComponent<HumanLogic>().wooAble){
//     //             myTarget = cols[i].transform.position;
//     //         }
//     //     }
//     // }

//     void OnTriggerEnter(Collider col){
//         if (!isPlayer && gameObject.layer!=10 && wooAble){
//             if (col.gameObject.layer == 8){
//                 wooAble = false;
//                 FindObjectOfType<Manager>().WooCPU(this.gameObject,col.gameObject);
//             } else if (col.gameObject.layer == 9){
//                 wooAble = false;
//                 dontMove = true;
//                 FindObjectOfType<Manager>().BeginWoo(this.gameObject);
//             } else if (col.gameObject.layer == 9 && !wooAble){
//                 DontTalkToMe();
//             }
//         }
//         if (col.gameObject.layer == 11){
//             col.gameObject.transform.position = new Vector3(transform.position.x + Random.Range(-100,100), 1, transform.position.z + 100);
//             GetComponent<Stats>().charm += 3;
//             Manager manager = FindObjectOfType<Manager>();
//             manager.spawner.JudgeMe(this.gameObject);
//             manager.particles.PlayItem(transform.position);
//             myTarget = Vector3.zero;
//         }

//         // if (gameObject.layer == 8 && !FindObjectOfType<Manager>().wooing){
//         //     if (col.gameObject.layer == 9 && wooAble){
//         //         StopAllCoroutines();
//         //         StartCoroutine(WaitForResponse());
//         //         moving = false;
//         //         FindObjectOfType<Manager>().BeginWoo(this.gameObject);
//         //     } else if (!wooAble){
//         //         DontTalkToMe();
//         //     }
//         // }
//     }

//     public void WooFail(){
//         dontMove = false;
//         StopAllCoroutines();
//         StartCoroutine(NewDirection(false));
//     }

//     void DontTalkToMe(){
//         Debug.Log("don't talk to me!");
//     }

//     IEnumerator WaitForResponse(){ //player has 5 seconds to successfully woo this character
//         yield return new WaitForSeconds(5);
//         FindObjectOfType<Manager>().WooFail();
//     }

//     // public void JoinParty(){
//     //     partyTarget = FindObjectOfType<PlayerMove>().selectedPartyMember;
//     //     gameObject.layer = 9;
//     //     partyMember = true;
//     // }
// }
