using UnityEngine;

public class UIMouseOver: MonoBehaviour
{
    bool _mouseOver;
    Manager _manager;
    CreatureLogic _cl;

    void OnEnable(){
        _manager = FindObjectOfType<Manager>();
        _cl = GetComponent<CreatureLogic>();
    }

    void Update(){
        if (Input.GetMouseButtonDown(1)){
            _manager.ui.HideStats();    
        }
    }

    // void OnMouseEnter(){ //HIGHLIGHT ON
    // }

    // void OnMouseExit(){ //HIGHLIGHT OFF
    // }

    void OnMouseDown(){
        _manager.ui.DisplayStats(_cl,transform.position);
        
    }

    public void TakeControl(){
        _manager.PartyManager.ChangePlayer(_cl);
    }
}
