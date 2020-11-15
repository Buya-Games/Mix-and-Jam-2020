using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    Manager manager;
    UIManager ui;
    PlayerMove move;
    string textToWrite;
    [HideInInspector] public Queue<string> speakingQueue = new Queue<string>();
    [HideInInspector] public Queue<string> speakerQueue = new Queue<string>();
    TMP_Text textName, textTalking;
    bool speaking;
    string invokeThis;
    
    void Start()
    {
        manager = GetComponent<Manager>();
        ui = FindObjectOfType<UIManager>();
        move = FindObjectOfType<PlayerMove>();
        textName = ui.textName;
        textTalking = ui.textTalking;
    }

    public void SayThis(string[] stuff, string[] whoSays, string invoke = null){
        
        for (int i = 0;i<stuff.Length;i++){
            speakingQueue.Enqueue(stuff[i]);
            speakerQueue.Enqueue(whoSays[i]);
        }
        if (!speaking){
            NextSentence();
        }
        if (invoke != null){
            invokeThis = invoke;
        }
    }

    void NextSentence(){
        if (speakingQueue.Count>0){
            ui.StartSpeaking();
            ui.textName.text = speakerQueue.Dequeue();
            StartCoroutine(TypeSentence(speakingQueue.Dequeue()));
        }
    }

    IEnumerator TypeSentence (string sentence){//, int insertLocation, int exitLocation, string colorText) {
        speaking = true;
        move.StopControl();
        int characterIndex = 0;
        //FindObjectOfType<AudioManager>().PlaySound("FemaleVoice");
        foreach (char letter in sentence.ToCharArray()){
                characterIndex++;
                textToWrite = sentence.Substring(0,characterIndex) + "<color=#00000000>" + sentence.Substring(characterIndex);
                textTalking.text = textToWrite;
                textToWrite = "";
                yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        speaking = false;
        move.StopControl(true);
        if (invokeThis != null && speakingQueue.Count==0){
            Invoke(invokeThis,0f);
            invokeThis = null;
        } else {
            NextSentence();
        }
    }

    void OpeningGiveControl(){
        move.OpeningCutsceneGiveControl();
        ui.StartSpeaking(false);
    }

    void StartFight(){
        FindObjectOfType<CutsceneDirector>().StartFight();
        ui.StartSpeaking(false);
    }

    void HomeSweetHome(){
        FindObjectOfType<CutsceneDirector>().HomeSweetHome();
    }

    void GameOver(){
        FindObjectOfType<CutsceneDirector>().GameOver();
    }

    void ReturnToMenu(){
        Debug.Log("goodnight yuji... better luck next time");
    }
    
}
