using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class CutsceneDirector : MonoBehaviour
{
    Manager manager;
    PlayableDirector director;
    [SerializeField] PlayableAsset[] cutscenes;
    bool openingScene = true;
    bool crashedRover, inspectRover, openingFight;
    [SerializeField] CinemachineVirtualCamera vcamCut, vcamGame;
    PlayerMove p;
    DialogueSystem dialogue;
    Words words;
    BoxCollider triggerBox;
    //[SerializeField] Transform enemy, saveTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Manager>();
        director = GetComponent<PlayableDirector>();
        p = FindObjectOfType<PlayerMove>();
        dialogue = FindObjectOfType<DialogueSystem>();
        words = FindObjectOfType<Words>();
        triggerBox = GetComponent<BoxCollider>();
    }

    
    public void StopPlay(){
        director.Stop();
    }

    void OnTriggerEnter(Collider col){
        if (openingScene){
            openingScene = false;
            triggerBox.center = new Vector3(0,1,91.6f);
            crashedRover = true;
            SwitchCamera();
            
            string s = manager.playerName;
            dialogue.SayThis(words.opening1, new string[5]{s,s,s,s,s});
        } else if (crashedRover){
            crashedRover = false;
            dialogue.SayThis(words.crashedRover,new string[1]{manager.playerName},"OpeningGiveControl");
            p.cutscene = false;
            //StartCoroutine(Yell());
            triggerBox.center = new Vector3(-7.7f,1,95.53f);
            triggerBox.size = new Vector3(6,1,5f);
            inspectRover = true;
        } else if (inspectRover){
            inspectRover = false;
            OpeningYell();
        } else if (openingFight){
            openingFight = false;
            OpeningWhatIsThat();
        }
    }

    public void FinishFade(){
        openingScene = true;
        p.OpeningCutsceneStart();
        // director.Play(cutscenes[0]);
        // director.extrapolationMode = DirectorWrapMode.Loop;
    }

    public void OpeningYell(){
        //OpeningSavingScene();
        dialogue.SayThis(words.yell,new string[1]{"???"});
        triggerBox.center = new Vector3(0,1,117f);
        triggerBox.size = new Vector3(50,1,1f);
        openingFight = true;
        director.Play(cutscenes[1]);
    }

    void SwitchCamera(){
        p.SetNewTarget(p.player);
        vcamGame.m_Priority = 11;
    }

    public void OpeningWhatIsThat(){
        dialogue.SayThis(words.whatisthat, new string[3]{"(%#@!&%",manager.playerName,"???"},"StartFight");
    }

    public void StartFight(){
        StopPlay();
        //enemy.GetComponent<EnemyLogic>().OpeningSequence(saveTarget);
        director.Play(cutscenes[2]);
    }

    public void EndFight(){
        StopPlay();
        dialogue.SayThis(words.thankYou, new string[6]{"???",manager.playerName,"???","???","???","???"},"HomeSweetHome");
    }

    public void HomeSweetHome(){
        director.Play(cutscenes[3]);
        dialogue.SayThis(words.hereWeAre, new string[3]{"???",manager.playerName,"???"},"GameOver");
    }

    public void GameOver(){
        director.Play(cutscenes[4]);
        dialogue.SayThis(words.gameover, new string[3]{"","",""},"ReturnToMenu");
    }
}
