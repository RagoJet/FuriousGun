using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour{
    private Rigidbody _rb;
    private Vector3 _startLocalPos;
    private Weapon _weapon;
    [SerializeField] private int speed;
    private int _damage;
    int _layerMask;
    private bool _isActive;
    public bool dangerousTrigger;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioClip explosionClip;

    private void Awake(){
        _layerMask = 1 << LayerMask.NameToLayer("Enemy") | (1 << LayerMask.NameToLayer("Player"));
        _weapon = GetComponentInParent<Weapon>();
        _rb = GetComponent<Rigidbody>();
        _startLocalPos = transform.localPosition;
    }

    public void Init(Vector3 direction){
        if (dangerousTrigger){
            Explosion();
        }
        else{
            _rb.AddForce(direction * speed);
            _isActive = true;
        }
    }

    private void OnTriggerEnter(Collider other){
        if (!other.CompareTag("Player")){
            dangerousTrigger = true;
        }


        if (!_isActive){
            return;
        }

        particleSystem.transform.up = -transform.forward;
        Explosion();
    }

    private void OnTriggerExit(Collider other){
        dangerousTrigger = false;
    }

    private void Explosion(){
        AudioPlayer.Instance.PlayClip(explosionClip);
        particleSystem.transform.position = transform.position;
        particleSystem.Play();
        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, new Vector3(2.7f, 1.9f, 2.7f),
            Quaternion.identity, _layerMask);
        foreach (var collider in colliders){
            if (collider.TryGetComponent(out Player player)){
                player.TakeDamage(_damage);
            }

            if (collider.CompareTag("Body") || collider.CompareTag("Head")){
                collider.GetComponentInParent<Enemy>().TakeDamage(_damage, 1);
            }
        }

        Preparation();
    }

    public void Construct(int damage){
        _damage = damage;
    }

    public void Preparation(){
        _rb.Sleep();
        transform.localScale = Vector3.zero;
        transform.parent = _weapon.transform;
        transform.forward = _weapon.transform.forward;
        transform.localPosition = _startLocalPos;
        transform.DOScale(Vector3.one, 0.2f);
        _isActive = false;
    }
}