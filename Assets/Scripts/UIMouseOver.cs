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

    void OnMouseEnter(){
        _manager.ui.DisplayStats(
            _cl.Face,
            _cl.MyName,
            _cl.Strength,
            _cl.Speed,
            _cl.Range,
            _cl.Tech,
            _cl.Age,
            _cl.Health,
            _cl.MaxHealth,
            transform.position
        );
    }

    void OnMouseExit(){
        _manager.ui.HideStats();
    }
}
