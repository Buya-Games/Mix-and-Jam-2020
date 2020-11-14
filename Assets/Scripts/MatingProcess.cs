using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatingProcess : MonoBehaviour
{
    Manager manager;
    void Start()
    {
        manager = FindObjectOfType<Manager>();
    }

    public void Congratulations(GameObject faja, GameObject maja, bool cpu){
        Stats fajaStats = faja.GetComponent<Stats>();
        Stats majaStats = maja.GetComponent<Stats>();

        GameObject baby = GameObject.Instantiate(manager.spawner.characterPrefab, faja.transform.position + Vector3.back,Quaternion.identity);
        baby.name = "computer baaaby " + cpu;
        if (cpu){
            baby.layer = 8;
        } else {
            baby.layer = 9;
        }
        Stats babyStats = baby.GetComponent<Stats>();
        BabyStats(fajaStats,majaStats, babyStats);
        manager.NewBaby(baby, cpu);
    }

    void BabyStats(Stats faja, Stats maja, Stats babyStats){
        babyStats.speed = ((faja.speed + maja.speed) / 2 );
        babyStats.brains = ((faja.brains + maja.brains) / 2 );
        babyStats.charm = ((faja.charm + maja.charm) / 2 );
        //Debug.Log("new baby! speed: " + babyStats.speed + ", brains: " + babyStats.brains + ", charm: " + babyStats.charm);
    }
}
