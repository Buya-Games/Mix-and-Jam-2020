using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] TMP_Text textPartyAge, textPartyStrength;
    [SerializeField] GameObject panelWoo, btnWoo;
    [SerializeField] TMP_Text textWooName;

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

    public void DisplayWoo(Stats wooTargetStats){
        //btnWoo.onClick.AddListener(() => SelectRegion(myNumber, myAbrv, myFullname));//, newBtn.GetComponent<RectTransform>()));      
        panelWoo.gameObject.SetActive(true);
        textWooName.text = wooTargetStats.myName;
    }

    public void CloseWoo(){
        panelWoo.gameObject.SetActive(false);
    }
}

