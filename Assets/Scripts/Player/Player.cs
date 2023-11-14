using UnityEngine;

public class Player : MonoBehaviour{
    private PlayerController _playerController;
    private CameraController _cameraController;
    private Inventory _inventory;
    private int _currentHealth;
    private int _maxHealth = 50;
    private bool _isAlive;
    [SerializeField] private AudioClip[] woundClips;

    private void Awake(){
        _playerController = GetComponent<PlayerController>();
        _inventory = GetComponent<Inventory>();
        _cameraController = GetComponentInChildren<CameraController>();

        _isAlive = true;
        _currentHealth = _maxHealth;
    }

    private void Update(){
        if (_isAlive){
            _playerController.Updater();
            _cameraController.Updater();
            _inventory.Updater();
        }
    }

    public void TakeDamage(int damage){
        if (_currentHealth > 0){
            AudioPlayer.Instance.PlayClip(woundClips[Random.Range(0, woundClips.Length)]);
            _currentHealth -= damage;
            if (_currentHealth <= 0){
                _isAlive = false;
                GetComponentInChildren<Weapon>().HideSelf();
                _cameraController.SeeToTheSky();
                AudioPlayer.Instance.PlayStartLevel();
            }
        }
    }
}