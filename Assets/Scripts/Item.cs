using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Manager _manager;

    protected virtual void Start(){
        _manager = FindObjectOfType<Manager>();
    }
    protected virtual void OnCollisionEnter(Collision col){
        _manager.GoldBalance++;
        _manager.ui.UpdateGoldText();
        _manager.particles.PlayItem(transform.position);
        _manager.spawner.RecycleItem(this.gameObject);

    }
}
