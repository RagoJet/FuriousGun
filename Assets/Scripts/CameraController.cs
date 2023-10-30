using UnityEngine;

public class CameraController : MonoBehaviour{
    [SerializeField] private float rotationSpeed = 500f;


    private float yRotation;
    private float xRotation;

    private bool _isAble = true;

    private void Awake(){
        MakeAble();
    }

    private void Update(){
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
        Cursor.visible = true;
    }

    public void MakeAble(){
        _isAble = true;
        Cursor.visible = false;
    }
}