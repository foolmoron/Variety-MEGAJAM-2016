using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FullscreenEffect : MonoBehaviour {

    public Material Effect;

    void Awake() {
        Effect = new Material(Effect);
    }
    
    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (Effect != null) {
            Graphics.Blit(source, destination, Effect);
        } else {
            Graphics.Blit(source, destination);
        }
    }
}