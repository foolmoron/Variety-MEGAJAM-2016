using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Player8 : MonoBehaviour {

    [Range(0, 10)]
    public float MoveSpeed = 5;
    [Range(0, 100)]
    public float HorizontalRotateSpeed = 25;

    CharacterController controller;
    new Camera camera;

    new Light light;
    [Range(0, 1000)]
    public float LightLeadingMultiplier = 100;
    [Range(0, 0.25f)]
    public float LightRotationSpeed = 0.1f;
    float targetLightRotation;
    
    void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        light = GetComponentInChildren<Light>();
    }
    
    void Update() {
        // light rotation leading player rotation
        {
            targetLightRotation = Input.GetAxis("Mouse X") * LightLeadingMultiplier;
            light.transform.localRotation = Quaternion.Euler(light.transform.localRotation.eulerAngles.x, Mathf.LerpAngle(light.transform.localRotation.eulerAngles.y, targetLightRotation, LightRotationSpeed), light.transform.localRotation.eulerAngles.z);
        }
        // player rotation
        {
            controller.transform.Rotate(0, HorizontalRotateSpeed * Input.GetAxis("Mouse X"), 0);
        }
        // player movement
        {
            controller.Move(transform.TransformVector(new Vector3(MoveSpeed * Input.GetAxis("Horizontal"), 0, MoveSpeed * Input.GetAxis("Vertical")) * MoveSpeed * Time.deltaTime));
        }
    }
}
