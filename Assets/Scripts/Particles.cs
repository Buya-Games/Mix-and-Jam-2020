﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public ParticleSystem baby, item, enemyHit, friendlyHit, bombHit;
    
    public void PlayBaby(Vector3 where){
        baby.transform.position = where;
        baby.Play();
    }
    public void PlayItem(Vector3 where){
        item.transform.position = where;
        item.Play();
    }

    public void EnemyHit(Vector3 where){
        enemyHit.transform.position = where;
        enemyHit.Play();
    }

    public void FriendlyHit(Vector3 where){
        friendlyHit.transform.position = where;
        friendlyHit.Play();
    }

    public void BombHit(Vector3 where){
        where.y = 0.1f;
        bombHit.transform.position = where;
        bombHit.Play();
    }
    
}
