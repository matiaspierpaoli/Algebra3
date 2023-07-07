using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public struct Quat
    {
        #region Variables
        public float x;
        public float y;
        public float z;
        public float w;
        #endregion

        #region Constants
        public const float kEpsolon = 1E-06F;
        #endregion

        #region Constructor
        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        #endregion

        #region Indexer
        public float this[int index] // Ahora es posible acceder a los elementos del quat en forma de array. quat[0] es la x por ejemplo
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default:
                        throw new IndexOutOfRangeException("Quaternion index out of range!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default:
                        throw new IndexOutOfRangeException("Quaternion index out of range!");
                }
            }
        }
        #endregion

        #region Properties
        public static Quat identity // Un quaternion solo lejible que no tiene rotacion al estar x,y,z en 0 y su real es 1 por lo que esta normalizado
        {
            get { return new Quat(0f, 0f, 0f, 1f); }
        }

        public Vector3 eulerAngles // Con los euler angles se puede obtener los angulos de rotacion en grados
        {
            get
            {
                // angulos
                float angleX = Mathf.Rad2Deg * Mathf.Atan2(2 * (w * x + y * z), 1 - 2 * (x * x + y * y)); // Atan2 calcula el angulo entre el vector "2 * (w * x + y * z)" pryectado en el eje XY, y el eje X positivo
                float angleY = Mathf.Rad2Deg * Mathf.Asin(2 * (w * y - z * x)); // Asin calucla el angulo entre el vector "2 * (w * y - z * x)" proyectado en el plano YZ, y el eje Z positivo
                float angleZ = Mathf.Rad2Deg * Mathf.Atan2(2 * (w * z + x * y), 1 - 2 * (y * y + z * z)); // Atan2 calcula el angulo entre el vector "2 * (w * z + x * y)" pryectado en el eje XZ, y el eje X positivo

                return new Vector3(angleX, angleY, angleZ);
            }
        }

        public Quat normalized // Devuelve un quaternion normalizado llamando al metodo que tiene como parametro al mismo
        {
            get { return Normalize(this); }
        }
        #endregion

        #region Methods

        public void Normalize() // Simplemente normalizar cada variable del quat sin devolver nada
        {
            float magnitude = Mathf.Sqrt(x * x + y * y + z * z + w * w);
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
            w /= magnitude;
        }

        public static Quat Normalize(Quat quaternion) // Se devuelve el quaternion que se pasa como parametro normalizado
        {
            float magnitude = Mathf.Sqrt(quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w);
            return new Quat(quaternion.x / magnitude, quaternion.y / magnitude, quaternion.z / magnitude, quaternion.w / magnitude);
        }
        #endregion

    }

}
