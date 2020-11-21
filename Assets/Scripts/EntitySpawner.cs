using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] GameObject _creaturePrefab;
    // [SerializeField] GameObject itemPrefab;
    // [SerializeField] Transform itemParent;
    Queue<GameObject> _creatureQueue = new Queue<GameObject>();
    Manager manager;
    [SerializeField] Gradient gradient;

    void Awake(){
        manager = GetComponent<Manager>();
        SpawnQueueReserve();
    }

    void SpawnQueueReserve(){ //just spawning 10 bodies to have some reserve for the queue at the start
        for (int i = 0;i<(20);i++){
            GameObject newCreature = GameObject.Instantiate(_creaturePrefab,Vector3.down * 2,Quaternion.identity);
            _creatureQueue.Enqueue(newCreature);
            newCreature.SetActive(false);
        }
    }

    public void RecyleCreature(GameObject who){
        _creatureQueue.Enqueue(who);
        who.SetActive(false);
    }

    public void SpawnCreature(Vector3 where, string creatureName, bool enemy = false){
        GameObject newCreature = _creatureQueue.Dequeue();
        newCreature.name = creatureName;
        CreatureLogic logic = newCreature.GetComponent<CreatureLogic>();
        logic.SetCreature(where,enemy);
        newCreature.SetActive(true);
    }

    // void QueueEnemies(GameObject){
    //     for (int i = 0;i<10;i++){
    //         GameObject newEnemy = SpawnEnemy(Vector3.zero);//GameObject.Instantiate(enemyPrefab,pos,Quaternion.identity,enemyParent);
    //         allEnemies.Enqueue(newEnemy);
    //         newEnemy.SetActive(false);
    //     }
    // }

    // public GameObject SpawnEnemy(Vector3 pos){
    //     GameObject newEnemy = GameObject.Instantiate(enemyPrefab,pos,Quaternion.identity);
    //     return newEnemy;
    // }

    // public void KillEnemy(GameObject enemy){
    //     allEnemies.Enqueue(enemy);
    //     enemy.SetActive(false);
    // }

    // public void JudgeMe(GameObject judgeWho){
    //     judgeWho.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(judgeWho.GetComponent<Stats>().charm/100);
    // }

    // public void SpawnCharacter(Vector3 pos, string name){
    //     GameObject newCharacter = GameObject.Instantiate(characterPrefab,pos,Quaternion.identity);
    //     Stats stat = newCharacter.GetComponent<Stats>();
    //     CharacterStatGenerator(stat);
    //     newCharacter.name = name;
    //     newCharacter.layer = 8;
        
    //     newCharacter.GetComponent<CharacterMove>().partyTarget = manager.player.transform;
    //     stat.myName = name;
        
    //     newCharacter.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(stat.charm/100);
    //     manager.statManager.AddCharacter(newCharacter);
    // }

    // void CharacterStatGenerator(Stats stats){
    //     float totalStats = 100;
    //     float brains = Random.Range(0,70f);
    //     totalStats-=brains;
    //     float speed = Random.Range(10,totalStats);
    //     totalStats-=speed;

    //     stats.brains = brains;
    //     stats.speed = speed;
    //     stats.charm = totalStats;
    //     stats.age = 20;
    //     stats.health = 100;

    //     //add the charm stuff here if you want more complexity
    // }

    // public void SpawnItem(Vector3 pos){
    //     GameObject newItem = GameObject.Instantiate(itemPrefab,pos,Quaternion.identity);
    //     allItems.Enqueue(newItem);
    // }

    // public void CircleOfLife(GameObject addToQueue){
    //     lifepoolQueue.Enqueue(addToQueue);
    // }

}
