using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MatingProcess : MonoBehaviour
{
    Manager manager;
    [SerializeField] Transform babyParent;
    void Start()
    {
        manager = FindObjectOfType<Manager>();
    }

    public void Congratulations(GameObject faja, GameObject maja, Vector3 birthplace, bool cpu){
        Stats fajaStats = faja.GetComponent<Stats>();
        Stats majaStats = maja.GetComponent<Stats>();

        GameObject baby;
        if (manager.spawner.lifepoolQueue.Count > 0){
            baby = manager.spawner.lifepoolQueue.Dequeue();
            baby.transform.position = birthplace;
            baby.transform.rotation = Quaternion.identity;
            baby.SetActive(true);
        } else {
            baby = GameObject.Instantiate(manager.spawner.characterPrefab, birthplace,Quaternion.identity, babyParent);
        }
        Vector3 origScale = baby.transform.localScale;
        baby.transform.localScale = Vector3.zero;
        baby.transform.DOScale(origScale,1f);
        manager.particles.PlayBaby(birthplace);
        if (cpu){
            baby.name = faja.name + "+" + maja.name;
            baby.layer = 8;
        } else {
            baby.transform.parent = null;
            baby.name = "new baby";
            baby.layer = 9;
        }
        Stats babyStats = baby.GetComponent<Stats>();
        BabyStats(fajaStats,majaStats, babyStats);
        manager.NewBabyCharacter(baby);
        //manager.NewBaby(baby, cpu);
    }

    void BabyStats(Stats faja, Stats maja, Stats babyStats){
        babyStats.speed = Mathf.Max(faja.speed, maja.speed) + Random.Range(-5,5);
        babyStats.brains = Mathf.Max(faja.brains, maja.brains) + Random.Range(-5,5);
        babyStats.charm = Mathf.Max(faja.charm, maja.charm) + Random.Range(-5,5);
        // babyStats.speed = ((faja.speed + maja.speed) / 2 + Random.Range(-5,5));
        // babyStats.brains = ((faja.brains + maja.brains) / 2  + Random.Range(-5,5));
        // babyStats.charm = ((faja.charm + maja.charm) / 2  + Random.Range(-5,5));
        babyStats.age = 20;
        //Debug.Log("new baby! speed: " + babyStats.speed + ", brains: " + babyStats.brains + ", charm: " + babyStats.charm);
    }
}
