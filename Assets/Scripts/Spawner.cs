using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject _creaturePrefab;
    [SerializeField] GameObject _itemPrefab;
    Queue<GameObject> _creatureQueue = new Queue<GameObject>();
    Queue<GameObject> _itemQueue = new Queue<GameObject>();
    Manager manager;
    [SerializeField] Gradient gradient;

    void Awake(){
        manager = GetComponent<Manager>();
        SpawnQueueReserve();
    }

    void SpawnQueueReserve(){ //spawning for creature pool queue
        for (int i = 0;i<(20);i++){
            SpawnCreature();

            GameObject newItem = GameObject.Instantiate(_itemPrefab,Vector3.down * 2,Quaternion.identity);
            _itemQueue.Enqueue(newItem);
            newItem.SetActive(false);
        }
    }

    void SpawnCreature(){
        GameObject newCreature = GameObject.Instantiate(_creaturePrefab,Vector3.down * 2,Quaternion.identity);
        _creatureQueue.Enqueue(newCreature);
        newCreature.SetActive(false);
    }

    public void SpawnItem(Vector3 pos){
        GameObject newItem = _itemQueue.Dequeue();
        pos.y = 0;
        newItem.transform.position = pos;
        newItem.SetActive(true);
        _itemQueue.Enqueue(newItem);
    }

    public void RecycleCreature(GameObject who){
        _itemQueue.Enqueue(who);
        who.SetActive(false);
    }

    public void RecycleItem(GameObject who){
        _creatureQueue.Enqueue(who);
        who.SetActive(false);
    }

    public GameObject SpawnCreature(Vector3 where, string creatureName, bool enemy = false, bool child = false, bool returnIt = false){
        if (_creatureQueue.Count == 0){
            SpawnCreature();
        }
        GameObject newCreature = _creatureQueue.Dequeue();
        newCreature.name = creatureName;
        CreatureLogic logic = newCreature.GetComponent<CreatureLogic>();
        newCreature.SetActive(true);
        logic.SetCreature(where, enemy, child);
        if (!returnIt){
            return null;
        } else {
            return newCreature;
        }
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


    // public void CircleOfLife(GameObject addToQueue){
    //     lifepoolQueue.Enqueue(addToQueue);
    // }

}
