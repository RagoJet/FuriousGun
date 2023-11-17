using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveStarter : MonoBehaviour{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI timeOfWaveText;
    [SerializeField] private EnemiesFactory enemiesFactory;
    [SerializeField] private Transform door;
    private Tween _tween;

    private int level = 15;
    private bool _isWaveStarted = false;
    private float _timeOfWave = 30f;

    int countOfAliveMonsters;

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

        if (other.TryGetComponent(out Player player)){
            _isWaveStarted = true;
            CloseTheDoor();
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
                enemiesFactory.CreateMonsters(level, countMonsters, this);
                countOfAliveMonsters += countMonsters;
            }
        }

        _timeOfWave = 60;
        timeOfWaveText.text = "";
        StartCoroutine(LastWaveCheck());
    }

    IEnumerator LastWaveCheck(){
        while (countOfAliveMonsters > 0){
            yield return new WaitForSeconds(3f);
        }

        level++;
        _isWaveStarted = false;
        OpenTheDoor();
        player.Refresh();
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
}