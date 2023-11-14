using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour{
    [SerializeField] private Inventory inventory;

    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI[] weaponPrices;


    private void Awake(){
        UpdateWeaponsPrice();
    }

    private void UpdateWeaponsPrice(){
        for (int i = 0; i < weaponPrices.Length; i++){
            weaponPrices[i].text = "$" + inventory.weapons[i].Price;
        }
    }

    public void UpdateGoldUI(int gold){
        this.gold.text = "$" + gold;
    }
}