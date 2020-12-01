using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    [SerializeField] TMP_Text textGold;
    [SerializeField] GameObject panelTalking, btn1, btn2, btn3, btn4, face, healthBarPrefab;
    public TMP_Text textName, textTalking;
    [SerializeField] TMP_Text textPopUp;
    Queue<TMP_Text> popupTexts = new Queue<TMP_Text>();
    [SerializeField] GameObject[] partyUIs;
    [HideInInspector] public Queue<GameObject> partyUIQueue = new Queue<GameObject>();
    Queue<GameObject> _healthBarsQueue = new Queue<GameObject>();
    [SerializeField] FaceDisplay _faceStats;
    [SerializeField] GameObject charaterStatsDisplay;
    [SerializeField] FaceDisplay _gridFacePrefab;

    [SerializeField] List<GameObject> _listGridSpots = new List<GameObject>();//I need a list cuz a Stack isn't visible in Editor
    Stack<GameObject> _stackGridSpots = new Stack<GameObject>();
    
    [SerializeField] TMP_Text textStatsDisplayNameAge, textStatsDisplayStrengthSpeed, textStatsDisplayTechRange, textStatsDisplayHealth;

    Manager manager;

    void Awake(){
        manager = GetComponent<Manager>();
        LoadPopUpTexts();
        InitializePartyUI();
        CreateHealthBar(10);
        
    }

    void Start(){
        _faceStats = _faceStats.GetComponent<FaceDisplay>();
    }

    public void StartSpeaking(bool start = true){
        panelTalking.SetActive(start);
    }

    public void UpdateGoldText(){
        textGold.text = "Gold: " + manager.GoldBalance;
    }

    void InitializePartyUI(){
        // for (int i = 0;i<partyUIs.Length;i++){
        //     partyUIQueue.Enqueue(partyUIs[i]);
        //     partyUIs[i].SetActive(false);
        // }
        for (int i = 0;i<_listGridSpots.Count;i++){
            _stackGridSpots.Push(_listGridSpots[i]);
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

    public void DisplayStats(float[] displayFace, string name, float strength, float speed, float range, float tech, float age, float health, float maxHealth, Vector3 where){
        textStatsDisplayNameAge.text = name + " (" + age.ToString("F0") + ")";
        textStatsDisplayHealth.text = health.ToString("F0") + "/" + maxHealth.ToString("F0");
        textStatsDisplayStrengthSpeed.text = "STR: " + strength.ToString("F2") + " SPEED: " + speed.ToString("F2");
        textStatsDisplayTechRange.text = "TECH: " + tech.ToString("F2") + " RANGE: " + range.ToString("F2");
        where.y = 5;
        charaterStatsDisplay.transform.position = where;
        _faceStats.DisplayFace(displayFace);
        charaterStatsDisplay.SetActive(true);
    }

    public void HideStats(){
        charaterStatsDisplay.SetActive(false);
    }

    public void AddFaceToGrid(float[] face, CreatureLogic who){
        Transform spot = _stackGridSpots.Pop().transform;
        FaceDisplay newFace = Instantiate(_gridFacePrefab,Vector3.zero,Quaternion.identity,spot) as FaceDisplay;
        newFace.GetComponent<DragDrop>().MyCreature = who;
        newFace.GetComponent<RectTransform>().localPosition = new Vector3(0,-14,0);
        newFace.DisplayFace(face);
    }

    public void RemoveFaceFromGrid(GameObject spot, FaceDisplay face){
        Destroy(face.gameObject);
        _stackGridSpots.Push(spot);
    }



    void LoadPopUpTexts(){
        for (int i = 0;i<10;i++){
            GameObject newPopup = Instantiate(textPopUp.gameObject,textPopUp.transform.position,textPopUp.transform.rotation,textPopUp.transform.parent);
            popupTexts.Enqueue(newPopup.GetComponent<TMP_Text>());
            newPopup.SetActive(false);
        }
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

