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

    public void Congratulations(GameObject faja, GameObject maja){
        Stats fajaStats = faja.GetComponent<Stats>();
        Stats majaStats = maja.GetComponent<Stats>();

        GameObject baby = GameObject.Instantiate(manager.spawner.characterPrefab, faja.transform.position,Quaternion.identity);
        baby.layer = 9;
        Stats babyStats = baby.GetComponent<Stats>();
        babyStats = BabyStats(fajaStats,majaStats);
        manager.move.SetNewTarget(baby.transform);
    }

    Stats BabyStats(Stats faja, Stats maja)
    {
        Stats babyStats = new Stats();
        babyStats.speed = ((faja.speed + maja.speed) / 2 );
        babyStats.brains = ((faja.brains + maja.brains) / 2 );

        return babyStats;
    }
}
