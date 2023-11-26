using DG.Tweening;
using UnityEngine;
using YG;

enum TypeOfWeapon{
    Projectile,
    RayCast
}

public class Weapon : MonoBehaviour{
    private Inventory inventory;
    public int countOfBullets;
    public int countOfAddBullets;
    [SerializeField] private float distanceShot = 100f;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ParticleSystem attackFX;
    [SerializeField] private int damage;
    [SerializeField] private int price;

    [SerializeField] private Bullet bullet;

    [SerializeField] private ParticleSystem headHitFX;
    [SerializeField] private ParticleSystem bodyHitFX;
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

    [SerializeField] private TypeOfWeapon _typeOfWeapon;
    int _layerMask;

    private void Awake(){
        if (bullet != null){
            bullet.Construct(damage);
        }

        _readyPosition = transform.localPosition;
        _hidePosition = new Vector3(_readyPosition.x, _readyPosition.y - 1, _readyPosition.z);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1f,
            transform.localPosition.z);
    }

    private void Start(){
        inventory = Inventory.Instance;
        _audioPlayer = AudioPlayer.Instance;
        _layerMask = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Environment"));
    }


    public void ShowSelf(){
        _timeFromLastShot = delayShotTime;
        _tweenWeapon.Kill();
        gameObject.SetActive(true);
        _tweenWeapon = transform.DOLocalMove(_readyPosition, 0.3f).OnComplete(() => _isInHand = true);
        if (bullet != null){
            bullet.dangerousTrigger = false;
        }
    }

    public void HideSelf(){
        _isInHand = false;
        _tweenWeapon.Kill();
        _tweenWeapon = transform.DOLocalMove(_hidePosition, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }

    private void Update(){
        _timeFromLastShot += Time.deltaTime;
        if (_isInHand){
            if (Input.GetMouseButtonDown(0)){
                if (countOfBullets <= 0){
                    _audioPlayer.PlayNoAmmoSound();
                }
            }

            if (Input.GetMouseButton(0)){
                if (_timeFromLastShot >= delayShotTime && countOfBullets > 0){
                    Shot();
                }
            }
        }
    }

    private void Shot(){
        attackFX.Play();
        _timeFromLastShot = 0;
        _audioPlayer.PlayClip(shotClip);
        _tweenWeapon = transform.DOShakePosition(delayShotTime * 0.8f, shakeVectorStrength);

        switch (_typeOfWeapon){
            case TypeOfWeapon.Projectile:
                ProjectileAttack();
                break;
            case TypeOfWeapon.RayCast:
                RayCastAttack();
                break;
        }

        countOfBullets--;
        inventory.UpdateCountOfBulletsUI();
    }

    private void RayCastAttack(){
        Ray ray = new Ray(cameraController.transform.position, cameraController.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distanceShot, _layerMask)){
            if (hitInfo.transform.CompareTag("Body")){
                bodyHitFX.transform.position = hitInfo.transform.position;
                bodyHitFX.transform.up = -hitInfo.transform.forward;
                bodyHitFX.Play();
                var enemy = hitInfo.transform.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage, 1);
                _audioPlayer.PlayBodyShot();
            }

            if (hitInfo.transform.CompareTag("Head")){
                headHitFX.transform.position = hitInfo.transform.position;
                headHitFX.transform.up = -hitInfo.transform.forward;
                headHitFX.Play();
                var enemy = hitInfo.transform.GetComponentInParent<Enemy>();
                enemy.TakeDamage(damage, 3);
                _audioPlayer.PlayHeadShot();
            }
        }
    }

    private void ProjectileAttack(){
        bullet.transform.parent = null;
        bullet.Init(cameraController.transform.forward);
    }

    public string GetInfo(){
        string lang = YandexGame.savesData.language;
        switch (lang){
            case "ru":
                return
                    $"Урон: {damage}\nВыстрелов в секунду: {(1 / delayShotTime).ToString("0.##")}\nДальность: {distanceShot}\nДобавить патронов: {countOfAddBullets}";
            case "en":
                return
                    $"Damage: {damage}\nShots in seconds: {(1 / delayShotTime).ToString("0.##")}\nShot range: {distanceShot}\nAdd ammo: {countOfAddBullets}";
        }

        return null;
    }
}