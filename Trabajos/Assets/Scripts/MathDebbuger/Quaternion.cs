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

        public Vec3 eulerAngles // Con los euler angles se puede obtener los angulos de rotacion en grados
        {
            get
            {
                // angulos
                float angleX = Mathf.Rad2Deg * Mathf.Atan2(2 * (w * x + y * z), 1 - 2 * (x * x + y * y)); // Atan2 calcula el angulo entre el vector "2 * (w * x + y * z)" pryectado en el eje XY, y el eje X positivo
                float angleY = Mathf.Rad2Deg * Mathf.Asin(2 * (w * y - z * x)); // Asin calucla el angulo entre el vector "2 * (w * y - z * x)" proyectado en el plano YZ, y el eje Z positivo
                float angleZ = Mathf.Rad2Deg * Mathf.Atan2(2 * (w * z + x * y), 1 - 2 * (y * y + z * z)); // Atan2 calcula el angulo entre el vector "2 * (w * z + x * y)" pryectado en el eje XZ, y el eje X positivo

                return new Vec3(angleX, angleY, angleZ);
            }
        }

        public Quat normalized // Devuelve un quaternion normalizado llamando al metodo que tiene como parametro al mismo
        {
            get { return Normalize(this); }
        }
        #endregion

        #region Methods

        public static float Angle(Quat a, Quat b) // El angulo entre dos Quaternions se calcula a partir de su producto clampeado el cual devuelve un float que se le aplica Acos,
                                                              // y con este numero en radianes se lo pasa a grados
        {
            float dotProduct = Dot(a, b);
            dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
            float angleRad = Mathf.Acos(dotProduct);
            return Mathf.Rad2Deg * angleRad;
        }

        public static Quat AngleAxis(float angle, Vec3 axis) // El quaternion devuelto representa la rotacion sobre un eje de un angulo especificados, primero angulo despues eje
        {
            float angleRad = Mathf.Deg2Rad * angle;
            Vec3 normalizedAxis = axis.normalized;
            
            float halfAngle = angleRad * 0.5f;

            float sinHalfAngle = Mathf.Sin(halfAngle);
            float cosHalfAngle = Mathf.Cos(halfAngle);

            float qx = normalizedAxis.x * sinHalfAngle;
            float qy = normalizedAxis.y * sinHalfAngle;
            float qz = normalizedAxis.z * sinHalfAngle;
            float qw = cosHalfAngle;

            return new Quat(qx, qy, qz, qw);
        }

        public static Quat AxisAngle(Vec3 axis, float angle) // El quaternion devuelto representa la rotacion sobre un eje de un angulo especificados, primero eje despues angulo
        {
            float angleRad = Mathf.Deg2Rad * angle;
            Vec3 normalizedAxis = axis.normalized;

            float halfAngle = angleRad * 0.5f;

            float sinHalfAngle = Mathf.Sin(halfAngle);
            float cosHalfAngle = Mathf.Cos(halfAngle);

            float qx = normalizedAxis.x * sinHalfAngle;
            float qy = normalizedAxis.y * sinHalfAngle;
            float qz = normalizedAxis.z * sinHalfAngle;
            float qw = cosHalfAngle;

            return new Quat(qx, qy, qz, qw);
        }

        public static float Dot(Quat a, Quat b) // La suma total de la multiplcacion entre cada componente
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static Quat Euler(Vec3 euler) // Convierte angulos euler en un quaternion trabajando cada componente por separado, tomando como parametros un vector3
        {
            Vec3 eulerRad = euler * Mathf.Deg2Rad;

            float halfX = eulerRad.x * 0.5f;
            float halfY = eulerRad.y * 0.5f;
            float halfZ = eulerRad.z * 0.5f;

            float sinX = Mathf.Sin(halfX);
            float cosX = Mathf.Cos(halfX);
            float sinY = Mathf.Sin(halfY);
            float cosY = Mathf.Cos(halfY);
            float sinZ = Mathf.Sin(halfZ);
            float cosZ = Mathf.Cos(halfZ);

            float qx = sinX * cosY * cosZ - cosX * sinY * sinZ;
            float qy = cosX * sinY * cosZ + sinX * cosY * sinZ;
            float qz = cosX * cosY * sinZ - sinX * sinY * cosZ;
            float qw = cosX * cosY * cosZ + sinX * sinY * sinZ;

            return new Quat(qx, qy, qz, qw);
        }

        public static Quat Euler(float x, float y, float z) // Convierte angulos euler en un quaternion trabajando cada componente por separado, tomando como parametros componentes de un vector3
        {
            float halfX = (Mathf.Deg2Rad * x ) * 0.5f;
            float halfY = (Mathf.Deg2Rad * y) * 0.5f;
            float halfZ = (Mathf.Deg2Rad * z) * 0.5f;

            float sinX = Mathf.Sin(halfX);
            float cosX = Mathf.Cos(halfX);
            float sinY = Mathf.Sin(halfY);
            float cosY = Mathf.Cos(halfY);
            float sinZ = Mathf.Sin(halfZ);
            float cosZ = Mathf.Cos(halfZ);

            float qx = sinX * cosY * cosZ - cosX * sinY * sinZ;
            float qy = cosX * sinY * cosZ + sinX * cosY * sinZ;
            float qz = cosX * cosY * sinZ - sinX * sinY * cosZ;
            float qw = cosX * cosY * cosZ + sinX * sinY * sinZ;

            return new Quat(qx, qy, qz, qw);
        }

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
