using UnityEngine;
using System.Collections;

public class Follow10 : MonoBehaviour {

    public Transform Target;
    
    void Update() {
        transform.position = Target.position.withZ(transform.position.z);
    }
}
