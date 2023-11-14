using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState{
    Death,
    Moving,
    Attacking
}

public class Enemy : MonoBehaviour{
    public event Action<Enemy> Ondie;
    private int _currentHP;
    private EnemyState _state;
    private EnemyDescription _enemyDescription;
    private NavMeshAgent _agent;
    private Player _target;

    private float _timeFromLastCheck;

    private Animator _animator;
    [SerializeField] bool armed;
    private int _attackHashAnim;
    private int _moveHashAnim;
    private static readonly int _headDeathAnim = Animator.StringToHash("HeadDeath");
    private static readonly int _simpleDeathAnim = Animator.StringToHash("SimpleDeath");

    private float timeFromLastAttack;

    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip deathClip;

    private void Awake(){
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _attackHashAnim = armed ? Animator.StringToHash("WeaponAttack") : Animator.StringToHash("BoxAttack");
        _moveHashAnim = _agent.speed < 6 ? Animator.StringToHash("Walk") : Animator.StringToHash("Run");
    }


    public void Init(EnemyDescription enemyDescription, Player playerController){
        _enemyDescription = enemyDescription;
        _target = playerController;
        _state = EnemyState.Moving;
        _animator.SetTrigger(_moveHashAnim);
        _agent.isStopped = false;
        _currentHP = _enemyDescription.maxHealth;
    }

    private void Update(){
        switch (_state){
            case EnemyState.Moving:
                if (Time.time - _timeFromLastCheck > 0.1f){
                    Vector3 posTarget = _target.transform.position;
                    _agent.SetDestination(posTarget);
                    _timeFromLastCheck = Time.time;

                    float distance = Vector3.Distance(transform.position, posTarget);

                    if (distance <= _agent.stoppingDistance){
                        _agent.isStopped = true;
                        _state = EnemyState.Attacking;
                        _animator.SetTrigger(_attackHashAnim);
                    }
                }

                break;
            case EnemyState.Attacking:
                Vector3 direction = _target.transform.position - transform.position;
                direction.y = 0;
                if (direction != Vector3.zero){
                    transform.forward = direction;
                }

                break;
        }
    }

    public void DealDamage(){
        AudioPlayer.Instance.PlayClip(attackClip);
        Vector3 direction = _target.transform.position - transform.position;
        direction.y = 0;
        float distance = Vector3.SqrMagnitude(direction);
        if (distance < _agent.stoppingDistance * _agent.stoppingDistance){
            if (Time.time - timeFromLastAttack > 1.5f){
                timeFromLastAttack = Time.time;
                _target.TakeDamage(_enemyDescription.damage);
            }
        }

        else{
            _agent.isStopped = false;
            _state = EnemyState.Moving;
            _animator.SetTrigger(_moveHashAnim);
        }
    }

    public int GetLevel(){
        return _enemyDescription.level;
    }

    public void TakeDamage(int damage, int multiple){
        if (_state == EnemyState.Death) return;

        _currentHP -= damage * multiple;
        if (_currentHP <= 0){
            AudioPlayer.Instance.PlayClip(deathClip);
            _state = EnemyState.Death;
            _agent.isStopped = true;
            if (multiple == 1){
                _animator.SetTrigger(_simpleDeathAnim);
            }
            else{
                _animator.SetTrigger(_headDeathAnim);
            }

            StartCoroutine(DeathLifeCycle());
        }
    }

    IEnumerator DeathLifeCycle(){
        yield return new WaitForSeconds(1.7f);
        Ondie.Invoke(this);
    }
}