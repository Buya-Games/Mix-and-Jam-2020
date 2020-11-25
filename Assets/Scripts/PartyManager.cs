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
                Destroy(potential.GetComponent<UIMouseOver>());
                _manager.spawner.RecyleCreature(potential);
            }
        }
        PartyMembers.Add(except);
        PotentialPartners.Clear();
    }

    public void ChangePlayer(GameObject newPlayer){
        if (PartyMembers.Contains(newPlayer)){
            _manager.player = newPlayer;
            _manager.move.SetNewTarget(newPlayer.transform);
            foreach (GameObject member in PartyMembers){
                member.GetComponent<CreatureLogic>().SetPartner();
            }
            newPlayer.GetComponent<CreatureLogic>()._player = true;
            newPlayer.GetComponent<CreatureLogic>()._party = false;
        } else {
            Debug.Log("clicked creature is not a party member");
        }
    }
    
}
