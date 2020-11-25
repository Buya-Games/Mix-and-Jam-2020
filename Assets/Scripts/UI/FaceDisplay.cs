using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceDisplay : MonoBehaviour
{
    [SerializeField] Image faceSpot;
    [SerializeField] Image browSpot, eyesSpot, noseSpot, mouthSpot, hairSpot;
    FaceGenerator _face;
    // Start is called before the first frame update
    void Awake(){
        _face = FindObjectOfType<FaceGenerator>();
        // browSpot = browSpot.GetComponent<Image>();
        // eyesSpot = eyesSpot.GetComponent<Image>();
        // noseSpot = noseSpot.GetComponent<Image>();
        // mouthSpot = mouthSpot.GetComponent<Image>();
    }

    public void DisplayFace(float[] face){
        browSpot.sprite = _face.GetBrows((int)face[0]);
        eyesSpot.sprite = _face.GetEyes((int)face[1]);
        noseSpot.sprite = _face.GetNose((int)face[2]);
        mouthSpot.sprite = _face.GetMouth((int)face[3]);
        hairSpot.sprite = _face.GetHair((int)face[4]);
        faceSpot.color = _face.GetFaceColor(face[5]);
        hairSpot.color = _face.GetHairColor(face[6]);
    }
}
