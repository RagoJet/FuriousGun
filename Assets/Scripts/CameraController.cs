using UnityEngine;

public class CameraController : MonoBehaviour{
    [SerializeField] private float rotationSpeed = 500f;


    private float yRotation;
    private float xRotation;

    private void Awake(){
        Cursor.visible = false;
    }

    private void Update(){
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;

        yRotation += mouseX * rotationSpeed;
        xRotation -= mouseY * rotationSpeed;

        xRotation = Mathf.Clamp(xRotation, -85f, 82f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}