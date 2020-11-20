using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Manager : MonoBehaviour
{
    [HideInInspector] public EntitySpawner spawner;
    [HideInInspector] public UIManager ui;
    [HideInInspector] public EntityStatManager statManager;
    [HideInInspector] public PlayerMove move;
    [HideInInspector] public bool gameStarted;
    [SerializeField] int totalCharacters, totalItems, totalEnemy;
    [SerializeField] Transform initialPlayer;
    GameObject _wooTarget;
    [HideInInspector] public bool wooing; //to avoid triggering more than one woo at a time
    [HideInInspector] public GameObject player;
    [HideInInspector] public Stats playerStats;
    MatingProcess matingProcess;
    [HideInInspector] public Particles particles;
    [HideInInspector] public float difficulty = 1;
    public string playerName;
    public float hitForce;

    // Start is called before the first frame update
    void Awake()
    {
        //Time.timeScale = 20;
        spawner = GetComponent<EntitySpawner>();
        ui = GetComponent<UIManager>();
        statManager = GetComponent<EntityStatManager>();
        move = GetComponent<PlayerMove>();
        matingProcess = GetComponent<MatingProcess>();
        particles = GetComponent<Particles>();

        //NewBaby(initialPlayer.gameObject, false);
        move.SetNewTarget(initialPlayer);
        statManager.AddCharacter(initialPlayer.gameObject);
        player = initialPlayer.gameObject;
        playerStats = initialPlayer.GetComponent<Stats>();
        //statManager.AddPartyCharacter(initialPlayer.gameObject, true);

    }

    void Start(){
        for (int i = 0;i<totalCharacters;i++){
            spawner.SpawnCharacter(new Vector3(Random.Range(-1,1),1.1f,Random.Range(-1,1)),i.ToString("F0"));
        }
        for (int i = 0;i<totalItems;i++){
            spawner.SpawnItem(new Vector3(Random.Range(-100,100),1.1f,Random.Range(-10,50)));
        }
        for (int i = 0;i<totalEnemy;i++){
            Vector3 where = new Vector3(Random.Range(-100,100),1.1f,Random.Range(-10,50));
            GameObject newEnemy = spawner.SpawnEnemy(where);
            newEnemy.GetComponent<EnemyLogic>().PlaceEnemy(where);
        }
        gameStarted = true;
        Vector3 origScale = initialPlayer.transform.localScale;
        initialPlayer.transform.localScale = Vector3.zero;
        initialPlayer.transform.DOScale(origScale,1f);
        particles.PlayBaby(initialPlayer.position);
        
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

    public void NewBabyCharacter(GameObject newBaby){
        statManager.AddPartyCharacter(newBaby);
    }

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
