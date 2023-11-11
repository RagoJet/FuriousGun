using UnityEngine;
using UnityEngine.AI;

enum EnemyState{
    Death,
    Moving,
    Attacking
}

public class Enemy : MonoBehaviour{
    private EnemyState _state;
    private EnemyDescription _enemyDescription;
    private NavMeshAgent _agent;
    private PlayerController _target;

    private float _timeFromLastCheck;

    private Animator _animator;
    [SerializeField] bool armed;
    private int _attackHashAnim;
    private int _moveHashAnim;
    private static readonly int _headDeathAnim = Animator.StringToHash("HeadDeath");
    private static readonly int _simpleDeathAnim = Animator.StringToHash("SimpleDeath");

    private float timeFromLastAttack;

    private void Awake(){
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _attackHashAnim = armed ? Animator.StringToHash("WeaponAttack") : Animator.StringToHash("BoxAttack");
        _moveHashAnim = _agent.speed < 6 ? Animator.StringToHash("Walk") : Animator.StringToHash("Run");
    }


    public void Init(EnemyDescription enemyDescription, PlayerController playerController){
        _enemyDescription = enemyDescription;
        _target = playerController;
        _state = EnemyState.Moving;
        _animator.SetTrigger(_moveHashAnim);
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
        Vector3 direction = _target.transform.position - transform.position;
        direction.y = 0;
        float distance = Vector3.SqrMagnitude(direction);
        if (distance < _agent.stoppingDistance * _agent.stoppingDistance){
            if (Time.time - timeFromLastAttack > 1.5f){
                timeFromLastAttack = Time.time;
                Debug.Log("attacked");
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
}