// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EntityStatManager : MonoBehaviour
// {
//     [HideInInspector] public List<GameObject> allCharacterObjects = new List<GameObject>();
//     [HideInInspector] public List<Stats> allCharactersStats = new List<Stats>();
//     [HideInInspector] public List<GameObject> partyCharacterObjects = new List<GameObject>();
//     [HideInInspector] public List<Stats> partyCharacterStats = new List<Stats>();
//     [SerializeField] float ageSpeed;
//     [SerializeField] float deathAge;
//     [HideInInspector] public float globalSpeed, globalBrains, globalCharm, globalSpeedMax, globalBrainsMax, globalCharmMax;
//     [HideInInspector] public int globalPopulation;
    
//     Manager manager;

//     void Start(){
//         manager = GetComponent<Manager>();
//     }
//     void Update()
//     {
//         for (int i = 0;i<allCharactersStats.Count;i++){
//             allCharactersStats[i].age+=(ageSpeed * Time.deltaTime);
//             // if(allCharactersStats[i].age>deathAge){
//             //     manager.CharacterDeath(allCharacterObjects[i]);
//             // }
//         }
//         //TallyPartyStats();
        
//     }

//     public void AddCharacter(GameObject newChar){
//         allCharacterObjects.Add(newChar);
//         allCharactersStats.Add(newChar.GetComponent<Stats>());
//         TallyGlobalStats();
//     }

//     public void RemoveFromGlobalPool(GameObject removeObj){
//         allCharacterObjects.Remove(removeObj);
//         allCharactersStats.Remove(removeObj.GetComponent<Stats>());
//         TallyGlobalStats();
//     }

//     void TallyGlobalStats(){
//         globalBrains = 0;
//         globalCharm = 0;
//         globalSpeed = 0;
//         globalBrainsMax = 0;
//         globalSpeedMax = 0;
//         globalCharmMax = 0;
//         for (int i = 0;i<allCharactersStats.Count;i++){
//             globalBrains += allCharactersStats[i].brains;
//             globalSpeed += allCharactersStats[i].speed;
//             globalCharm += allCharactersStats[i].charm;

//             if (allCharactersStats[i].brains > globalBrainsMax){
//                 globalBrainsMax = allCharactersStats[i].brains;
//             }
//             if (allCharactersStats[i].speed > globalSpeedMax){
//                 globalSpeedMax = allCharactersStats[i].speed;
//             }
//             if (allCharactersStats[i].charm > globalCharmMax){
//                 globalCharmMax = allCharactersStats[i].charm;
//             }
//         }
//         globalBrains/=allCharactersStats.Count;
//         globalSpeed/=allCharactersStats.Count;
//         globalCharm/=allCharactersStats.Count;
//         globalPopulation = allCharactersStats.Count;
//     }

//     public void AddPartyCharacter(GameObject newPartyMember, bool initial = false){
//         partyCharacterObjects.Add(newPartyMember);
//         partyCharacterStats.Add(newPartyMember.GetComponent<Stats>());
//         if (!initial){
//             newPartyMember.GetComponent<CharacterMove>().JoinParty();
//         }
//     }

//     // void TallyPartyStats(){
//     //     float age = 0;
//     //     float strength = 0;
//     //     for (int i = 0;i<partyCharacterStats.Count;i++){
//     //         age+=partyCharacterStats[i].age;
//     //         strength+=partyCharacterStats[i].strength;
//     //     }
//     //     manager.partyAge = age/partyCharacterStats.Count;
//     //     manager.partyStrength = strength/partyCharacterStats.Count;
//     // }

//     // public void WooSuccess(GameObject wooTarget){
//     //     AddPartyCharacter(wooTarget);
//     // }

// }
