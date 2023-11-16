using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyWeaponButton : Button, IPointerEnterHandler, IPointerExitHandler{
    [SerializeField] private int indexOfButton;
    [SerializeField] private TextMeshProUGUI infoText;


    public void OnPointerEnter(PointerEventData eventData){
        infoText.text = Inventory.Instance.weapons[indexOfButton].GetInfo();
    }

    public void OnPointerExit(PointerEventData eventData){
        infoText.text = "";
    }
}