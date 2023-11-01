using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private Sprite[] currentWeaponIcons;
    public Weapon[] weapons;
    private int _indexOfCurrentWeapon = -1;

    private bool _switchAccess = true;

    private void Start(){
        _indexOfCurrentWeapon = 0;
        weapons[_indexOfCurrentWeapon].ShowSelf();

        currentWeaponIcon.sprite = currentWeaponIcons[0];
    }


    private void SwitchWeapon(int index){
        if (!_switchAccess){
            return;
        }

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
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            SwitchWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)){
            SwitchWeapon(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)){
            SwitchWeapon(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)){
            SwitchWeapon(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)){
            SwitchWeapon(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)){
            SwitchWeapon(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)){
            SwitchWeapon(8);
        }
    }

    public void ShowWeapon(){
        weapons[_indexOfCurrentWeapon].ShowSelf();
        _switchAccess = true;
    }

    public void HideWeapon(){
        weapons[_indexOfCurrentWeapon].HideSelf();
        _switchAccess = false;
    }
}