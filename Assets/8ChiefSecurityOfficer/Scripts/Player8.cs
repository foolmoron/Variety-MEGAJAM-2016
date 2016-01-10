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

    public float RotationSoundTrigger = 0.05f;
    bool soundTriggered;
    public AudioClip StepSound;

    Shaker shaker;
    [Range(0, 0.2f)]
    public float MaxShake = 0.04f;

    public AnimationCurve DistanceToStatic;
    ScaryItem[] items;
    Staticer staticer;
    public AudioClip ItemSound;

    public TextMesh ItemCount;
    [Range(0, 5)]
    public float ItemCountTime = 2;
    float itemCountTime;

    public GameObject Instructions;

    void Start() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        light = GetComponentInChildren<Light>();
        shaker = GetComponentInChildren<Shaker>();

        items = FindObjectsOfType<ScaryItem>();
        staticer = FindObjectOfType<Staticer>();
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
        // trigger sound based on animation
        {
            var angle = Mathf.Min(Mathf.Abs(camera.transform.transform.localRotation.eulerAngles.z), Mathf.Abs(camera.transform.transform.localRotation.eulerAngles.z - 360));
            if (!soundTriggered && angle >= RotationSoundTrigger) {
                soundTriggered = true;
                AudioSource.PlayClipAtPoint(StepSound, camera.transform.position.withY(0));
            } else if (angle < RotationSoundTrigger) {
                soundTriggered = false;
            }
        }
        // static based on distance to item
        {
            ScaryItem nearestItem = null;
            var nearestDistance = float.PositiveInfinity;
            for (int i = 0; i < items.Length; i++) {
                if (items[i] && (items[i].transform.position - transform.position).magnitude < nearestDistance) {
                    nearestItem = items[i];
                    nearestDistance = (items[i].transform.position - transform.position).magnitude;
                }
            }
            staticer.Amount = DistanceToStatic.Evaluate(nearestDistance);
        }
        // item count text
        {
            var itemsLeft = 0;
            for (int i = 0; i < items.Length; i++) {
                if (items[i]) {
                    itemsLeft++;
                }
            }
            ItemCount.text = (items.Length - itemsLeft) + " / " + items.Length;
        }
        // item count display
        {
            if (itemCountTime > 0) {
                itemCountTime -= Time.deltaTime;
                ItemCount.gameObject.SetActive(staticer.OnFrames == 0);
            } else {
                ItemCount.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        var item = other.GetComponent<ScaryItem>();
        if (item) {
            Destroy(item.gameObject);
            staticer.OnFrames = 20;
            itemCountTime = ItemCountTime;
            AudioSource.PlayClipAtPoint(ItemSound, camera.transform.position);
        }
        // turn off instructions when hitting trigger around the first corner
        {
            Instructions.SetActive(false);
        }
    }
}
