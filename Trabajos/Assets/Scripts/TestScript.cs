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

        // Start is called before the first frame update
        void Start()
        {
            v1 = new Vector3(from_x, from_y, from_z);
            v2 = new Vector3(to_x, to_y, to_z);
            v3 = new Vec3(from_x, from_y, from_z);
            v4 = new Vec3(to_x, to_y, to_z);

            //result1 = Vector3.Angle(v1, v2);
            //result2 = Vec3.Angle(v3, v4);

            Debug.Log(result1);
            Debug.Log(result2);
        }
    }  
}
