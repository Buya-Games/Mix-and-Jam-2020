using UnityEngine;

public class AliveCreature : MonoBehaviour
{
    Manager manager;
    public string myName;
    public float age, speed, health, strength, brains, charm, heal;
    public bool alive, enemy, isPlayer, partyMember;
    public enum State {attack, patrol, chasing};
    //Animator _anim;
    public Vector3 _target;
    State _state;

    void Start(){
        manager = FindObjectOfType<Manager>();
        //_anim = GetComponent<Animator>();
    }

    public void SetCreature(Vector3 where){
        SetStats(manager.difficulty);
        transform.position = where;
        Initialize();
    }

    void SetStats(float difficulty){
        float totalStats = 100 * difficulty;
        brains = Random.Range(0,70);
        totalStats-=brains;
        speed = Random.Range(10,totalStats);
        totalStats-=speed;
        strength = Random.Range(10,totalStats);

        // mySpeed = myStats.speed/10;
        // myStrength = myStats.strength/10;

        // patrolTimer = (100 - myStats.speed)/5;
        // attackTimer = (100 - myStats.speed)/10;
        // chaseTimer = (myStats.brains)/8;
        // float mySize = Mathf.Clamp(myStrength/2,0.1f,10);
        // transform.localScale = new Vector3(mySize,mySize*2,mySize);
        // myStats.health = 100 * difficulty * mySize;
        // myFOV.SetFOV(myStats.brains);
    }

    void Initialize(){
        if (enemy)
            _state = State.patrol;
    }

    void Update(){

    }
}
