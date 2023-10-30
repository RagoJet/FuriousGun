using DG.Tweening;
using UnityEngine;

public class MainDoor : MonoBehaviour{
    [SerializeField] private Transform door;
    
    
    private Tween _tween;


    private void OpenTheDoor(){
        _tween.Kill();
        _tween = door.DORotate(new Vector3(0, 120f, 0f), 1f, RotateMode.Fast);
    }

    private void CloseTheDoor(){
        _tween.Kill();
        _tween = door.DORotate(new Vector3(0, 0f, 0f), 1f, RotateMode.Fast);
    }
}