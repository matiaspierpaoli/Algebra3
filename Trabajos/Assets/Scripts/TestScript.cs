using UnityEngine;

namespace CustomMath
{
    public class TestScript : MonoBehaviour
    {
        private Vector3 v1;
        private Vector3 v2;

        private Vec3 v3;
        private Vec3 v4;

        [SerializeField] private float from_x;
        [SerializeField] private float from_y;
        [SerializeField] private float from_z;

        [SerializeField] private float to_x;
        [SerializeField] private float to_y;
        [SerializeField] private float to_z;

        float result1;
        float result2;

        float maxLenght = 10;
        float timeElapsed = 10;

        // Start is called before the first frame update
        void Start()
        {
            v1 = new Vector3(from_x, from_y, from_z);
            v2 = new Vector3(to_x, to_y, to_z);
            v3 = new Vec3(from_x, from_y, from_z);
            v4 = new Vec3(to_x, to_y, to_z);

            //result1 = Vector3.Angle(v1, v2);
            //result2 = Vec3.Angle(v3, v4);

            //result1 = Vector3.Magnitude(v1);
            //result2 = Vec3.Magnitude(v3);

            //Debug.Log(Vector3.ClampMagnitude(v1, maxLenght));
            //Debug.Log(Vec3.ClampMagnitude(v3, maxLenght));

            //Debug.Log(Vector3.Cross(v1, v2));
            //Debug.Log(Vec3.Cross(v3, v4));

            //result1 = Vector3.Distance(v1, v2);
            //result2 = Vec3.Distance(v3, v4);

            //result1 = Vector3.Dot(v1, v2);
            //result2 = Vec3.Dot(v3, v4);

            //Debug.Log(Vector3.Lerp(v1, v2, timeElapsed));
            //Debug.Log(Vec3.Lerp(v3, v4, timeElapsed));

            //Debug.Log(Vector3.LerpUnclamped(v1, v2, timeElapsed));
            //Debug.Log(Vec3.LerpUnclamped(v3, v4, timeElapsed));

            Debug.Log(Vector3.Max(v1, v2));
            Debug.Log(Vec3.Max(v3, v4));

            Debug.Log(Vector3.Min(v1, v2));
            Debug.Log(Vec3.Min(v3, v4));

            //Debug.Log(result1);
            //Debug.Log(result2);
        }
    }  
}
