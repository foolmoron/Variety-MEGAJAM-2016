using UnityEngine;
using System.Collections;

public class GrowWhenKey : MonoBehaviour {

    public KeyCode Key = KeyCode.Space;

    [Range(0, 20)]
    public float MinSize = 0f;
    [Range(0, 20)]
    public float MaxSize = 10.5f;
    [Range(0, 0.25f)]
    public float SizeSpeed = 0.1f;

    public AudioClip PressSound;
    public AudioClip ReleaseSound;

    void Start() {
    }
    
    void Update() {
        var targetSize = Input.GetKey(Key) ? MaxSize : MinSize;
        var currentSize = transform.localScale.x;
        var newSize = Mathf.Lerp(currentSize, targetSize, SizeSpeed);
        transform.localScale = new Vector3(newSize, newSize);

        if (Input.GetKeyDown(Key)) {
            AudioSource.PlayClipAtPoint(PressSound, Vector3.zero);
        } else if (Input.GetKeyUp(Key)) {
            AudioSource.PlayClipAtPoint(ReleaseSound, Vector3.zero);
        }
    }
}
