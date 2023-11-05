using DG.Tweening;
using UnityEngine;

public class Weapon : MonoBehaviour{
    [SerializeField] private ParticleSystem attackFX;
    [SerializeField] private int damage;
    [SerializeField] private int price;
    public int Price => price;
    public bool available;
    private bool _isInHand;

    private Vector3 _hidePosition;
    private Vector3 _readyPosition;
    private Tween _tweenWeapon;

    [SerializeField] private float delayShotTime = 0.1f;
    private float _timeFromLastShot;

    [SerializeField] private Vector3 shakeVectorStrength = Vector3.one;

    private AudioPlayer _audioPlayer;
    [SerializeField] private AudioClip shotClip;

    private void Awake(){
        _readyPosition = transform.localPosition;
        _hidePosition = new Vector3(_readyPosition.x, _readyPosition.y - 1, _readyPosition.z);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1f,
            transform.localPosition.z);
    }

    private void Start(){
        _audioPlayer = AudioPlayer.Instance;
    }


    public void ShowSelf(){
        _timeFromLastShot = delayShotTime;
        _tweenWeapon.Kill();
        gameObject.SetActive(true);
        _tweenWeapon = transform.DOLocalMove(_readyPosition, 0.3f).OnComplete(() => _isInHand = true);
    }

    public void HideSelf(){
        _isInHand = false;
        _tweenWeapon.Kill();
        _tweenWeapon = transform.DOLocalMove(_hidePosition, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }

    private void Update(){
        _timeFromLastShot += Time.deltaTime;
        if (_isInHand){
            if (Input.GetMouseButton(0)){
                if (_timeFromLastShot >= delayShotTime){
                    attackFX.Play();
                    _timeFromLastShot = 0;
                    _audioPlayer.PlayClip(shotClip);
                    _tweenWeapon = transform.DOShakePosition(delayShotTime * 0.8f, shakeVectorStrength);
                }
            }
        }
    }
}