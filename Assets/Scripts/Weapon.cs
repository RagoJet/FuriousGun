using System;
using DG.Tweening;
using UnityEngine;

public class Weapon : MonoBehaviour{
    [SerializeField] private int damage;
    [SerializeField] private int price;
    public int Price => price;
    public bool available;
    private bool _isInHand;

    private Vector3 _readyPosition;
    private Tween _tween;

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
        if (_isInHand){
            if (Input.GetMouseButtonDown(0)){
                Debug.Log("ATTTACK");
            }
        }
    }
}