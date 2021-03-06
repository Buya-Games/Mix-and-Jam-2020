﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Manager : MonoBehaviour
{
    
    [HideInInspector] public Spawner spawner;
    [HideInInspector] public UIManager ui;
    [HideInInspector] public PartyManager PartyManager;
    [HideInInspector] public PlayerMove move;
    [HideInInspector] public FaceGenerator FaceGenerator;
    [HideInInspector] public bool gameStarted;
    
    [SerializeField] int totalCharacters, totalEnemy;
    [SerializeField] Transform initialPlayer;
    [HideInInspector] public GameObject player;
    //MatingProcess matingProcess;
    [HideInInspector] public Particles particles;
    public string playerName;
    public float hitForce;
    public LayerMask enemyLM, friendlyLM, PlayerOnlyLM;
    [HideInInspector] public int GoldBalance;
    public Material FovMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        spawner = GetComponent<Spawner>();
        ui = GetComponent<UIManager>();
        move = GetComponent<PlayerMove>();
        PartyManager = GetComponent<PartyManager>();
        particles = GetComponent<Particles>();
        FaceGenerator = GetComponentInChildren<FaceGenerator>();
        player = initialPlayer.gameObject;
    }

    void Start(){
        InitializeFirstPlayer();
        SpawnPartners();
    }

    void InitializeFirstPlayer(){
        CreatureLogic playerLogic = player.GetComponent<CreatureLogic>();
        playerLogic.SetCreature(player.transform.position,false);
        PartyManager.PartyMembers.Add(player);
        move.SetNewTarget(initialPlayer);
        ui.AddFaceToGrid(playerLogic.MyGridFace);
    }

    public void SpawnPartners(){
        int xLoc = 0;
        for (int i = 0;i<totalCharacters;i++){
            GameObject potential = spawner.SpawnCreature(new Vector3(xLoc,1.1f,15),"partner " + i.ToString("F0"), false,false,true);
            potential.AddComponent<PartnerLogic>();
            potential.AddComponent<UIMouseOver>();
            PartyManager.PotentialPartners.Add(potential);
            xLoc+=5;
        }
    }

    public void SpawnEnemies(){
        for (int i = 0;i<totalEnemy;i++){
            spawner.SpawnCreature(new Vector3(Random.Range(-100,100),1.1f,Random.Range(50,100)),"enemy " + i.ToString("F0"), true);
        }
        gameStarted = true;
    }

    // public void BeginWoo(GameObject wooTarget){
    //     wooing = true;
    //     ui.DisplayWoo(wooTarget.GetComponent<Stats>());
    //     _wooTarget = wooTarget;
    // }

    // public void Woo(){
    //     float x = (playerStats.charm * Random.Range(1,1.2f)) - _wooTarget.GetComponent<Stats>().charm;
    //     if (x > 0){
    //         //statManager.WooSuccess(_wooTarget);
    //         //Debug.Log(_wooTarget.name + "wooed!" + " your charm:" + playerStats.charm + ", partner charm: " + _wooTarget.GetComponent<Stats>().charm);
    //         ui.ShowResult("Baby Born!");
    //         //ByeMomByeDad(player,_wooTarget);
    //         matingProcess.Congratulations(player,_wooTarget,player.transform.position + (Vector3.forward * 2), false);
    //         statManager.AddPartyCharacter(_wooTarget);

    //     } else {
    //         ui.ShowResult("Woo Failed!");
    //         WooFail();
    //     }
    //     ui.CloseWoo();
    //     _wooTarget.GetComponent<CharacterMove>().StopAllCoroutines(); //stops the TalkToMe coroutine in HumanLogic
    //     wooing = false;
    // }

    // public void WooCPU(GameObject woo1, GameObject woo2){
    //     HumanLogic woo1Logic = woo1.GetComponent<HumanLogic>();
    //     HumanLogic woo2Logic = woo2.GetComponent<HumanLogic>();
    //     if (woo1Logic.GetInstanceID() < woo2Logic.GetInstanceID()){ //makes sure only one of the collisions triggers this job
    //         //Debug.Log("wooing " + woo1.name + " and " + woo2.name + " at " + Time.time);
    //         float x = (woo1.GetComponent<Stats>().charm * Random.Range(0.8f,1.2f)) - woo2.GetComponent<Stats>().charm;
    //         if (x > 0){
    //             ByeMomByeDad(woo1,woo2);
    //             int noKids = Random.Range(1,4);
    //             int leftRight = 1;
    //             for (int i = 0; i < noKids; i++){
    //                 leftRight *= -1; // spawning kids to left/right to avoid them mating with each other
    //                 matingProcess.Congratulations(woo1,woo2,woo2.transform.position + (Vector3.right * (i + 1) * 10 * leftRight),true);
    //             }
    //         } else {
    //             woo1Logic.WooFail();
    //             woo2Logic.WooFail();
    //         }
    //     }
    // }

    // public void WooFail(){
    //     ui.CloseWoo();
    //     _wooTarget.GetComponent<CharacterMove>().WooFail();
    //     Debug.Log(_wooTarget.name + "failed!");
    //     wooing = false;
    // }

    // public void NewBabyCharacter(GameObject newBaby){
    //     statManager.AddPartyCharacter(newBaby);
    // }

    // public void NewBaby(GameObject newPlayer, bool cpu){
    //     // Rigidbody rb = newPlayer.AddComponent<Rigidbody>();
    //     // rb.isKinematic = true;
    //     // rb.useGravity = false;

    //     CharacterMove newMove = newPlayer.GetComponent<CharacterMove>();
    //     if (!cpu){
    //         newMove.isPlayer = true;
    //         newMove.wooAble = false;

    //         player = newPlayer;
    //         playerStats = newPlayer.GetComponent<Stats>();
    //         move.SetNewTarget(newPlayer.transform);
    //     } else{
    //         newMove.wooAble = true;
    //         newMove.moving = true;
    //         //newMove.dontMove = false;
    //     }

    //     statManager.AddCharacter(newPlayer);
    //     spawner.JudgeMe(newPlayer);
    // }

    // void ByeMomByeDad(GameObject mom, GameObject dad){
    //     mom.transform.position = dad.transform.position + (Vector3.right * 2);
    //     mom.layer = 10;
    //     dad.layer = 10;
    //     HumanLogic momMove = mom.GetComponent<HumanLogic>();
    //     HumanLogic dadMove = dad.GetComponent<HumanLogic>();
    //     momMove.isPlayer = false;
    //     momMove.dontMove = false;
    //     dadMove.isPlayer = false;
    //     dadMove.dontMove = false;
    //     statManager.RemoveFromGlobalPool(mom);
    //     statManager.RemoveFromGlobalPool(dad);
    //     spawner.CircleOfLife(mom);
    //     spawner.CircleOfLife(dad);
    // }

    // public void CharacterDeath(GameObject deadChar){
    //     Debug.Log(deadChar.name + " has died");
    // }
}
