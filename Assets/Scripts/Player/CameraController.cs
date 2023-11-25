using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour{
    public static CameraController Instance;
    public float rotationSpeed = 350f;
    [SerializeField] private Slider slider;


    private float yRotation;
    private float xRotation;

    private bool _isAble = true;

    private void Awake(){
        Instance = this;
        MakeAble();
    }

    public void Updater(){
        if (_isAble == false){
            return;
        }

        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;

        yRotation += mouseX * rotationSpeed;
        xRotation -= mouseY * rotationSpeed;

        xRotation = Mathf.Clamp(xRotation, -85f, 82f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void Disable(){
        _isAble = false;
        // Cursor.visible = true;
        Screen.lockCursor = false;
    }

    public void MakeAble(){
        _isAble = true;
        // Cursor.visible = false;
        Screen.lockCursor = true;
    }

    public void SeeToTheSky(){
        transform.DORotate(new Vector3(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 2f);
        transform.DOLocalMoveY(-0.5f, 1.5f);
    }

    public void ChangeSpeed(float x){
        rotationSpeed = x * 600 + 25;
    }

    public void ChangeSliderValue(){
        slider.value = (rotationSpeed - 25) / 600f;
    }
}