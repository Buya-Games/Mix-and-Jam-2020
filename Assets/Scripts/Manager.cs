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
    // Start is called before the first frame update
    void Awake()
    {
        spawner = GetComponent<EntitySpawner>();
        ui = GetComponent<UIManager>();
        statManager = GetComponent<EntityStatManager>();
        move = GetComponent<PlayerMove>();

        move.SetNewTarget(initialPlayer);
        statManager.AddCharacter(initialPlayer.gameObject);
        statManager.AddPartyCharacter(initialPlayer.gameObject);

        for (int i = 0;i<totalCharacters;i++){
            spawner.SpawnCharacter(new Vector3(Random.Range(-100,100),0,Random.Range(-100,100)),"Character" + i);
        }
        gameStarted = true;
    }

    public void CharacterDeath(GameObject deadChar){
        Debug.Log(deadChar.name + " has died");
    }
}
