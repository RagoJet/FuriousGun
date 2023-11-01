using System;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private Sprite[] currentWeaponIcons;
    public Weapon[] weapons;
    private int _indexOfCurrentWeapon = -1;

    private void Start(){
        _indexOfCurrentWeapon = 0;
        weapons[_indexOfCurrentWeapon].ShowSelf();

        currentWeaponIcon.sprite = currentWeaponIcons[0];
    }

    public void GetAndShowWeapon(int index){
        if (!weapons[index].available || _indexOfCurrentWeapon == index){
            return;
        }

        weapons[_indexOfCurrentWeapon].HideSelf();
        _indexOfCurrentWeapon = index;
        weapons[_indexOfCurrentWeapon].ShowSelf();

        currentWeaponIcon.sprite = currentWeaponIcons[index];
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            GetAndShowWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            GetAndShowWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            GetAndShowWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)){
            GetAndShowWeapon(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)){
            GetAndShowWeapon(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)){
            GetAndShowWeapon(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)){
            GetAndShowWeapon(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)){
            GetAndShowWeapon(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)){
            GetAndShowWeapon(8);
        }
    }
}