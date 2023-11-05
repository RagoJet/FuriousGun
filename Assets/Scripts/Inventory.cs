using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{
    private Dictionary<KeyCode, int> weaponKeys = new Dictionary<KeyCode, int>();

    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private Sprite[] currentWeaponIcons;
    public Weapon[] weapons;
    private int _indexOfCurrentWeapon = -1;
    private int _lastUseIndexOfWeapon = 0;

    private bool _switchAccess = true;

    private void Awake(){
        weaponKeys[KeyCode.Alpha1] = 0;
        weaponKeys[KeyCode.Alpha2] = 1;
        weaponKeys[KeyCode.Alpha3] = 2;
        weaponKeys[KeyCode.Alpha4] = 3;
        weaponKeys[KeyCode.Alpha5] = 4;
        weaponKeys[KeyCode.Alpha6] = 5;
        weaponKeys[KeyCode.Alpha7] = 6;
        weaponKeys[KeyCode.Alpha8] = 7;
        weaponKeys[KeyCode.Alpha9] = 8;
    }

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
        _lastUseIndexOfWeapon = _indexOfCurrentWeapon;
        _indexOfCurrentWeapon = index;
        weapons[_indexOfCurrentWeapon].ShowSelf();

        currentWeaponIcon.sprite = currentWeaponIcons[index];
    }

    private void Update(){
        // switch weapons by MouseScroll
        float scrollDirection = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDirection != 0){
            int newIndex = _indexOfCurrentWeapon;
            if (scrollDirection > 0){
                newIndex++;

                if (newIndex > weapons.Length - 1){
                    newIndex = 0;
                }

                for (int i = newIndex; i <= weapons.Length; i++){
                    if (i > weapons.Length - 1){
                        i = 0;
                    }

                    if (weapons[i].available){
                        newIndex = i;
                        break;
                    }
                }
            }
            else{
                newIndex--;
                if (newIndex < 0){
                    newIndex = weapons.Length - 1;
                }

                for (int i = newIndex; i >= -1; i--){
                    if (i < 0){
                        i = weapons.Length - 1;
                    }

                    if (weapons[i].available){
                        newIndex = i;
                        break;
                    }
                }
            }

            SwitchWeapon(newIndex);
        }

        //
        // switching weapons by KeyValue
        foreach (var kvp in weaponKeys){
            if (Input.GetKeyDown(kvp.Key)){
                SwitchWeapon(kvp.Value);
                break;
            }
        }
        //

        if (Input.GetKeyDown(KeyCode.Q)){
            SwitchWeapon(_lastUseIndexOfWeapon);
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