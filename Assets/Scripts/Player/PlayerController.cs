using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    [SerializeField] private Camera weaponCamera;
    [SerializeField] private CameraController cameraController;
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    [SerializeField] private float speedMovement;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] float gravityValue = -50f;
    private bool _groundedPlayer;

    private bool _isAble = true;

    private Sequence _sequenceWeaponCamera;


    private void Awake(){
        _controller = GetComponent<CharacterController>();
    }

    public void Updater(){
        if (_isAble == false){
            return;
        }

        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0){
            _playerVelocity.y = 0f;
        }

        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 direction = cameraController.transform.TransformDirection(inputVector);
        direction.y = 0;
        _controller.Move(direction.normalized * Time.deltaTime * speedMovement);

        if (!_sequenceWeaponCamera.IsActive() && direction != Vector3.zero){
            InitWeaponCameraSequence();
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && _groundedPlayer){
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }


    public void Disable(){
        _isAble = false;
        cameraController.Disable();
    }

    public void MakeAble(){
        _isAble = true;
        cameraController.MakeAble();
    }

    private void InitWeaponCameraSequence(){
        _sequenceWeaponCamera = DOTween.Sequence();
        Vector3 defaultPos = weaponCamera.transform.localPosition;
        Vector3 toRightVector = new Vector3(0.11f, 0.02f, -0.05f);
        Vector3 toLeftVector = new Vector3(0.09f, 0.02f, -0.05f);

        float duration = 0.17f;

        _sequenceWeaponCamera.Append(weaponCamera.transform.DOLocalMove(toRightVector, duration));
        _sequenceWeaponCamera.Append(weaponCamera.transform.DOLocalMove(defaultPos, duration));
        _sequenceWeaponCamera.Append(weaponCamera.transform.DOLocalMove(toLeftVector, duration));
        _sequenceWeaponCamera.Append(weaponCamera.transform.DOLocalMove(defaultPos, duration));
    }
}