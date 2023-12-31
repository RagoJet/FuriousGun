using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Shop : MonoBehaviour{
    private PlayerController _player;
    private Inventory inventory;
    public int gold;

    [SerializeField] private ShopUI shopUI;

    [SerializeField] private TextMeshProUGUI infoShopText;
    [SerializeField] private TextMeshProUGUI promptOpenShopText;
    [SerializeField] private TextMeshProUGUI watchAdText;
    [SerializeField] private TextMeshProUGUI sliderCameraText;
    [SerializeField] private TextMeshProUGUI infoAboutWeaponsText;
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
            var lang = YandexGame.savesData.language;
            switch (lang){
                case "ru":
                    promptOpenShopText.text = "Нажмите 'У'";
                    break;
                case "en":
                    promptOpenShopText.text = "Press 'E'";
                    break;
            }

            openPanelImage.gameObject.SetActive(true);
            _player = playerController;
            WaveStarter.Instance.wasInShop = true;
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
            YandexGame.FullscreenShow();
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

        var lang = YandexGame.savesData.language;
        switch (lang){
            case "ru":
                infoShopText.text = "Информация об оружии\nНаведите курсор на оружие";
                watchAdText.text = "Смотреть рекламу\n+ $" + WaveStarter.Instance.Level * 350;
                sliderCameraText.text = "Скорость вращения камеры";
                infoAboutWeaponsText.text =
                    "Если оружие уже куплено, то при нажатии на кнопку покупки этого оружия, добавятся боеприпасы к нему.";
                break;
            case "en":
                infoShopText.text = "Info of weapon\nHover your cursor over the weapon";
                watchAdText.text = "Watch ad\n+ $" + WaveStarter.Instance.Level * 350;
                sliderCameraText.text = "Speed rotation of camera";
                infoAboutWeaponsText.text =
                    "If a weapon has already been purchased, clicking on the purchase button for that weapon will add ammunition to it.";
                break;
        }

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

    public void AddGold(int gold){
        this.gold += gold;
    }

    public void WatchRewardAd(){
        YandexGame.RewVideoShow(0);
        AddGold(WaveStarter.Instance.Level * 350);
        shopUI.UpdateGoldUI(gold);
    }
}