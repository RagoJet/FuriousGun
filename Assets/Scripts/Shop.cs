using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour{
    private PlayerController _player;

    [SerializeField] Image openPanelImage;
    [SerializeField] Image shopPanel;

    [SerializeField] private Transform tableToLook;
    [SerializeField] private Transform aslanShoper;
    private Animator _aslanAnimator;
    private static readonly int Client = Animator.StringToHash("Client");
    private static readonly int Busy = Animator.StringToHash("Busy");


    private void Awake(){
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
            shopPanel.gameObject.SetActive(true);
            _aslanAnimator.SetTrigger(Client);
            var lookDir = _player.transform.position - aslanShoper.transform.position;
            lookDir.y = 0;
            aslanShoper.transform.rotation = Quaternion.LookRotation(lookDir);

            openPanelImage.gameObject.SetActive(false);
            _player.Disable();
        }
    }

    public void CloseShopPanel(){
        shopPanel.gameObject.SetActive(false);
        _aslanAnimator.SetTrigger(Busy);
        aslanShoper.transform.LookAt(tableToLook, Vector3.up);


        openPanelImage.gameObject.SetActive(true);
        _player.MakeAble();
    }
}