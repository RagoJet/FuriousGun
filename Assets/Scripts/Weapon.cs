using UnityEngine;

public enum TypeOfWeapon{
}

public class Weapon : MonoBehaviour{
    [SerializeField] TypeOfWeapon typeOfWeapon;
    [SerializeField] private int damage;

    private bool available = true;
}