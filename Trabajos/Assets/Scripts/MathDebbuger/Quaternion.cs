using System;
using UnityEngine;

namespace Trabajos
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

        public static Quat FromToRotation(Vec3 fromDirection, Vec3 toDirection) // Se crea un quaternion que representa la rotacion desde un vector a otro
        {
            Vec3 from = fromDirection.normalized;
            Vec3 to = toDirection.normalized;

            float dot = Vec3.Dot(from, to);

            if (dot >= 1f - Mathf.Epsilon) // Si son paralelos
            {
                return identity;
            }
            else if (dot <= -1f + Mathf.Epsilon) // Si son opuestos, se rota en 180 en un eje arbitrario
            {
                Vec3 orthogonalAxis = Vec3.Cross(Vec3.Forward, from).normalized;
                return AngleAxis(180f, orthogonalAxis);
            }
            else
            {
                Vec3 rotationAxis = Vec3.Cross(from, to).normalized;
                float rotationAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                return AngleAxis(rotationAngle, rotationAxis);
            }
        }

        public static Quat Inverse(Quat rotation) // Se niegan los componentes imaginarios pero el real se mantiene
        {
            return new Quat(-rotation.x, -rotation.y, -rotation.z, rotation.w);
        }

        public static Quat Lerp(Quat a, Quat b, float t) // Se realiza una interpoalcion lineal entre dos Quaternions con un parametro de interpolacion clampeado
        {
            t = Mathf.Clamp01(t);

            return new Quat(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
        }

        public static Quat LerpUnclamped(Quat a, Quat b, float t) // Se realiza una interpoalcion lineal entre dos Quaternions con un parametro de interpolacion sin clampear
        {
            return new Quat(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
        }

        public static Quat LookRotation(Vec3 forward) // Usado para calular el quaternion de rotacion, asumiendo Vec3.Up como el up 
        {
            Vec3 up = Vec3.Up; 
            return LookRotation(forward, up);
        }

        public static Quat LookRotation(Vec3 forward, Vec3 upwards) // Usado para calular el quaternion de rotacion, indicando el forward y upward
        {
            Vec3 normalizedForward = forward.normalized;
            Vec3 normalizedUpwards = upwards.normalized;

            Quat rotation = Quat.identity;
            rotation.SetLookRotation(normalizedForward, normalizedUpwards);

            return rotation;
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

        public static Quat RotateTowards(Quat from, Quat to, float maxDegreesDelta) // Se rota un quaternion hacia otro mediante un angulo maximo
        {
            float maxRadiansDelta = maxDegreesDelta * Mathf.Deg2Rad;

            float angle = Quat.Angle(from, to);

            if (angle <= maxRadiansDelta) 
            {
                return to;
            }

            float t = maxRadiansDelta / angle;

            return Quat.Slerp(from, to, t);
        }

        public static Quat Slerp(Quat a, Quat b, float t) // Lerp en forma esferica. Se rota un quat mediante un parametro t clampeado, siempre por el camino mas corto
        {
            t = Mathf.Clamp01(t);

            float dot = Quat.Dot(a, b);

            if (dot < 0f) // SI uno esta mirando en la direccion contraria se da vuelta
            {
                b = Quat.Negate(b);
                dot = -dot;
            }

            float epsilon = 1e-6f;
            float oneMinusEpsilon = 1f - epsilon;

            if (dot > oneMinusEpsilon) // Si estan cerca
            {
                Quat result = Quat.Lerp(a, b, t); // Interpolacion normal
                return result.normalized;
            }
            else
            {
                float theta = Mathf.Acos(dot);
                float sinTheta = Mathf.Sin(theta);

                float factorA = Mathf.Sin((1f - t) * theta) / sinTheta;
                float factorB = Mathf.Sin(t * theta) / sinTheta;

                Quat result = factorA * a + factorB * b;
                return result.normalized;
            }
        }

        public static Quat SlerpUnclumped(Quat a, Quat b, float t) // Lerp en forma esferica. Se rota un quat mediante un parametro t sin clampear, siempre por el camino mas corto
        {
            float dot = Quat.Dot(a, b);

            if (dot < 0f) // SI uno esta mirando en la direccion contraria se da vuelta
            {
                b = Quat.Negate(b);
                dot = -dot;
            }

            float epsilon = 1e-6f;
            float oneMinusEpsilon = 1f - epsilon;

            if (dot > oneMinusEpsilon) // Si estan cerca
            {
                Quat result = Quat.Lerp(a, b, t); // Interpolacion normal
                return result.normalized;
            }
            else
            {
                float theta = Mathf.Acos(dot);
                float sinTheta = Mathf.Sin(theta);

                float factorA = Mathf.Sin((1f - t) * theta) / sinTheta;
                float factorB = Mathf.Sin(t * theta) / sinTheta;

                Quat result = factorA * a + factorB * b;
                return result.normalized;
            }
        }

        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection) // Se crea una rotacion que empieza en fromDirection y termina en toDirection
        {
            Vec3 normalizedFrom = fromDirection.normalized;
            Vec3 normalizedTo = toDirection.normalized;

            float dot = Vec3.Dot(normalizedFrom, normalizedTo);

            Vec3 cross = Vec3.Cross(normalizedFrom, normalizedTo);

            x = cross.x;
            y = cross.y;
            z = cross.z;
            w = 1f + dot;

            Normalize();
        }

        public void SetLookRotation(Vec3 view) // Alinea la direccion forward con la view designada
        {
            Vec3 forward = view.normalized;

            Vec3 up = Vec3.Up;
            Vec3 right = Vec3.Cross(forward, up).normalized;
            up = Vec3.Cross(right, forward).normalized;

            float m00 = right.x;
            float m01 = right.y;
            float m02 = right.z;
            float m10 = up.x;
            float m11 = up.y;
            float m12 = up.z;
            float m20 = forward.x;
            float m21 = forward.y;
            float m22 = forward.z;

            float trace = m00 + m11 + m22;
            float w, x, y, z;

            if (trace > 0f)
            {
                float s = Mathf.Sqrt(trace + 1f) * 2f;
                float invS = 1f / s;
                w = 0.25f * s;
                x = (m21 - m12) * invS;
                y = (m02 - m20) * invS;
                z = (m10 - m01) * invS;
            }
            else if (m00 > m11 && m00 > m22)
            {
                float s = Mathf.Sqrt(1f + m00 - m11 - m22) * 2f;
                float invS = 1f / s;
                w = (m21 - m12) * invS;
                x = 0.25f * s;
                y = (m01 + m10) * invS;
                z = (m02 + m20) * invS;
            }
            else if (m11 > m22)
            {
                float s = Mathf.Sqrt(1f + m11 - m00 - m22) * 2f;
                float invS = 1f / s;
                w = (m02 - m20) * invS;
                x = (m01 + m10) * invS;
                y = 0.25f * s;
                z = (m12 + m21) * invS;
            }
            else
            {
                float s = Mathf.Sqrt(1f + m22 - m00 - m11) * 2f;
                float invS = 1f / s;
                w = (m10 - m01) * invS;
                x = (m02 + m20) * invS;
                y = (m12 + m21) * invS;
                z = 0.25f * s;
            }

            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        private void SetLookRotation(Vec3 view, Vec3 up) // El quaternion resultante representa la rotation requerida para alinear la forward con las view y up designadas
        {
            Vec3 forward = view.normalized;
            Vec3 right = Vec3.Cross(up, forward).normalized;
            Vec3 newUp = Vec3.Cross(forward, right);

            float m00 = right.x;
            float m01 = right.y;
            float m02 = right.z;
            float m10 = newUp.x;
            float m11 = newUp.y;
            float m12 = newUp.z;
            float m20 = forward.x;
            float m21 = forward.y;
            float m22 = forward.z;

            float num8 = (m00 + m11) + m22;
            if (num8 > 0f)
            {
                float num = Mathf.Sqrt(num8 + 1f);
                w = num * 0.5f;
                num = 0.5f / num;
                x = (m12 - m21) * num;
                y = (m20 - m02) * num;
                z = (m01 - m10) * num;
                return;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                float num7 = Mathf.Sqrt(((1f + m00) - m11) - m22);
                float num4 = 0.5f / num7;
                x = 0.5f * num7;
                y = (m01 + m10) * num4;
                z = (m02 + m20) * num4;
                w = (m12 - m21) * num4;
                return;
            }
            if (m11 > m22)
            {
                float num6 = Mathf.Sqrt(((1f + m11) - m00) - m22);
                float num3 = 0.5f / num6;
                x = (m10 + m01) * num3;
                y = 0.5f * num6;
                z = (m21 + m12) * num3;
                w = (m20 - m02) * num3;
                return;
            }
            float num5 = Mathf.Sqrt(((1f + m22) - m00) - m11);
            float num2 = 0.5f / num5;
            x = (m20 + m02) * num2;
            y = (m21 + m12) * num2;
            z = 0.5f * num5;
            w = (m01 - m10) * num2;
        }

        public void ToAngleAxis(out float angle, out Vec3 axis) // Sirve para simplificar la visualizacion de quaternions.
                                                                // Se representa la rotacion de un angulo a lo largo de un eje, el cual representa la direccion de la rotacion.
                                                                // Mientras que el angulo representa la magnitud
        {
            Quat normalizedQuaternion = new Quat();
            normalizedQuaternion.Normalize();

            angle = 2f * Mathf.Acos(normalizedQuaternion.w) * Mathf.Rad2Deg;

            float sinHalfAngle = Mathf.Sqrt(1f - normalizedQuaternion.w * normalizedQuaternion.w);
            if (Mathf.Abs(sinHalfAngle) < 0.0001f)
            {
                axis = Vec3.Up; 
            }
            else
            {
                float invSinHalfAngle = 1f / sinHalfAngle;
                axis = new Vec3(
                    normalizedQuaternion.x * invSinHalfAngle,
                    normalizedQuaternion.y * invSinHalfAngle,
                    normalizedQuaternion.z * invSinHalfAngle
                ).normalized;
            }
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }


        public static Quat Negate(Quat quat) // Hacer que el quaternion mire en la direccion opuesta 
        {
            return new Quat(-quat.x, -quat.y, -quat.z, -quat.w);
        }
        #endregion

        #region Operators
        public static Quat operator *(float scalar, Quat quat)
        {
            return new Quat(scalar * quat.x, scalar * quat.y, scalar * quat.z, scalar * quat.w);
        }

        public static Quat operator +(Quat quat1, Quat quat2)
        {
            return new Quat(quat1.x + quat2.x, quat1.y + quat2.y, quat1.z + quat2.z, quat1.w + quat2.w);
        }

        public static bool operator ==(Quat lhs, Quat rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
        }

        public static bool operator !=(Quat lhs, Quat rhs)
        {
            return !(lhs == rhs);
        }

        public static Vec3 operator *(Quat rotation, Vec3 point)
        {
            Vec3 vector = new Vec3(rotation.x, rotation.y, rotation.z);

            Vec3 crossProduct = Vec3.Cross(vector, point);

            Vec3 scaledVector = 2f * rotation.w * crossProduct;

            Vec3 multipliedVector = point + scaledVector + Vec3.Cross(vector, crossProduct);

            return multipliedVector;
        }

        public static Quat operator *(Quat lhs, Quat rhs)
        {
            float x = lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y;
            float y = lhs.w * rhs.y - lhs.x * rhs.z + lhs.y * rhs.w + lhs.z * rhs.x;
            float z = lhs.w * rhs.z + lhs.x * rhs.y - lhs.y * rhs.x + lhs.z * rhs.w;
            float w = lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z;

            return new Quat(x, y, z, w);
        }
        #endregion 
    }

}
