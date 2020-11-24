using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CreatureLogic : MonoBehaviour
{
    Manager _manager;
    public string _myName;
    public float _age, _speed, _maxHealth, _strength, _brains, _charm, _heal, _health;
    public float _partyRange;//distance to player... if beyond this, must abandon attack and follow player
    float _attackRange, _chaseTimer, _chaseCounter, _attackTimer, _attackCounter, _scanArea;
    public bool _alive, _enemy, _party, _player;
    public enum State {attack, patrol, chasing};
    //Animator _anim;
    public Transform _playerTarget, _attackTarget;
    [SerializeField] Vector3 _patrolTarget;
    [SerializeField] Vector3 _myOffset;
    State _state;
    Color _origColor;
    [SerializeField] LayerMask _myLM;
    [SerializeField] Transform _visibleMesh;
    public GameObject _myUI;
    Transform _myHealthBar;
    TMP_Text _textName;
    Image _displayHealth, _healthBar;
    
    

    void Awake(){
        _manager = FindObjectOfType<Manager>();
        //_anim = GetComponent<Animator>();
    }

    public void SetCreature(Vector3 where, bool enemy = false, bool child = false, bool partyMember = false){
        _enemy = enemy;
        _party = partyMember;
        transform.position = where;
        if (!child){
            SetStats();
        }
        _visibleMesh = transform.Find("VisibleMesh");
    }

    public void SetChild(CreatureLogic faja, CreatureLogic maja){
        _strength = Mathf.Max(faja._strength, maja._strength) * Random.Range(.8f,1.2f);
        _speed = Mathf.Max(faja._speed, maja._speed) * Random.Range(.8f,1.2f);
        _brains = Mathf.Max(faja._brains, maja._brains) * Random.Range(.8f,1.2f);
        _charm = Mathf.Max(faja._charm, maja._charm) * Random.Range(.8f,1.2f);
        _heal = Mathf.Max(faja._heal, maja._heal) * Random.Range(.8f,1.2f);
        SetStats(true);
        _party = true;
    }

    void SetStats(bool child = false){
        if (!child){
            float totalStats = 15;
            _strength = Random.Range(1f,12f);
            totalStats-=_strength;
            _speed = Random.Range(1f,totalStats);
            totalStats-=_speed;
            _brains = Random.Range(1f,totalStats);
            _charm = Random.Range(1f,totalStats);
            _heal = Random.Range(1f,totalStats);
        }
        
        _maxHealth = _strength * 3;
        _health = _maxHealth;
        _age = 20;

        _attackRange = transform.localScale.y;
        _chaseTimer = _brains * 2;// just a random number for now
        _attackTimer = 10/_speed;// just a random number for now
        _partyRange = 15;// just a random number for now

        if (_enemy){ //if enemy
            gameObject.layer = 12;
            _origColor = new Color(0,0.549f,0.204f,0);
            _myLM = _manager.enemyLM;
        } else { //if friendly
            _origColor = new Color(1,0.729f,0.514f,0);
            _myLM = _manager.friendlyLM;
            if (!_player){ //if friendly NPC
                gameObject.layer = 9;
                SetPlayer();
            } else { //if player
                gameObject.layer = 8;
            }
            //SetUI();
        }
        SetHealthBar();
        GetComponentInChildren<MeshRenderer>().material.color = _origColor;
        _alive = true;

        // float mySize = Mathf.Clamp(_strength/2,0.1f,3);
        // transform.localScale = new Vector3(mySize,mySize*2,mySize);
        // myFOV.SetFOV(myStats.brains);
    }

    void SetPlayer(){ //creates a random offset away from the main player for this character
        _playerTarget = _manager.player.transform;

        if (_manager.move.offsetPositions.Count > 0){
            _myOffset = _manager.move.offsetPositions.Dequeue();
            _manager.move.offsetPositions.Enqueue(_myOffset * 2);
        } else {
            float _randoDist = Random.Range(1,3f);
            _myOffset = new Vector3(_randoDist * (Random.Range(0,2)-1),0,_randoDist * (Random.Range(0,2)-1));
        }
        
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
        _health+=amount;
        _healthBar.fillAmount = (_health/_maxHealth);
        //_displayHealth.fillAmount = (_health/_maxHealth);
    }

    void Update(){
        if (_alive){
            if (_health <= 0){
                _alive = false;
                Death();
            }
            CheckForEnemies();
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
                    MoveToTarget(_playerTarget.position + _myOffset);
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

    void CheckForEnemies(){
        Collider[] allHits = Physics.OverlapSphere(transform.position,_brains,_myLM,QueryTriggerInteraction.Ignore);
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
    }

    void Bobbing(){
        _visibleMesh.position = _visibleMesh.position + transform.up * Mathf.Sin(Time.time * 3 * _speed) * 0.015f;
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
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        _chaseCounter+=Time.deltaTime;
        if (_chaseCounter > _chaseTimer){
            _chaseCounter = 0;
            _attackTarget = null;
        }
    }

    void PatrolToTarget(){
        transform.position = Vector3.MoveTowards(transform.position, _patrolTarget, _speed * Time.deltaTime);
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
            if (targetLogic._health <= 0 || _attackTarget.gameObject.activeSelf == false){ //if target ain't dead
                _attackTarget = null; //he's dead jim!
            } else if (gameObject.activeSelf == true){//if im alive
                float rando = Random.Range(.8f,1.2f);
                float hitPoint = _strength * rando;
                //if (_enemy){
                    targetLogic.NewHealth(-hitPoint);
                // } else {
                //     targetLogic._health-=hitPoint;
                // }
                _manager.ui.HitGUI(_attackTarget.position, hitPoint, Color.red);
                _manager.particles.FriendlyHit(_attackTarget.position);
                StartCoroutine(HitDisplay(_attackTarget.gameObject, targetLogic._origColor));
                HitTarget(_attackTarget.GetComponent<Rigidbody>());
            }
        }
    }

    IEnumerator HitDisplay(GameObject who, Color origColor){
        MeshRenderer rendy = who.GetComponentInChildren<MeshRenderer>();
        rendy.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        rendy.material.color = origColor;
    }

    void HitTarget(Rigidbody who){
        Vector3 dir = (who.transform.position - transform.position).normalized; // hit them away from me
        dir.y = 0;
        who.velocity = (dir * _manager.hitForce); //replace this with my strength
    }

    void Death(){
        if (_enemy){
            int howMany = Mathf.CeilToInt(_strength/10);
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
