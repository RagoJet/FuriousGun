using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour{
    private PlayerController _player;
    private Inventory inventory;
    public int gold = 5000;

    [SerializeField] private ShopUI shopUI;

    [SerializeField] Image shopPanel;
    [SerializeField] Image openPanelImage;
    [SerializeField] private Transform tableToLook;
    [SerializeField] private Transform aslanShoper;
    private Animator _aslanAnimator;
    private static readonly int Client = Animator.StringToHash("Client");
    private static readonly int Busy = Animator.StringToHash("Busy");

    private Tween _tween;

    [SerializeField] private Image aimIcon;

    private void Start(){
        inventory = Inventory.Instance;
        _aslanAnimator = aslanShoper.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent(out PlayerController playerController)){
            openPanelImage.gameObject.SetActive(true);
            _player = playerController;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.TryGetComponent(out PlayerController playerController)){
            openPanelImage.gameObject.SetActive(false);
            _player = null;
        }
    }

    private void Update(){
        if (_player != null && Input.GetKeyDown(KeyCode.E)){
            OpenShopPanel();
        }
    }

    public void CloseShopPanel(){
        shopPanel.gameObject.SetActive(false);
        _aslanAnimator.SetTrigger(Busy);
        var lookDir = tableToLook.transform.position - aslanShoper.transform.position;
        lookDir.y = 0;
        _tween.Kill();
        _tween = aslanShoper.transform.DORotateQuaternion(Quaternion.LookRotation(lookDir), 1f);

        openPanelImage.gameObject.SetActive(true);
        _player.MakeAble();
        inventory.ShowWeapon();
        aimIcon.gameObject.SetActive(true);
    }

    public void OpenShopPanel(){
        shopPanel.gameObject.SetActive(true);
        _aslanAnimator.SetTrigger(Client);
        var lookDir = _player.transform.position - aslanShoper.transform.position;
        lookDir.y = 0;
        _tween.Kill();
        _tween = aslanShoper.transform.DORotateQuaternion(Quaternion.LookRotation(lookDir), 1f);

        openPanelImage.gameObject.SetActive(false);
        _player.Disable();
        shopUI.UpdateGoldUI(gold);
        inventory.HideWeapon();
        aimIcon.gameObject.SetActive(false);
    }

    public void BuyWeapon(int lvlWeapon){
        if (inventory.weapons[lvlWeapon].Price <= gold){
            Weapon weapon = inventory.weapons[lvlWeapon];
            if (weapon.available == false){
                weapon.available = true;
            }
            else{
                weapon.countOfBullets += weapon.countOfAddBullets;
                inventory.UpdateCountOfBulletsUI();
            }

            gold -= weapon.Price;
            shopUI.UpdateGoldUI(gold);
        }
    }
}