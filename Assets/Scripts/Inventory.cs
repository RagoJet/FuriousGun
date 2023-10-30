using UnityEngine;

public class Inventory : MonoBehaviour{
    [SerializeField] private Weapon[] weapons;
    private int _indexOfCurrentWeapon = 0;

    public void GetAndShowWeapon(int index){
        weapons[_indexOfCurrentWeapon].gameObject.SetActive(false);
        _indexOfCurrentWeapon = index;
        weapons[_indexOfCurrentWeapon].gameObject.SetActive(true);
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