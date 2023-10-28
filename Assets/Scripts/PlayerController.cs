using UnityEngine;

public class PlayerController : MonoBehaviour{
    [SerializeField] private Camera camera;
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    [SerializeField] private float speedMovement;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] float gravityValue = -50f;
    private bool _groundedPlayer;


    private void Awake(){
        _controller = GetComponent<CharacterController>();
    }

    void Update(){
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0){
            _playerVelocity.y = 0f;
        }

        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 direction = camera.transform.TransformDirection(inputVector);
        direction.y = 0;
        _controller.Move(direction.normalized * Time.deltaTime * speedMovement);


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && _groundedPlayer){
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
}