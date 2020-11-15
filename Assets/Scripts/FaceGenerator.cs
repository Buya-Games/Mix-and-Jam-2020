using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FaceGenerator : MonoBehaviour
{
    Sprite[] brows, eyes, noses, mouth; // Source: https://designalikie.com/charatore/
    [SerializeField] Sprite[] hair; //Asset: Unruly Games 2000+ Faces
    [SerializeField] Image faceSpot;
    [SerializeField] Image browSpot, eyesSpot, noseSpot, mouthSpot, hairSpot;
    int browsNo, eyesNo, nosesNo, mouthNo;
    [SerializeField] Gradient faceGradient, hairGradient;
    int hairNo = 12;
    // Start is called before the first frame update
    void Start()
    {
        browSpot = browSpot.GetComponent<Image>();
        eyesSpot = eyesSpot.GetComponent<Image>();
        noseSpot = noseSpot.GetComponent<Image>();
        mouthSpot = mouthSpot.GetComponent<Image>();

        Sprite[] brows1 = Resources.LoadAll<Sprite>("brows1");
        Sprite[] brows2 = Resources.LoadAll<Sprite>("brows2");
        Sprite[] eyes1 = Resources.LoadAll<Sprite>("eyes1");
        Sprite[] eyes2 = Resources.LoadAll<Sprite>("eyes2");
        Sprite[] noses1 = Resources.LoadAll<Sprite>("noses1");
        Sprite[] noses2 = Resources.LoadAll<Sprite>("noses2");
        Sprite[] mouth1 = Resources.LoadAll<Sprite>("mouth1");
        Sprite[] mouth2 = Resources.LoadAll<Sprite>("mouth2");

        brows = brows1.Concat(brows2).ToArray();
        eyes = eyes1.Concat(eyes2).ToArray();
        noses = noses1.Concat(noses2).ToArray();
        mouth = mouth1.Concat(mouth2).ToArray();

        browsNo = brows.Length;
        eyesNo = eyes.Length;
        nosesNo = noses.Length;
        mouthNo = mouth.Length;
    }

    public void RandomFace(){
        GenerateFace(
            Random.Range(0,browsNo),
            Random.Range(0,eyesNo),
            Random.Range(0,nosesNo),
            Random.Range(0,mouthNo),
            Random.Range(0,hairNo),
            Random.Range(0,1f),
            Random.Range(0,1f)
        );
    }

    public void GenerateFace(int browsIndex, int eyesIndex, int noseIndex, int mouthIndex, int hairIndex, float hairColor, float faceColor){
        browSpot.sprite = brows[browsIndex];
        eyesSpot.sprite = eyes[eyesIndex];
        noseSpot.sprite = noses[noseIndex];
        mouthSpot.sprite = mouth[mouthIndex];
        hairSpot.sprite = hair[hairIndex];
        faceSpot.color = faceGradient.Evaluate(faceColor);
        hairSpot.color = hairGradient.Evaluate(hairColor);
    }
}
