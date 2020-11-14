using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] TMP_Text textAge, textSpeed, textBrains, textCharm, textGlobalSpeed, textGlobalBrains, textGlobalCharm, textGlobalPopulation;
    [SerializeField] GameObject panelWoo, btnWoo;
    [SerializeField] TMP_Text textWooName;

    Manager manager;

    void Start(){
        manager = GetComponent<Manager>();
    }


    void Update(){
        if (manager.gameStarted){
            UpdateMainGUI();
            UpdateGlobalStatsGUI();
        }
        
    }

    void UpdateMainGUI(){
        textAge.text = "Age: " + manager.playerStats.age.ToString("F0");
        textSpeed.text = "Speed: " + manager.playerStats.speed.ToString("F0");
        textBrains.text = "Brains: " + manager.playerStats.brains.ToString("F0");
        textCharm.text = "Charm: " + manager.playerStats.charm.ToString("F0");
        //textPartyStrength.text = "Party Charm: " + manager.partyStrength.ToString("F0");
        // textPartyAge.text = "Party Age: " + manager.partyAge.ToString("F0");
        // textPartyStrength.text = "Party Strength: " + manager.partyStrength.ToString("F0");
    }

    void UpdateGlobalStatsGUI(){
        textGlobalSpeed.text = "Global Speed: " + manager.statManager.globalSpeed.ToString("F0") + " (Max: " + manager.statManager.globalSpeedMax.ToString("F0") + ")";
        textGlobalBrains.text = "Global Brains: " + manager.statManager.globalBrains.ToString("F0")  + " (Max: " + manager.statManager.globalBrainsMax.ToString("F0") + ")";
        textGlobalCharm.text = "Global Charm: " + manager.statManager.globalCharm.ToString("F0")  + " (Max: " + manager.statManager.globalCharmMax.ToString("F0") + ")";
        textGlobalPopulation.text = "Global Pop: " + manager.statManager.globalPopulation.ToString("F0");

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

