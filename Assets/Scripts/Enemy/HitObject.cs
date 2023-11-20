using UnityEngine;

public class HitObject : MonoBehaviour{
    private ParticleSystem _particleSystem;
    private Rigidbody _rigidbody;
    [SerializeField] private int speed;
    [SerializeField] private int damage;


    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();
    }


    public void Init(Vector3 direction){
        _rigidbody.AddForce(direction * speed);
        _particleSystem.Play();
    }

    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent(out Player player)){
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment")){
            Destroy(gameObject);
        }
    }
}