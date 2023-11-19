using System.Collections;
using DG.Tweening;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

public class WaveStarter : MonoBehaviour{
    public static WaveStarter Instance;
    [SerializeField] private Shop shop;
    [SerializeField] private Player player;
    [SerializeField] private EnemiesFactory enemiesFactory;
    [SerializeField] private Transform door;
    private Tween _tween;

    private int level = 1;
    public bool wasInShop;
    private bool _isWaveStarted = false;
    private float _timeOfWave = 60f;

    int countOfAliveMonsters;

    private void OnEnable() => YandexGame.GetDataEvent += LoadData;

    private void OnDisable() => YandexGame.GetDataEvent -= LoadData;

    public void LoadData(){
        level = YandexGame.savesData.level;
        shop.gold = YandexGame.savesData.gold;
        Weapon[] weapons = Inventory.Instance.weapons;
        CameraController.Instance.rotationSpeed = YandexGame.savesData.rotationCameraSpeed;
        CameraController.Instance.ChangeSliderValue();
        WeaponData[] weaponDatas = YandexGame.savesData.WeaponDatas;

        for (int i = 0; i < weapons.Length; i++){
            weapons[i].available = weaponDatas[i].available;
            weapons[i].countOfBullets = weaponDatas[i].amountOfAmmo;
        }
    }

    private void Start(){
        if (YandexGame.SDKEnabled == true){
            LoadData();
        }

        Instance = this;
    }

    private void OpenTheDoor(){
        AudioPlayer.Instance.PlayDoorSound();
        _tween.Kill();
        _tween = door.DORotate(new Vector3(0, 120f, 0f), 1.3f, RotateMode.Fast);
    }

    private void CloseTheDoor(){
        AudioPlayer.Instance.PlayDoorSound();
        AudioPlayer.Instance.PlayStartLevelSound();
        _tween.Kill();
        _tween = door.DORotate(new Vector3(0, 0f, 0f), 1.3f, RotateMode.Fast);
    }

    private void OnTriggerExit(Collider other){
        if (_isWaveStarted){
            return;
        }

        if (!wasInShop){
            return;
        }

        if (other.TryGetComponent(out Player player)){
            wasInShop = false;
            _isWaveStarted = true;
            CloseTheDoor();
            SaveData();
            StartCoroutine(StartWave());
        }
    }

    private IEnumerator StartWave(){
        while (_timeOfWave > 0){
            yield return new WaitForSeconds(1f);
            _timeOfWave -= 1f;

            if (_timeOfWave % 5 == 0){
                var countMonsters = Random.Range(1, 4);
                enemiesFactory.CreateMonsters(level, countMonsters);
                countOfAliveMonsters += countMonsters;
            }
        }

        _timeOfWave = 60 + level;
        StartCoroutine(LastWaveCheck());
    }

    IEnumerator LastWaveCheck(){
        while (countOfAliveMonsters > 0){
            yield return new WaitForSeconds(3f);
        }

        shop.AddGold(level * 500);
        level++;
        _isWaveStarted = false;
        OpenTheDoor();
        player.Refresh();
        SaveData();
    }

    public void MinusMonster(Enemy enemy){
        countOfAliveMonsters--;
    }


    public void SaveData(){
        YandexGame.savesData.rotationCameraSpeed = CameraController.Instance.rotationSpeed;
        YandexGame.savesData.level = level;
        YandexGame.savesData.gold = shop.gold;
        Weapon[] weapons = Inventory.Instance.weapons;

        YandexGame.savesData.WeaponDatas = new WeaponData[weapons.Length];

        for (int i = 0; i < weapons.Length; i++){
            YandexGame.savesData.WeaponDatas[i].available = weapons[i].available;
            YandexGame.savesData.WeaponDatas[i].amountOfAmmo = weapons[i].countOfBullets;
        }

        YandexGame.SaveProgress();
    }
}