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
    List<GameObject> allCharacters = new List<GameObject>();
    List<GameObject> allEnemies = new List<GameObject>();
    List<GameObject> allItems = new List<GameObject>();
    Manager manager;
    [SerializeField] Gradient gradient;

    void Awake(){
        manager = GetComponent<Manager>();
    }

    public void SpawnCharacter(Vector3 pos, string name){
        GameObject newCharacter = GameObject.Instantiate(characterPrefab,pos,Quaternion.identity,characterParent);
        newCharacter.name = name;
        newCharacter.layer = 8;
        Stats stat = newCharacter.GetComponent<Stats>();
        stat.myName = name;
        CharacterStatGenerator(stat);
        newCharacter.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(stat.strength/100);
        Debug.Log(stat.strength/100);

        newCharacter.AddComponent<CharacterMove>();
        manager.statManager.AddCharacter(newCharacter);
    }

    void CharacterStatGenerator(Stats stats){
        stats.age = Random.Range(18f,25f);
        stats.strength = Random.Range(30f,100f);
        stats.speed = Random.Range(20f,100f);
    }

    public void SpawnEnemy(Vector3 pos){
        GameObject newEnemy = GameObject.Instantiate(enemyPrefab,pos,Quaternion.identity,enemyParent);
        allEnemies.Add(newEnemy);
    }

    public void SpawnItem(Vector3 pos){
        GameObject newItem = GameObject.Instantiate(itemPrefab,pos,Quaternion.identity,itemParent);
        allItems.Add(newItem);
    }

}
