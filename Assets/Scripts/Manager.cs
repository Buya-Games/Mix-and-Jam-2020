using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [HideInInspector] public float partyAge;
    [HideInInspector] public float partyStrength;
    [HideInInspector] public EntitySpawner spawner;
    [HideInInspector] public UIManager ui;
    [HideInInspector] public EntityStatManager statManager;
    [HideInInspector] public PlayerMove move;
    [HideInInspector] public bool gameStarted;
    [SerializeField] int totalCharacters;
    [SerializeField] Transform initialPlayer;
    GameObject _wooTarget;
    [HideInInspector] public bool wooing; //to avoid triggering more than one woo at a time

    // Start is called before the first frame update
    void Awake()
    {
        spawner = GetComponent<EntitySpawner>();
        ui = GetComponent<UIManager>();
        statManager = GetComponent<EntityStatManager>();
        move = GetComponent<PlayerMove>();

        move.SetNewTarget(initialPlayer);
        statManager.AddCharacter(initialPlayer.gameObject);
        statManager.AddPartyCharacter(initialPlayer.gameObject, true);

        for (int i = 0;i<totalCharacters;i++){
            spawner.SpawnCharacter(new Vector3(Random.Range(-100,100),1.1f,Random.Range(-100,100)),"Character" + i);
        }
        gameStarted = true;
    }

    public void BeginWoo(GameObject wooTarget){
        wooing = true;
        ui.DisplayWoo(wooTarget.GetComponent<Stats>());
        _wooTarget = wooTarget;
    }

    public void Woo(){
        int x = Random.Range(0,2);
        if (x == 1){
            statManager.WooSuccess(_wooTarget);
            Debug.Log(_wooTarget.name + "wooed!");
        } else {
            WooFail();
        }
        ui.CloseWoo();
        _wooTarget.GetComponent<CharacterMove>().StopAllCoroutines(); //stops the TalkToMe coroutine in CharacterMove
        wooing = false;
    }

    public void WooFail(){
        ui.CloseWoo();
        _wooTarget.GetComponent<CharacterMove>().WooFail();
        Debug.Log(_wooTarget.name + "failed!");
        wooing = false;
    }

    public void CharacterDeath(GameObject deadChar){
        Debug.Log(deadChar.name + " has died");
    }
}
