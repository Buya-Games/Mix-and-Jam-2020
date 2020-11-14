using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] TMP_Text textPartyAge;
    [SerializeField] TMP_Text textPartyStrength;
    Manager manager;

    void Start(){
        manager = GetComponent<Manager>();
    }


    void Update(){
        if (manager.gameStarted){
            UpdateMainGUI();
        }
        
    }

    void UpdateMainGUI(){
        textPartyAge.text = "Party Age: " + manager.partyAge.ToString("F0");
        textPartyStrength.text = "Party Strength: " + manager.partyStrength.ToString("F0");
    }

    
}
