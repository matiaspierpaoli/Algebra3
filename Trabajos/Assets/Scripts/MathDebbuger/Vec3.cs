using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CustomMath
{
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Variables
        public float x;
        public float y;
        public float z;

        public float sqrMagnitude 
        { 
            get 
            { 
                Vec3 newVec3 = new Vec3(x,y,z);
                return Magnitude(newVec3); 
            }  
        }

        public Vec3 normalized 
        {
            get 
            {
                Vec3 newVec3 = new Vec3(x,y,z);
                float length = newVec3.magnitude;
                if (length > 0)                
                    return newVec3 / length;              
                else                
                    return Zero;
                

                //Vec3 newVec3 = new Vec3(x, y, z);
                //newVec3.Normalize();
                //return newVec3;
            }
        }

        public float magnitude 
        {
            get 
            {
                return Mathf.Sqrt(x * x + y * y + z * z); 
            }
        }
        #endregion

        #region constants
        public const float epsilon = 1e-05f;
        #endregion

        #region Default Values
        public static Vec3 Zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
        public static Vec3 One { get { return new Vec3(1.0f, 1.0f, 1.0f); } }
        public static Vec3 Forward { get { return new Vec3(0.0f, 0.0f, 1.0f); } }
        public static Vec3 Back { get { return new Vec3(0.0f, 0.0f, -1.0f); } }
        public static Vec3 Right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }
        public static Vec3 Left { get { return new Vec3(-1.0f, 0.0f, 0.0f); } }
        public static Vec3 Up { get { return new Vec3(0.0f, 1.0f, 0.0f); } }
        public static Vec3 Down { get { return new Vec3(0.0f, -1.0f, 0.0f); } }
        public static Vec3 PositiveInfinity { get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }
        public static Vec3 NegativeInfinity { get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }
        #endregion                                                                                                                                                                               

        #region Constructors
        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector2 v2)
        {
            this.x = v2.x;
            this.y = v2.y;
            this.z = 0.0f;
        }
        #endregion

        #region Operators
        public static bool operator ==(Vec3 left, Vec3 right)
        {
            float diff_x = left.x - right.x;
            float diff_y = left.y - right.y;
            float diff_z = left.z - right.z;
            float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;
            return sqrmag < epsilon * epsilon;
        }
        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        public static Vec3 operator +(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x + rightV3.x, leftV3.y + rightV3.y, leftV3.z + rightV3.z);
        }

        public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x - rightV3.x, leftV3.y - rightV3.y, leftV3.z - rightV3.z) ;
        }

        public static Vec3 operator -(Vec3 v3)
        {
            return new Vec3(-v3.x, -v3.y, -v3.z);
        }

        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }
        public static Vec3 operator *(float scalar, Vec3 v3)
        {
            return new Vec3(scalar * v3.x, scalar * v3.y, scalar * v3.z);
        }
        public static Vec3 operator /(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x / scalar, v3.y / scalar, v3.z / scalar);
        }

        public static implicit operator Vector3(Vec3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }

        public static implicit operator Vector2(Vec3 v2)
        {
            return new Vector3(v2.x, v2.y);
        }
        #endregion

        #region Functions
        public override string ToString()
        {
            return "X = " + x.ToString() + "   Y = " + y.ToString() + "   Z = " + z.ToString();
        }
        public static float Angle(Vec3 from, Vec3 to)
        {
            float dot = Dot(from.normalized, to.normalized); // Producto de los dos vectores normalizados
            return Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg; // Arcocoseno del producto es el angulo en radianes, pero luego se pasa a grados
        }
        public static Vec3 ClampMagnitude(Vec3 vector, float maxLength) // Cortar magnitud
        {
            return new Vec3((vector / Magnitude(vector)) * maxLength);
        }
        public static float Magnitude(Vec3 vector) // Distancia entre un punto y otro, mediante pitagoras
        {
            return (float)Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }
        public static Vec3 Cross(Vec3 a, Vec3 b) // Producto punto, vector perpendicular a los otros dos pasados por parametro
        {
            return new Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }
        public static float Distance(Vec3 a, Vec3 b) // Distancia entre dos vectores mediante pitagoras
        {
            float distance_x = a.x - b.x;
            float distance_y = a.y - b.y;
            float distance_z = a.z - b.z;

            return Mathf.Sqrt(distance_x * distance_x + distance_y * distance_y + distance_z * distance_z);
        }
        public static float Dot(Vec3 a, Vec3 b) // Producto de dos vectores
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        public static Vec3 Lerp(Vec3 a, Vec3 b, float t) // Calcula la interpolacion lineal medante dos puntos y un intermediario,
                                                         // comunmente usado para mover un objeto de un punto a otro en cierto periodo de tiempo
        {
            t = Mathf.Clamp01(t); // Limitar el valor de t de 0 a 1
            return new Vec3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }
        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t) // Igual que lerp esta vez sin clampar t
        {
            return new Vec3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }
        public static Vec3 Max(Vec3 a, Vec3 b) // Calcular el maximo valor de x,y,z entre ambos vectores
        {
            return new Vec3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y) , Mathf.Max(a.z, b.z));
        }
        public static Vec3 Min(Vec3 a, Vec3 b) // Calcular el minimo valor de x,y,z entre ambos vectores
        {
            return new Vec3(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
        }
        public static float SqrMagnitude(Vec3 vector) // Igual que Magnitude pero sin calcular Sqrt
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }
        public static Vec3 Project(Vec3 vector, Vec3 onNormal) // Se calcula la ptoyeccion de "vector" en el plano definido por onNormal
                                                               // El vector resultante representa el vector en la direccion de normal
        {
            float sqrMag = SqrMagnitude(onNormal);
            if (sqrMag < Mathf.Epsilon)
                return new Vec3(0f, 0f, 0f);
            else
                return onNormal * Vec3.Dot(vector, onNormal) / sqrMag;
        }
        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal) // Se calcula la refleccion de inDirection en el plano definido por inNormal
                                                                    // El vector resultante representa el vector en la direcion en la que el objeto rebota con la superficie
        {
            return inDirection - 2 * Vec3.Dot(inDirection, inNormal) * inNormal;
        }
        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }
        public void Scale(Vec3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }
        public static Vec3 Normalize(Vec3 vector)
        {
            return new Vec3(vector.x / vector.magnitude, vector.y / vector.magnitude, vector.z / vector.magnitude);        
        }
        #endregion

        #region Internals
        public override bool Equals(object other)
        {
            if (!(other is Vec3)) return false;
            return Equals((Vec3)other);
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }
        #endregion
    }
}