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
    
    void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        light = GetComponentInChildren<Light>();
    }
    
    void Update() {
        // rotation
        {
            controller.transform.Rotate(0, HorizontalRotateSpeed * Input.GetAxis("Mouse X"), 0);
        }
        // movement
        {
            controller.Move(transform.TransformVector(new Vector3(MoveSpeed * Input.GetAxis("Horizontal"), 0, MoveSpeed * Input.GetAxis("Vertical")) * MoveSpeed * Time.deltaTime));
        }
    }
}
