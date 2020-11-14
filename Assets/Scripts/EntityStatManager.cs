using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> allCharacterObjects = new List<GameObject>();
    [HideInInspector] public List<Stats> allCharactersStats = new List<Stats>();
    [HideInInspector] public List<GameObject> partyCharacterObjects = new List<GameObject>();
    [HideInInspector] public List<Stats> partyCharacterStats = new List<Stats>();
    [SerializeField] float ageSpeed;
    [SerializeField] float deathAge;
    
    Manager manager;

    void Start(){
        manager = GetComponent<Manager>();
    }
    void Update()
    {
        for (int i = 0;i<allCharactersStats.Count;i++){
            allCharactersStats[i].age+=(ageSpeed * Time.deltaTime);
            if(allCharactersStats[i].age>deathAge){
                manager.CharacterDeath(allCharacterObjects[i]);
            }
        }
        TallyPartyStats();
        
    }

    public void AddCharacter(GameObject newChar){
        allCharacterObjects.Add(newChar);
        allCharactersStats.Add(newChar.GetComponent<Stats>());

    }

    public void AddPartyCharacter(GameObject newPartyMember){
        partyCharacterObjects.Add(newPartyMember);
        partyCharacterStats.Add(newPartyMember.GetComponent<Stats>());
    }

    void TallyPartyStats(){
        float age = 0;
        float strength = 0;
        for (int i = 0;i<partyCharacterStats.Count;i++){
            age+=partyCharacterStats[i].age;
            strength+=partyCharacterStats[i].strength;
        }
        manager.partyAge = age/partyCharacterStats.Count;
        manager.partyStrength = strength/partyCharacterStats.Count;

    }



    // void UpdateAgeApperance(){

    // }
}
