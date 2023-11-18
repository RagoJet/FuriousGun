using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

public class WaveStarter : MonoBehaviour{
    public static WaveStarter Instance;
    [SerializeField] private Shop shop;
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI timeOfWaveText;
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

        WeaponData[] weaponDatas = YandexGame.savesData.WeaponDatas;

        for (int i = 0; i < weapons.Length; i++){
            weapons[0].available = weaponDatas[0].available;
            weapons[0].countOfBullets = weaponDatas[0].amountOfAmmo;
        }
    }

    private void Awake(){
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
            timeOfWaveText.text = GetMinSec(_timeOfWave);

            if (_timeOfWave % 5 == 0){
                var countMonsters = Random.Range(1, 4);
                enemiesFactory.CreateMonsters(level, countMonsters);
                countOfAliveMonsters += countMonsters;
            }
        }

        _timeOfWave = 60 + level;
        timeOfWaveText.text = "";
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

    private String GetMinSec(float sec){
        int min = (int) sec / 60;
        sec = sec % 60;
        if (sec < 10){
            return $"{min}:0{sec}";
        }
        else{
            return $"{min}:{sec}";
        }
    }

    public void SaveData(){
        YandexGame.savesData.level = level;
        YandexGame.savesData.gold = shop.gold;
        Weapon[] weapons = Inventory.Instance.weapons;

        YandexGame.savesData.WeaponDatas = new WeaponData[weapons.Length];

        for (int i = 0; i < weapons.Length; i++){
            YandexGame.savesData.WeaponDatas[0].available = weapons[0].available;
            YandexGame.savesData.WeaponDatas[0].amountOfAmmo = weapons[0].countOfBullets;
        }
    }
}