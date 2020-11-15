using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCutscene : MonoBehaviour
{
    [SerializeField] bool openingFight;
    float health = 2;
    [SerializeField] GameObject player;

    void OnTriggerEnter(Collider col){
        if (col.gameObject == player){
            Debug.Log("touch");
            if (health < 0){
                FindObjectOfType<CutsceneDirector>().EndFight();
            } else{
                health--;
                FindObjectOfType<UIManager>().HitGUI(transform.position, Random.Range(1,5));
            }
        }

    }
    
}
