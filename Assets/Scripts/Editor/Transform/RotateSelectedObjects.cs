using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

// does euler rotation which is fine for 2D but not really great for 3D
public class RotateSelectedObjects : ScriptableWizard {
    static Transform[] objectsToRotation = { };
    static Vector3 initialRotation = Vector3.zero;
    static Vector3 rotationStep = Vector3.zero;
    static bool local = true;

    public Transform[] ObjectsToRotate = { };
    public Vector3 InitialRotation = Vector3.zero;
    public Vector3 RotationStep = Vector3.zero;
    public bool Local = true;

    [MenuItem("Transform/Rotate Selected Objects...")]
    static void CreateWizard() {
        DisplayWizard("Rotate Selected Objects", typeof(RotateSelectedObjects), "Done");
    }

    public RotateSelectedObjects() {
        ObjectsToRotate = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        Array.Sort(ObjectsToRotate, (t1, t2) => t1.name.CompareTo(t2.name));
        InitialRotation = initialRotation;
        RotationStep = rotationStep;
        Local = local;

        Undo.RecordObjects(ObjectsToRotate, "Rotate Selected Objects");
    }

    void OnWizardUpdate() {
        objectsToRotation = ObjectsToRotate;
        initialRotation = InitialRotation;
        rotationStep = RotationStep;
        local = Local;

        // process results in update for better UX
        for (int i = 0; i < objectsToRotation.Length; i++) {
            var obj = objectsToRotation[i];
            if (local) {
                obj.localRotation = Quaternion.Euler(initialRotation + rotationStep * i);
            } else {
                obj.rotation = Quaternion.Euler(initialRotation + rotationStep * i);
            }
        }
    }

    void OnWizardCreate() {
    }
}