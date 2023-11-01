using DG.Tweening;
using UnityEngine;

public class Weapon : MonoBehaviour{
    [SerializeField] private ParticleSystem attackFX;

    [SerializeField] private int damage;
    [SerializeField] private int price;
    public int Price => price;
    public bool available;
    private bool _isInHand;

    private Vector3 _readyPosition;
    private Tween _tween;

    [SerializeField] private float delayShotTime = 0.1f;
    private float _timeFromLastShot;

    private void Awake(){
        _readyPosition = transform.localPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1f,
            transform.localPosition.z);
    }

    public void ShowSelf(){
        _tween.Kill();
        gameObject.SetActive(true);
        _tween = transform.DOLocalMoveY(_readyPosition.y, 0.4f).OnComplete(() => _isInHand = true);
    }

    public void HideSelf(){
        _isInHand = false;
        _tween.Kill();
        _tween = transform.DOLocalMoveY(_readyPosition.y - 1f, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }

    private void Update(){
        _timeFromLastShot += Time.deltaTime;
        if (_isInHand){
            if (Input.GetMouseButton(0)){
                if (_timeFromLastShot >= delayShotTime){
                    attackFX.Play();
                    _timeFromLastShot = 0;
                }
            }
        }
    }
}