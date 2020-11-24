using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> PartyMembers = new List<GameObject>();
    [HideInInspector] public List<GameObject> PotentialPartners = new List<GameObject>();
    Manager _manager;

    void Start(){
        _manager = GetComponent<Manager>();
    }

    public void CleanPotentials(GameObject except){
        foreach (GameObject potential in PotentialPartners){
            if (potential != except){
                Destroy(potential.GetComponent<PartnerLogic>());
                _manager.spawner.RecyleCreature(potential);
            }
        }
        PartyMembers.Add(except);
        PotentialPartners.Clear();
    }
    
}
