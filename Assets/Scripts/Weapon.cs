using UnityEngine;

public class Weapon : MonoBehaviour{
    [SerializeField] private int damage;
    [SerializeField] private int price;

    public int Price => price;
    public bool available;
   
}