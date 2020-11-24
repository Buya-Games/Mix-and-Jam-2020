using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    [SerializeField] TMP_Text textAge, textSpeed, textBrains, textCharm, textGlobalSpeed, textGlobalBrains, textGlobalCharm, textGlobalPopulation, textResult;
    [SerializeField] GameObject panelTalking, btn1, btn2, btn3, btn4, face, healthBarPrefab;
    public TMP_Text textName, textTalking;
    [SerializeField] TMP_Text textPopUp;
    Queue<TMP_Text> popupTexts = new Queue<TMP_Text>();
    [SerializeField] GameObject[] partyUIs;
    [HideInInspector] public Queue<GameObject> partyUIQueue = new Queue<GameObject>();
    Queue<GameObject> _healthBarsQueue = new Queue<GameObject>();

    Manager manager;

    void Awake(){
        manager = GetComponent<Manager>();
        LoadPopUpTexts();
        InitializePartyUI();
        CreateHealthBar(10);
    }


    void Update(){
        if (manager.gameStarted){
            UpdateMainGUI();
            //UpdateGlobalStatsGUI();
        }
        
    }

    public void StartSpeaking(bool start = true){
        panelTalking.SetActive(start);

    }

    void InitializePartyUI(){
        for (int i = 0;i<partyUIs.Length;i++){
            partyUIQueue.Enqueue(partyUIs[i]);
            partyUIs[i].SetActive(false);
        }
    }

    void CreateHealthBar(int howMany){
        for (int i = 0; i<howMany;i++){
            GameObject newBar = Instantiate(healthBarPrefab,Vector3.zero,Quaternion.identity);
            _healthBarsQueue.Enqueue(newBar);
            newBar.SetActive(false);
        }
    }

    public Transform CreateMyHealthBar(Transform parent){
        if (_healthBarsQueue.Count == 0){
            CreateHealthBar(1);
        }
        Transform newBar = _healthBarsQueue.Dequeue().transform;
        newBar.SetParent(parent);
        float rando = Random.Range(1.75f,2.25f);
        newBar.localPosition = new Vector3(0,rando,0);
        newBar.localRotation = Quaternion.Euler(45,0,0);
        newBar.gameObject.SetActive(true);
        return newBar;
    }

    public void RemoveHealthBar(GameObject healthBar){
        _healthBarsQueue.Enqueue(healthBar);
        healthBar.SetActive(false);

    }


    void LoadPopUpTexts(){
        for (int i = 0;i<10;i++){
            GameObject newPopup = Instantiate(textPopUp.gameObject,textPopUp.transform.position,textPopUp.transform.rotation,textPopUp.transform.parent);
            popupTexts.Enqueue(newPopup.GetComponent<TMP_Text>());
            newPopup.SetActive(false);
        }
    }

    void UpdateMainGUI(){
        // textAge.text = "Your Age: " + manager.playerStats.age.ToString("F0");
        // textSpeed.text = "Your Speed: " + manager.playerStats.speed.ToString("F0") + " (" + (manager.playerStats.speed-33).ToString("+0;-#") + ")";
        // textBrains.text = "Your Brains: " + manager.playerStats.brains.ToString("F0") + " (" + (manager.playerStats.brains-33).ToString("+0;-#") + ")";
        // textCharm.text = "Your Charm: " + manager.playerStats.charm.ToString("F0") + " (" + (manager.playerStats.charm-33).ToString("+0;-#") + ")";
        //textPartyStrength.text = "Party Charm: " + manager.partyStrength.ToString("F0");
        // textPartyAge.text = "Party Age: " + manager.partyAge.ToString("F0");
        // textPartyStrength.text = "Party Strength: " + manager.partyStrength.ToString("F0");
    }

    // void UpdateGlobalStatsGUI(){
    //     textGlobalSpeed.text = "Global Speed: " + manager.statManager.globalSpeed.ToString("F0") + " (Max: " + manager.statManager.globalSpeedMax.ToString("F0") + ")";
    //     textGlobalBrains.text = "Global Brains: " + manager.statManager.globalBrains.ToString("F0")  + " (Max: " + manager.statManager.globalBrainsMax.ToString("F0") + ")";
    //     textGlobalCharm.text = "Global Charm: " + manager.statManager.globalCharm.ToString("F0")  + " (Max: " + manager.statManager.globalCharmMax.ToString("F0") + ")";
    //     textGlobalPopulation.text = "Global Pop: " + manager.statManager.globalPopulation.ToString("F0");

    // }

    public void ShowResult(string text){
        textResult.text = text;
        StartCoroutine(ClearResult());
    }

    IEnumerator ClearResult(){
        yield return new WaitForSeconds(1);
        textResult.text = "";
    }

    public void DisplayWoo(Stats wooTargetStats){
        //btnWoo.onClick.AddListener(() => SelectRegion(myNumber, myAbrv, myFullname));//, newBtn.GetComponent<RectTransform>()));      
        panelTalking.gameObject.SetActive(true);
        textName.text = wooTargetStats.myName;
    }

    public void CloseWoo(){
        panelTalking.gameObject.SetActive(false);
    }

    public void HitGUI(Vector3 pos, float amount, Color whatColor, bool critical = false){
        DisplayPopup(pos,"-" + amount.ToString("F0"), whatColor);
    }

    void DisplayPopup(Vector3 where, string what, Color whatColor){
        if (popupTexts.Count > 0){
            //where.y -=25;
            TMP_Text popup = popupTexts.Dequeue();
            Vector3 origSize = popup.transform.localScale;
            popup.transform.localScale = Vector3.zero;
            popup.color = whatColor;
            popup.text = what;
            popup.transform.position = where;// + new Vector3(Random.Range(-1f,1f),0,0);//Random.Range(-3f,3f));
            popup.gameObject.SetActive(true);
            popup.transform.DOScale(origSize,0.1f).OnComplete(() => {
                popup.transform.DOPunchScale(origSize*1.2f,0.3f,5,0.2f);
                popup.transform.DOMoveY(where.y+10,1).OnComplete(() => popup.gameObject.SetActive(false));
                popupTexts.Enqueue(popup);
            });
        }
    }
}

