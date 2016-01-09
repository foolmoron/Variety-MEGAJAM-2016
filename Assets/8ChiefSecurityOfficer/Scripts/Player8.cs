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


    public AnimationCurve MoveBobbing;
    public float MoveAnimationScale;
    public float CameraMoveScale;
    float animationPosition;

    Shaker shaker;
    [Range(0, 0.2f)]
    public float MaxShake = 0.04f;
    
    void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        light = GetComponentInChildren<Light>();
        shaker = GetComponentInChildren<Shaker>();
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
        var movement = 0f;
        {
            movement = MoveSpeed * Mathf.Max(Input.GetAxis("Vertical"), 0) * MoveSpeed * Time.deltaTime;
            controller.Move(transform.TransformVector(0, 0, movement));
        }
        // animate based on movement
        {
            if (movement != 0) {
                animationPosition += movement * MoveAnimationScale;
                camera.transform.localRotation = Quaternion.Euler(0, 0, MoveBobbing.Evaluate(animationPosition) * 360f);
                camera.transform.localPosition = camera.transform.localPosition.withX(MoveBobbing.Evaluate(animationPosition) * CameraMoveScale);
            } else {
                camera.transform.localRotation = Quaternion.identity;
                camera.transform.localPosition = camera.transform.localPosition.withX(0);
            }
        }
    }
}
