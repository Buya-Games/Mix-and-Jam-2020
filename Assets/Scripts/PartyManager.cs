using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> PartyMembers = new List<GameObject>();
    [HideInInspector] public List<GameObject> PotentialPartners = new List<GameObject>();
    Manager _manager;

    DropListener[] _grid;
    public List<Vector3> GridSpaces = new List<Vector3>();
    Stack<Vector3> _gridStack = new Stack<Vector3>();


    void Start(){
        _manager = GetComponent<Manager>();
        CreatePositionsQueue();
    }

    void CreatePositionsQueue(){//creates a queue of available offset positions closest to the player
        _gridStack.Clear();
        for (int i = 0;i<GridSpaces.Count;i++){
            _gridStack.Push(GridSpaces[i]);
        }
    }

    void AssignPartyPosition(CreatureLogic who){
        Vector3 pos = _gridStack.Pop();
        who.SetPosition(pos);
        GridSpaces.Remove(pos);
    }

    public void RecyclePartyPosition(Vector3 pos){
        _gridStack.Push(pos);
        GridSpaces.Add(pos);
    }

    public void CleanPotentials(GameObject except){
        foreach (GameObject potential in PotentialPartners){
            if (potential != except){
                Destroy(potential.GetComponent<PartnerLogic>());
                Destroy(potential.GetComponent<UIMouseOver>());
                _manager.spawner.RecycleCreature(potential);
            }
        }
        PartyMembers.Add(except);
        PotentialPartners.Clear();
    }

    public void ChangePlayer(GameObject newPlayer){
        if (newPlayer.layer == 9){
            _manager.player = newPlayer;
            _manager.move.SetNewTarget(newPlayer.transform);
            foreach (GameObject member in PartyMembers){
                member.GetComponent<CreatureLogic>().SetPartner();
                member.layer = 9;
            }
            newPlayer.GetComponent<CreatureLogic>()._player = true;
            newPlayer.GetComponent<CreatureLogic>()._party = false;
            newPlayer.layer = 8;
        } else {
            Debug.Log("invalid click: " + newPlayer.name);
        }
    }

    public void SelectPartner(GameObject partner){
        CleanPotentials(partner);
        
        GameObject child = _manager.spawner.SpawnCreature(_manager.player.transform.position,(_manager.player.name + " " + partner.name),false,true,true);
        child.AddComponent<UIMouseOver>();

        CreatureLogic childLogic = child.GetComponent<CreatureLogic>();
        CreatureLogic partnerLogic = partner.GetComponent<CreatureLogic>();
        partnerLogic.SetPartner();
        AssignPartyPosition(partnerLogic);
        childLogic.SetChild(_manager.player.GetComponent<CreatureLogic>(),partnerLogic);
        AssignPartyPosition(childLogic);

        PartyMembers.Add(partner);
        PartyMembers.Add(child);

        _manager.SpawnEnemies();
    }
    
}
