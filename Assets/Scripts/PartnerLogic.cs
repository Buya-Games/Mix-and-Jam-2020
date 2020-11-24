using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerLogic : MonoBehaviour
{
    void OnCollisionEnter(Collision col){
        if (col.gameObject.layer == 8){
            FindObjectOfType<Manager>().SelectPartner(this.gameObject);
            Destroy(this);
        }
    }
}
