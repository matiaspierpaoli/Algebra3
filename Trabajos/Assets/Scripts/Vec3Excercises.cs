using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathDebbuger;
using CustomMath;

public class Vec3Excercises : MonoBehaviour
{
    private enum Ejercicio
    {
        Uno, Dos, Tres, Cuatro, Cinco, Seis, Siete, Ocho, Nueve, Diez
    }


    [SerializeField] private Ejercicio _ejercicio;
    [SerializeField] private Color _color;
    
    [SerializeField] private Vector3 normalVec3_A;
    [SerializeField] private Vector3 normalVec3_B;

    private Vec3 customVec3_A;
    private Vec3 customVec3_B;
    private Vec3 customVec3_C;

    private float time;
    private const float timeLimit = 10f;

    private void Start()
    {
        _ejercicio = Ejercicio.Uno;
        _color = Color.yellow;
        time = 0f;

        Vector3Debugger.AddVector(normalVec3_A, Color.cyan, "A");
        Vector3Debugger.EnableEditorView("A");
        Vector3Debugger.AddVector(normalVec3_B, Color.green, "B");
        Vector3Debugger.EnableEditorView("B");
        Vector3Debugger.AddVector(customVec3_C, _color, "C");
        Vector3Debugger.EnableEditorView("C");
    }

    private void Update()
    {
        customVec3_A = new Vec3(normalVec3_A);
        customVec3_B = new Vec3(normalVec3_B); 

        switch (_ejercicio)
        {
            case Ejercicio.Uno:
                customVec3_C = customVec3_A + customVec3_B;
                break;
            case Ejercicio.Dos:
                customVec3_C = customVec3_B - customVec3_A;
                break;
            case Ejercicio.Tres:
                customVec3_C = new Vec3(customVec3_A.x * customVec3_B.x, customVec3_A.y * customVec3_B.y, customVec3_A.z * customVec3_B.z);
                break;
            case Ejercicio.Cuatro:
                customVec3_C = Vec3.Cross(customVec3_B, customVec3_A);
                break;
            case Ejercicio.Cinco:
                time += Time.deltaTime;
                if (time > 1)
                    time = 0;
                customVec3_C = Vec3.Lerp(customVec3_A, customVec3_B, time);
                break;
            case Ejercicio.Seis:
                customVec3_C = Vec3.Max(customVec3_A, customVec3_B);
                break;
            case Ejercicio.Siete:
                customVec3_C = Vec3.Project(customVec3_A, customVec3_B);
                break;
            case Ejercicio.Ocho:
                customVec3_C = Vec3.Normalize(customVec3_A + customVec3_B) * Vec3.Distance(customVec3_A, customVec3_B);
                break;
            case Ejercicio.Nueve:
                customVec3_C = Vec3.Reflect(customVec3_A, Vec3.Normalize(customVec3_B));
                break;
            case Ejercicio.Diez:
                time += Time.deltaTime;
                if (time > timeLimit)
                    time = 0;
                customVec3_C = Vec3.LerpUnclamped(customVec3_B, customVec3_A, time);
                break;

        }

        Vector3Debugger.UpdatePosition("A", new Vector3(customVec3_A.x, customVec3_A.y, customVec3_A.z));
        Vector3Debugger.UpdatePosition("B", new Vector3(customVec3_B.x, customVec3_B.y, customVec3_B.z));
        Vector3Debugger.UpdatePosition("C", new Vector3(customVec3_C.x, customVec3_C.y, customVec3_C.z));
    }
}
