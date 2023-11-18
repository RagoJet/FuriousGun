using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
    private PlayerController _playerController;
    private CameraController _cameraController;
    private Inventory _inventory;
    private int _currentHealth;
    private int _maxHealth = 100;
    private bool _isAlive;
    [SerializeField] private AudioClip[] woundClips;

    [SerializeField] TextMeshProUGUI currentHpText;
    private Color _colorOfText;
    [SerializeField] Volume volume;
    private ColorAdjustments _CA;

    private Sequence _sequenceCurrentHpText;

    private void Awake(){
        _colorOfText = currentHpText.color;
        _playerController = GetComponent<PlayerController>();
        _inventory = GetComponent<Inventory>();
        _cameraController = GetComponentInChildren<CameraController>();
        Refresh();
        volume.profile.TryGet<ColorAdjustments>(out _CA);
    }

    public void Refresh(){
        _isAlive = true;
        _currentHealth = _maxHealth;
        UpdateUIHealth();
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
            SequenceOfTextCurrentHp(damage);
            AudioPlayer.Instance.PlayClip(woundClips[Random.Range(0, woundClips.Length)]);
            _currentHealth -= damage;
            if (_currentHealth <= 0){
                _currentHealth = 0;
                _isAlive = false;
                GetComponentInChildren<Weapon>().HideSelf();
                _cameraController.SeeToTheSky();
                AudioPlayer.Instance.PlayGameOverSound();

                StartCoroutine(DeadVisibleMode());
            }

            UpdateUIHealth();
        }
    }

    private void UpdateUIHealth(){
        currentHpText.text = _currentHealth + @"<sprite=""HealthIcon"" name=""HealthIcon"">";
        ;
    }

    IEnumerator DeadVisibleMode(){
        while (_CA.saturation.value > -100f){
            _CA.saturation.value = _CA.saturation.value - 0.5f;
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SequenceOfTextCurrentHp(int strength){
        if (_sequenceCurrentHpText.IsActive()){
            return;
        }


        _sequenceCurrentHpText = DOTween.Sequence();
        currentHpText.color = Color.red;
        float duration = strength * 0.1f;
        strength *= 3;

        _sequenceCurrentHpText.Append(currentHpText.rectTransform.DOShakePosition(duration, strength));
        _sequenceCurrentHpText.Append(currentHpText.DOColor(_colorOfText, duration));
    }
}