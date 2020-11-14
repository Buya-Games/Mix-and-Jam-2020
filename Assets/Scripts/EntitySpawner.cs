using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] public GameObject characterPrefab;
    [SerializeField] Transform characterParent;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemyParent;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform itemParent;
    public Queue<GameObject> lifepoolQueue = new Queue<GameObject>();
    List<GameObject> allEnemies = new List<GameObject>();
    Queue<GameObject> allItems = new Queue<GameObject>();
    Manager manager;
    [SerializeField] Gradient gradient;

    void Awake(){
        manager = GetComponent<Manager>();
        SpawnQueueReserve();
    }

    void SpawnQueueReserve(){ //just spawning 10 bodies to have some reserve for the queue at the start
        for (int i = 0;i<10;i++){
            GameObject newCharacter = GameObject.Instantiate(characterPrefab,Vector3.down * 2,Quaternion.identity,characterParent);
            lifepoolQueue.Enqueue(newCharacter);
            newCharacter.SetActive(false);
        }
    }

    public void JudgeMe(GameObject judgeWho){
        judgeWho.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(judgeWho.GetComponent<Stats>().charm/100);
    }

    public void SpawnCharacter(Vector3 pos, string name){
        GameObject newCharacter = GameObject.Instantiate(characterPrefab,pos,Quaternion.identity,characterParent);
        newCharacter.name = name;
        newCharacter.layer = 8;
        Stats stat = newCharacter.GetComponent<Stats>();
        stat.myName = name;
        CharacterStatGenerator(stat);
        newCharacter.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(stat.charm/100);
        manager.statManager.AddCharacter(newCharacter);
    }

    void CharacterStatGenerator(Stats stats){
        float totalStats = 100;
        float brains = Random.Range(0,70f);
        totalStats-=brains;
        float speed = Random.Range(10,totalStats);
        totalStats-=speed;

        stats.brains = brains;
        stats.speed = speed;
        stats.charm = totalStats;
        stats.age = 20;

        //add the charm stuff here if you want more complexity
    }

    public void SpawnEnemy(Vector3 pos){
        GameObject newEnemy = GameObject.Instantiate(enemyPrefab,pos,Quaternion.identity,enemyParent);
        allEnemies.Add(newEnemy);
    }

    public void SpawnItem(Vector3 pos){
        GameObject newItem = GameObject.Instantiate(itemPrefab,pos,Quaternion.identity,itemParent);
        allItems.Enqueue(newItem);
    }

    public void CircleOfLife(GameObject addToQueue){
        lifepoolQueue.Enqueue(addToQueue);
    }

}
