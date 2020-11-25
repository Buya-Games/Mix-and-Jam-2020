using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FaceGenerator : MonoBehaviour
{
    Sprite[] _brows, _eyes, _noses, _mouth; // Source: https://designalikie.com/charatore/
    [SerializeField] Sprite[] hair; //Asset: Unruly Games 2000+ Faces
    int _browsNo, _eyesNo, _nosesNo, _mouthNo;
    int _hairNo = 12;//this loaded directly in Editor instead of from /Resources/ like the others
    [SerializeField] string[] consonants, vowels;
    [SerializeField] Gradient _faceGradient, _hairGradient;
    
    // Start is called before the first frame update
    void Start()
    {
        Sprite[] brows1 = Resources.LoadAll<Sprite>("brows1");
        Sprite[] brows2 = Resources.LoadAll<Sprite>("brows2");
        Sprite[] eyes1 = Resources.LoadAll<Sprite>("eyes1");
        Sprite[] eyes2 = Resources.LoadAll<Sprite>("eyes2");
        Sprite[] noses1 = Resources.LoadAll<Sprite>("noses1");
        Sprite[] noses2 = Resources.LoadAll<Sprite>("noses2");
        Sprite[] mouth1 = Resources.LoadAll<Sprite>("mouth1");
        Sprite[] mouth2 = Resources.LoadAll<Sprite>("mouth2");

        _brows = brows1.Concat(brows2).ToArray();
        _eyes = eyes1.Concat(eyes2).ToArray();
        _noses = noses1.Concat(noses2).ToArray();
        _mouth = mouth1.Concat(mouth2).ToArray();

        _browsNo = _brows.Length;
        _eyesNo = _eyes.Length;
        _nosesNo = _noses.Length;
        _mouthNo = _mouth.Length;
    }

    public string GenerateName(){
        string rando = "";
        bool vowel = false;
        int nameLength = Random.Range(3,6);
        for (int i = 0;i<nameLength;i++){
            if (vowel){
                vowel = false;
                int character = Random.Range(0,vowels.Length);
                rando+=vowels[character];
            } else {
                vowel = true;
                int character = Random.Range(0,consonants.Length);
                rando+=consonants[character];
            }
        }
        return rando;
    }

    public float[] GenerateRandomFace(){
        float[] face = new float[7];
        face[0] = (int)Random.Range(0,_browsNo);
        face[1] = (int)Random.Range(0,_eyesNo);
        face[2] = (int)Random.Range(0,_nosesNo);
        face[3] = (int)Random.Range(0,_mouthNo);
        face[4] = (int)Random.Range(0,_hairNo);
        face[5] = Random.Range(0,1f);
        face[6] = Random.Range(0,1f);
        return face;
    }

    public Sprite GetBrows(int browsIndex){
        return _brows[browsIndex];
    }
    public Sprite GetEyes(int eyesIndex){
        return _eyes[eyesIndex];
    }
    public Sprite GetNose(int noseIndex){
        return _noses[noseIndex];
    }
    public Sprite GetMouth(int mouthIndex){
        return _mouth[mouthIndex];
    }
    public Sprite GetHair(int hairIndex){
        return hair[hairIndex];
    }
    public Color GetFaceColor(float skinColor){
        return _faceGradient.Evaluate(skinColor);
    }
    public Color GetHairColor(float hairColor){
        return _hairGradient.Evaluate(hairColor);
    }

    // public void RandomFace(){
    //     GenerateFace(
    //         Random.Range(0,_browsNo),
    //         Random.Range(0,_eyesNo),
    //         Random.Range(0,_nosesNo),
    //         Random.Range(0,_mouthNo),
    //         Random.Range(0,_hairNo),
    //         Random.Range(0,1f),
    //         Random.Range(0,1f)
    //     );
    // }
    
    // public void GenerateFace(int browsIndex, int eyesIndex, int noseIndex, int mouthIndex, int hairIndex, float hairColor, float faceColor){
    //     browSpot.sprite = brows[browsIndex];
    //     eyesSpot.sprite = eyes[eyesIndex];
    //     noseSpot.sprite = noses[noseIndex];
    //     mouthSpot.sprite = mouth[mouthIndex];
    //     hairSpot.sprite = hair[hairIndex];
    //     faceSpot.color = faceGradient.Evaluate(faceColor);
    //     hairSpot.color = hairGradient.Evaluate(hairColor);
    // }
}
