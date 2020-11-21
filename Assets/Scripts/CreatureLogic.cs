using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureLogic : MonoBehaviour
{
    Manager _manager;
    public string _myName;
    public float _age, _speed, _maxHealth, _strength, _brains, _charm, _heal, _health;
    float _partyRange;//distance to player... if beyond this, must abandon attack and follow player
    float _attackRange, _chaseTimer, _attackTimer, _counter, _scanArea;
    public bool _alive, _enemy, _isPlayer;
    public enum State {attack, patrol, chasing};
    //Animator _anim;
    public Transform _playerTarget, _attackTarget;
    [SerializeField] Vector3 _patrolTarget;
    [SerializeField] Vector3 _myOffset;
    State _state;
    Color _origColor;
    [SerializeField] LayerMask _myLM;
    [SerializeField] Transform _visibleMesh;

    void Awake(){
        _manager = FindObjectOfType<Manager>();
        //_anim = GetComponent<Animator>();
    }

    public void SetCreature(Vector3 where, bool enemy = false){
        _enemy = enemy;
        transform.position = where;
        SetStats();
        _visibleMesh = transform.Find("VisibleMesh");
    }

    void SetStats(){
        float totalStats = 15;
        _strength = Random.Range(1f,12f);
        totalStats-=_strength;
        _speed = Random.Range(1f,totalStats);
        totalStats-=_speed;
        _brains = Random.Range(1f,totalStats);
        //totalStats-=_brains;
        _charm = Random.Range(1f,totalStats);
        //totalStats-=_charm;
        _heal = Random.Range(1f,totalStats);

        _brains *= 5;
        _maxHealth = _strength * 3;
        _health = _maxHealth;
        _age = 20;

        _attackRange = transform.localScale.y;
        _chaseTimer = _brains/2;
        _attackTimer = _speed/5;
        _partyRange = 10;// just a random number for now

        if (_enemy){ //if enemy
            gameObject.layer = 12;
            _origColor = new Color(0,0.549f,0.204f,0);
            _myLM = _manager.enemyLM;
        } else { //if friendly
            _origColor = new Color(1,0.729f,0.514f,0);
            _myLM = _manager.friendlyLM;
            if (!_isPlayer){ //if friendly NPC
                gameObject.layer = 9;
                SetPlayer();
            } else { //if player
                gameObject.layer = 8;
            }
        }
        GetComponentInChildren<MeshRenderer>().material.color = _origColor;
        _alive = true;

        // float mySize = Mathf.Clamp(_strength/2,0.1f,3);
        // transform.localScale = new Vector3(mySize,mySize*2,mySize);
        // myFOV.SetFOV(myStats.brains);
    }

    public void SetPlayer(){
        _playerTarget = _manager.player.transform;

        if (_manager.move.offsetPositions.Count > 0){
            _myOffset = _manager.move.offsetPositions.Dequeue();
            _manager.move.offsetPositions.Enqueue(_myOffset * 2);
        } else {
            float _randoDist = Random.Range(1,3f);
            _myOffset = new Vector3(_randoDist * (Random.Range(0,2)-1),0,_randoDist * (Random.Range(0,2)-1));
        }
    }

    void Update(){
        if (!_isPlayer && _alive){
            if (_health <= 0){
                _alive = false;
                Death();
            }
            CheckForEnemies();

            if (!_enemy){
                if (_attackTarget != null){
                    float dist = Mathf.Abs(Vector3.Distance(transform.position, _attackTarget.position));
                    if (dist < _partyRange && dist < _attackRange){
                        Attack();
                    } else {
                        MoveToTarget(_attackTarget.position);
                    }
                } else {
                    MoveToTarget(_playerTarget.position + _myOffset);
                }
            } else { //if enemy
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
            }
        }
        if (_isPlayer){
            CheckForEnemies();
            if (_attackTarget != null){
                float dist = Mathf.Abs(Vector3.Distance(transform.position, _attackTarget.position));
                if (dist < _attackRange){
                    Attack();
                }
            }
        }
        if (_alive){
            Bobbing();
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
        _counter+=Time.deltaTime;
        if (_counter > _chaseTimer){
            _counter = 0;
            _attackTarget = null;
        }
    }

    void PatrolToTarget(){
        transform.position = Vector3.MoveTowards(transform.position, _patrolTarget, _speed * Time.deltaTime);
        _counter+=Time.deltaTime;
        if (_counter > _chaseTimer){
            _counter = 0;
            _patrolTarget = RandomLocation();
        }
    }

    void Attack(){
        _counter+=Time.deltaTime;
        if (_counter > _attackTimer){
            _counter = 0;

            CreatureLogic targetLogic = _attackTarget.GetComponent<CreatureLogic>();
            if (targetLogic._health <= 0 || _attackTarget.gameObject.activeSelf == false){ //if target ain't dead
                _attackTarget = null; //he's dead jim!
            } else if (gameObject.activeSelf == true){//if im alive
                float rando = Random.Range(.8f,1.2f);
                float hitPoint = _strength * rando;
                targetLogic._health -= hitPoint;

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
        Debug.Log(name + " has died");
        this.gameObject.SetActive(false);
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
