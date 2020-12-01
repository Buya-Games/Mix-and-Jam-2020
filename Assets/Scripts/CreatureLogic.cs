using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CreatureLogic : MonoBehaviour
{
    Manager _manager;
    public string MyName;
    public float Age, Speed, MaxHealth, Health, Strength, Range, Tech, Heal;
    public float _partyRange;//distance to player... if beyond this, must abandon attack and follow player
    float _attackRange, _chaseTimer, _chaseCounter, _attackTimer, _attackCounter, _scanArea;
    public bool _alive, _enemy, _party, _player;
    public enum State {attack, patrol, chasing};
    //Animator _anim;
    public Transform _playerTarget, _attackTarget;
    [SerializeField] Vector3 _patrolTarget;
    //[SerializeField] Vector3 _myOffset;
    [HideInInspector] public Vector3 MyPosition;
    State _state;
    Color _origColor;
    [SerializeField] LayerMask _myLM;
    [SerializeField] Transform _visibleMesh;
    public GameObject _myUI;
    Transform _myHealthBar;
    TMP_Text _textName;
    Image _displayHealth, _healthBar;
    [HideInInspector] public float[] Face;
    [SerializeField] Color hairColor;
    [SerializeField] float vel;
    
    

    void Awake(){
        _manager = FindObjectOfType<Manager>();
        //_anim = GetComponent<Animator>();
    }

    public void SetCreature(Vector3 where, bool enemy = false, bool child = false){
        _enemy = enemy;
        _party = false;
        transform.position = where;
        if (!child){
            SetStats();
        }
    }

    public void SetChild(CreatureLogic faja, CreatureLogic maja){
        Strength = Mathf.Max(faja.Strength, maja.Strength) * Random.Range(.8f,1.2f);
        Speed = Mathf.Max(faja.Speed, maja.Speed) * Random.Range(.8f,1.2f);
        Range = Mathf.Max(faja.Range, maja.Range) * Random.Range(.8f,1.2f);
        Tech = Mathf.Max(faja.Tech, maja.Tech) * Random.Range(.8f,1.2f);
        Heal = Mathf.Max(faja.Heal, maja.Heal) * Random.Range(.8f,1.2f);
        SetStats(true);
        SetPlayer();
    }

    public void SetPartner(){
        _enemy = false;
        _player = false;
        SetPlayer();
    }

    void SetStats(bool child = false){
        if (!child){
            float totalStats = 15;
            Strength = Random.Range(1f,12f);
            totalStats-=Strength;
            Speed = Random.Range(1f,totalStats);
            totalStats-=Speed;
            Range = Random.Range(1f,totalStats) * 2;
            Tech = Random.Range(1f,totalStats);
            Heal = Random.Range(1f,totalStats);
        }
        
        MaxHealth = Strength * 3;
        Health = MaxHealth;
        Age = 20;

        _attackRange = transform.localScale.y;
        _chaseTimer = Range * 2;// just a random number for now
        _attackTimer = 10/Speed;// just a random number for now
        _partyRange = 15;// just a random number for now

        _visibleMesh = transform.Find("VisibleMesh");

        if (_enemy){ //if enemy
            gameObject.layer = 12;
            _origColor = new Color(0,0.549f,0.204f,0);
            _myLM = _manager.enemyLM;
        } else { //if friendly
            _origColor = new Color(1,0.729f,0.514f,0);
            _myLM = _manager.friendlyLM;
            MyName = _manager.FaceGenerator.GenerateName();
            CreateFace();
            if (!_player){ //if friendly NPC
                gameObject.layer = 9;
            } else { //if player
                gameObject.layer = 8;
                _manager.ui.AddFaceToGrid(Face, this);
            }
            //SetUI();
        }
        GetComponentInChildren<MeshRenderer>().material.color = _origColor;
        SetHealthBar();
        _alive = true;
        this.gameObject.name = MyName;
        StartCoroutine(CheckForEnemies());
        // float mySize = Mathf.Clamp(_strength/2,0.1f,3);
        // transform.localScale = new Vector3(mySize,mySize*2,mySize);
        // myFOV.SetFOV(myStats.brains);
    }

    void SetPlayer(){ //creates a random offset away from the main player for this character
        _party = true;
        _playerTarget = _manager.player.transform;
        _manager.ui.AddFaceToGrid(Face, this);

        // if (_manager.move.offsetPositions.Count > 0){
        //     _myOffset = _manager.move.offsetPositions.Dequeue();
        //     _manager.move.offsetPositions.Enqueue(_myOffset * 2);
        // } else {
            // float _randoDist = Random.Range(1,3f);
            // _myOffset = new Vector3(_randoDist * (Random.Range(0,2)-1),0,_randoDist * (Random.Range(0,2)-1));
        //}
        
    }

    public void SetPosition(Vector3 pos){
        MyPosition = pos;
    }

    void CreateFace(){
        Face = _manager.FaceGenerator.GenerateRandomFace();
        _visibleMesh.Find("Head").GetComponent<MeshRenderer>().material.color = _manager.FaceGenerator.GetHairColor(Face[6]);
        hairColor = _manager.FaceGenerator.GetHairColor(Face[6]);
    }

    void SetUI(){
        _myUI = _manager.ui.partyUIQueue.Dequeue();
        _myUI.SetActive(true);
        _displayHealth = _myUI.transform.Find("Member Button").GetComponent<Image>();
        _textName = _displayHealth.transform.Find("Member Name").GetComponent<TMP_Text>();
        _textName.text = gameObject.name;
    }

    void SetHealthBar(){
        _myHealthBar = _manager.ui.CreateMyHealthBar(transform);
        _healthBar = _myHealthBar.Find("Fillbar").GetComponent<Image>();
    }

    public void NewHealth(float amount){ //updates UI for party members
        Health+=amount;
        _healthBar.fillAmount = (Health/MaxHealth);
        //_displayHealth.fillAmount = (_health/_maxHealth);
    }

    void Update(){
        if (_alive){
            if (Health <= 0){
                _alive = false;
                Death();
            }
            Bobbing();

            if (_party){//if party member
                float distToPlayer = Mathf.Abs(Vector3.Distance(transform.position, _playerTarget.position));
                if (_attackTarget != null && distToPlayer < _partyRange){
                    float dist = Mathf.Abs(Vector3.Distance(transform.position, _attackTarget.position));
                    if (dist < _attackRange){
                        Attack();
                    } else {
                        MoveToTarget(_attackTarget.position);
                    }
                } else {
                    MoveToTarget(_playerTarget.position + MyPosition);
                }
            } else if (_enemy){ //if enemy
                if (_attackTarget != null){ // if you have an attack target
                    float dist = Mathf.Abs(Vector3.Distance(transform.position, _attackTarget.position));
                    if (dist < _attackRange){ //and are within attack range
                        Attack();
                    } else{
                        MoveToTarget(_attackTarget.position);
                    }
                } else {
                    PatrolToTarget(); //move to some random place (aka PATROL)
                }
            } else if (_player){
                if (_attackTarget != null){
                    float dist = Mathf.Abs(Vector3.Distance(transform.position, _attackTarget.position));
                    if (dist < _attackRange){
                        Attack();
                    }
                }
            }
        }
    }

    IEnumerator CheckForEnemies(){
        while (_alive){
            Collider[] allHits = Physics.OverlapSphere(transform.position,Range,_myLM,QueryTriggerInteraction.Collide);
            if (allHits.Length > 0){
                float closest = 1000;
                for (int i = 0;i<allHits.Length;i++){
                    float dist = Mathf.Abs(Vector3.Distance(transform.position,allHits[i].transform.position));
                    if (dist < closest){
                        closest = dist;
                        _attackTarget = allHits[i].transform;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
        
    }

    void Bobbing(){
        _visibleMesh.position = _visibleMesh.position + transform.up * Mathf.Sin(Time.time * 3 * Speed) * 0.015f;
    }

    Vector3 RandomLocation(){
        Vector3 randoDir = new Vector3(
            Random.Range(-1,1f) * Random.Range(1,10f),
            0,
            Random.Range(-1,1f) * Random.Range(1,10f));
        Vector3 newPos = transform.position + randoDir;
        return newPos;
    }
    

    void MoveToTarget(Vector3 target){
        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        _chaseCounter+=Time.deltaTime;
        if (_chaseCounter > _chaseTimer){
            _chaseCounter = 0;
            _attackTarget = null;
        }
    }

    void PatrolToTarget(){
        transform.position = Vector3.MoveTowards(transform.position, _patrolTarget, Speed * Time.deltaTime);
        _chaseCounter+=Time.deltaTime;
        if (_chaseCounter > _chaseTimer){
            _chaseCounter = 0;
            _patrolTarget = RandomLocation();
        }
    }

    void Attack(){
        _attackCounter+=Time.deltaTime;
        if (_attackCounter > _attackTimer){
            _attackCounter = 0;

            CreatureLogic targetLogic = _attackTarget.GetComponent<CreatureLogic>();
            if (targetLogic.Health <= 0 || _attackTarget.gameObject.activeSelf == false){ //if target ain't dead
                _attackTarget = null; //he's dead jim!
            } else if (gameObject.activeSelf == true){//if im alive
                float rando = Random.Range(.8f,1.2f);
                float hitPoint = Strength * rando;
                //if (_enemy){
                    targetLogic.NewHealth(-hitPoint);
                // } else {
                //     targetLogic._health-=hitPoint;
                // }
                _manager.ui.HitGUI(_attackTarget.position, hitPoint, Color.red);
                _manager.particles.FriendlyHit(_attackTarget.position);
                StartCoroutine(AttackLunge());
                StartCoroutine(HitDisplay(_attackTarget.gameObject, targetLogic._origColor));
            }
        }
    }

    IEnumerator HitDisplay(GameObject who, Color origColor){
        MeshRenderer rendy = who.GetComponentInChildren<MeshRenderer>();
        rendy.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        rendy.material.color = origColor;
    }

    IEnumerator HitTarget(Rigidbody who){
        Vector3 dir = (who.transform.position - transform.position).normalized; // hit them away from me
        dir.y = 0;
        float hitForce = _manager.hitForce / 15;
        while (hitForce > 0){
            who.velocity = (dir * _manager.hitForce); //replace this with my strength
            hitForce -= Time.deltaTime;
            yield return null;
        } 
    }

    IEnumerator AttackLunge(){
        Vector3 origPos = transform.position;
        Vector3 attackPos = _attackTarget.position;

        float attackSpeed = 3;
        float percent = 0;

        while (percent <= 1){
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
            transform.position = Vector3.Lerp(origPos,attackPos,interpolation);

            yield return null;
        }
        //StartCoroutine(HitTarget(_attackTarget.GetComponent<Rigidbody>()));
    }

    void Death(){
        if (_enemy){
            int howMany = Mathf.CeilToInt(Strength/10);
            for (int i = 0;i<howMany;i++){
                _manager.spawner.SpawnItem(transform.position);
                _manager.particles.PlayItem(transform.position);
            }
        }

        //Debug.Log(name + " has died");
        _alive = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.AddForce(Vector3.up * 10);
        _myHealthBar.gameObject.SetActive(false);
        gameObject.layer = 15;

        //this.gameObject.SetActive(false);
    }

        //!player
            //friendly?
                //idle
                //if enemy nearby && within party range
                    //chase
                        //if within attack range (based on class)
                            //attack
                //else
                    //follow player
            //else enemy
                //patrol
                //chase
                    //if within attack range (depends on your class)
                        //attack

}
