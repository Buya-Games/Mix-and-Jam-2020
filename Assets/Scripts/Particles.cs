using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public ParticleSystem baby, item;
    
    public void PlayBaby(Vector3 where){
        baby.transform.position = where;
        baby.Play();
    }
    public void PlayItem(Vector3 where){
        item.transform.position = where;
        item.Play();
    }
}
