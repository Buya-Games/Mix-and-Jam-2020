// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemyFOV : MonoBehaviour
// {
//     EnemyLogic myLogic;
//     public bool patrol;
//     MeshRenderer meshRenderer;
//     [SerializeField] Color alert, normal;

//     void Start(){
//         myLogic = transform.root.GetComponent<EnemyLogic>();
//         meshRenderer = GetComponent<MeshRenderer>();
//     }

//     public void SetFOV(float myBrains){
//         float fovSize = Mathf.Clamp(myBrains/2,10,40);
//         transform.localScale = new Vector3(fovSize,.1f,fovSize);
//     }

//     void OnTriggerStay(Collider col){
//         if (patrol)
//             if (col.gameObject.layer == 9){
//                     RaiseAlert(col.gameObject.transform);
//                 } else if (col.gameObject.layer == 13 && !col.gameObject.GetComponent<EnemyFOV>().patrol){
//                     RaiseAlert(col.transform.root.GetComponent<EnemyLogic>().myTarget);
//                 }
//     }

//     public void RaiseAlert(Transform target){
//         meshRenderer.material.color = alert;
//         myLogic.myTarget = target;
//         myLogic.ChangeState(false,true,false);
//     }

//     public void LowerAlert(){
//         patrol = true;
//         meshRenderer.material.color = normal;
//     }
// }
