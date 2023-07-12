using System;
using UnityEngine;

namespace CustomMath
{
    public struct MyMatrix4x4
    {
        #region Variables
        float m00;
        float m01;
        float m02;
        float m03;

        float m10;
        float m11;
        float m12;
        float m13;

        float m20;
        float m21;
        float m22;
        float m23;

        float m30;
        float m31;
        float m32;
        float m33;
        #endregion

        #region Constructor
        public MyMatrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
        {
            m00 = column0.x;
            m10 = column0.y;
            m20 = column0.z;
            m30 = column0.w;

            m01 = column1.x;
            m11 = column1.y;
            m21 = column1.z;
            m31 = column1.w;

            m02 = column2.x;
            m12 = column2.y;
            m22 = column2.z;
            m32 = column2.w;

            m03 = column3.x;
            m13 = column3.y;
            m23 = column3.z;
            m33 = column3.w;
        }
        #endregion

        #region Indexers
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m00;
                    case 1: return m01;
                    case 2: return m02;
                    case 3: return m03;
                    case 4: return m10;
                    case 5: return m11;
                    case 6: return m12;
                    case 7: return m13;
                    case 8: return m20;
                    case 9: return m21;
                    case 10: return m22;
                    case 11: return m23;
                    case 12: return m30;
                    case 13: return m31;
                    case 14: return m32;
                    case 15: return m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: m00 = value; break;
                    case 1: m01 = value; break;
                    case 2: m02 = value; break;
                    case 3: m03 = value; break;
                    case 4: m10 = value; break;
                    case 5: m11 = value; break;
                    case 6: m12 = value; break;
                    case 7: m13 = value; break;
                    case 8: m20 = value; break;
                    case 9: m21 = value; break;
                    case 10: m22 = value; break;
                    case 11: m23 = value; break;
                    case 12: m30 = value; break;
                    case 13: m31 = value; break;
                    case 14: m32 = value; break;
                    case 15: m33 = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public float this[int row, int column]
        {
            get => GetRow(this, row)[column];
            set => this[row + column * 4] = value;
        }

        #endregion

        #region Properties

        public static MyMatrix4x4 zero { get; } = new MyMatrix4x4();
        public static MyMatrix4x4 identity { get; } = new MyMatrix4x4{ m00 = 1.0f, m11 = 1.0f, m22 = 1.0f, m33 = 1.0f};

        public Quat rotation => GetRotation();
        public Vec3 lossyScale => GetLossyScale();
        public bool isIdentity => IsIdentity();
        public float determinant => Determinant(this);
        public MyMatrix4x4 transpose => Transpose(this);
        public MyMatrix4x4 inverse => Inverse(this);

        private Quat GetRotation()
        {
            Vec3 forward = new Vec3(m02, m12, m22).normalized; 
            Vec3 upwards = new Vec3(m01, m11, m21).normalized; 

            return Quat.LookRotation(forward, upwards);
        }

        private Vec3 GetLossyScale() // Calcula la escala al extraer las magnitudes en forma de vector3 de cada columna
        {
            Vec3 scale;
            scale.x = new Vec3(m00, m10, m20).magnitude;
            scale.y = new Vec3(m01, m11, m21).magnitude;
            scale.z = new Vec3(m02, m12, m22).magnitude;
            return scale;
        }

        private bool IsIdentity() // Chequear si la matrix no contiene transformaciones
        {
            return m00 == 1f && m01 == 0f && m02 == 0f && m03 == 0f &&
                   m10 == 0f && m11 == 1f && m12 == 0f && m13 == 0f &&
                   m20 == 0f && m21 == 0f && m22 == 1f && m23 == 0f &&
                   m30 == 0f && m31 == 0f && m32 == 0f && m33 == 1f;
        }



        #endregion

        #region Methods


        private float Determinant(MyMatrix4x4 m) // Calcular el valor escalar asociado con una matriz "cuadrada", es decir, 2x2 o 3x3, etc
        {
            float det = 0f;

            float subDet1 = m.m00 * (m.m11 * (m.m22 * m.m33 - m.m23 * m.m32) - m.m12 * (m.m21 * m.m33 - m.m23 * m.m31) + m.m13 * (m.m21 * m.m32 - m.m22 * m.m31));
            float subDet2 = m.m01 * (m.m10 * (m.m22 * m.m33 - m.m23 * m.m32) - m.m12 * (m.m20 * m.m33 - m.m23 * m.m30) + m.m13 * (m.m20 * m.m32 - m.m22 * m.m30));
            float subDet3 = m.m02 * (m.m10 * (m.m21 * m.m33 - m.m23 * m.m31) - m.m11 * (m.m20 * m.m33 - m.m23 * m.m30) + m.m13 * (m.m20 * m.m31 - m.m21 * m.m30));
            float subDet4 = m.m03 * (m.m10 * (m.m21 * m.m32 - m.m22 * m.m31) - m.m11 * (m.m20 * m.m32 - m.m22 * m.m30) + m.m12 * (m.m20 * m.m31 - m.m21 * m.m30));

            det = subDet1 - subDet2 + subDet3 - subDet4;

            return det;
        }

        public static MyMatrix4x4 Inverse(MyMatrix4x4 m) // La matriz devuelta representa la transformacion a la inversa a matriz original,
                                                         // cosa de que cuando a la matriz original se le multiplica la inversa se devuelve identity
        {
            MyMatrix4x4 result;

            float det = m.determinant;

            if (det != 0)
            {
                float invDet = 1f / det;

                result.m00 = (m.m11 * m.m22 * m.m33 - m.m11 * m.m23 * m.m32 - m.m21 * m.m12 * m.m33 +
                              m.m21 * m.m13 * m.m32 + m.m31 * m.m12 * m.m23 - m.m31 * m.m13 * m.m22) * invDet;
                result.m01 = (-m.m01 * m.m22 * m.m33 + m.m01 * m.m23 * m.m32 + m.m21 * m.m02 * m.m33 -
                              m.m21 * m.m03 * m.m32 - m.m31 * m.m02 * m.m23 + m.m31 * m.m03 * m.m22) * invDet;
                result.m02 = (m.m01 * m.m12 * m.m33 - m.m01 * m.m13 * m.m32 - m.m11 * m.m02 * m.m33 +
                              m.m11 * m.m03 * m.m32 + m.m31 * m.m02 * m.m13 - m.m31 * m.m03 * m.m12) * invDet;
                result.m03 = (-m.m01 * m.m12 * m.m23 + m.m01 * m.m13 * m.m22 + m.m11 * m.m02 * m.m23 -
                              m.m11 * m.m03 * m.m22 - m.m21 * m.m02 * m.m13 + m.m21 * m.m03 * m.m12) * invDet;

                result.m10 = (-m.m10 * m.m22 * m.m33 + m.m10 * m.m23 * m.m32 + m.m20 * m.m12 * m.m33 -
                              m.m20 * m.m13 * m.m32 - m.m30 * m.m12 * m.m23 + m.m30 * m.m13 * m.m22) * invDet;
                result.m11 = (m.m00 * m.m22 * m.m33 - m.m00 * m.m23 * m.m32 - m.m20 * m.m02 * m.m33 +
                              m.m20 * m.m03 * m.m32 + m.m30 * m.m02 * m.m23 - m.m30 * m.m03 * m.m22) * invDet;
                result.m12 = (-m.m00 * m.m12 * m.m33 + m.m00 * m.m13 * m.m32 + m.m10 * m.m02 * m.m33 -
                              m.m10 * m.m03 * m.m32 - m.m30 * m.m02 * m.m13 + m.m30 * m.m03 * m.m12) * invDet;
                result.m13 = (m.m00 * m.m12 * m.m23 - m.m00 * m.m13 * m.m22 - m.m10 * m.m02 * m.m23 +
                              m.m10 * m.m03 * m.m22 + m.m20 * m.m02 * m.m13 - m.m20 * m.m03 * m.m12) * invDet;

                result.m20 = (m.m10 * m.m21 * m.m33 - m.m10 * m.m23 * m.m31 - m.m20 * m.m11 * m.m33 +
                              m.m20 * m.m13 * m.m31 + m.m30 * m.m11 * m.m23 - m.m30 * m.m13 * m.m21) * invDet;
                result.m21 = (-m.m00 * m.m21 * m.m33 + m.m00 * m.m23 * m.m31 + m.m20 * m.m01 * m.m33 -
                              m.m20 * m.m03 * m.m31 - m.m30 * m.m01 * m.m23 + m.m30 * m.m03 * m.m21) * invDet;
                result.m22 = (m.m00 * m.m11 * m.m33 - m.m00 * m.m13 * m.m31 - m.m10 * m.m01 * m.m33 +
                              m.m10 * m.m03 * m.m31 + m.m30 * m.m01 * m.m13 - m.m30 * m.m03 * m.m11) * invDet;
                result.m23 = (-m.m00 * m.m11 * m.m23 + m.m00 * m.m13 * m.m21 + m.m10 * m.m01 * m.m23 -
                              m.m10 * m.m03 * m.m21 - m.m20 * m.m01 * m.m13 + m.m20 * m.m03 * m.m11) * invDet;

                result.m30 = (-m.m10 * m.m21 * m.m32 + m.m10 * m.m22 * m.m31 + m.m20 * m.m11 * m.m32 -
                              m.m20 * m.m12 * m.m31 - m.m30 * m.m11 * m.m22 + m.m30 * m.m12 * m.m21) * invDet;
                result.m31 = (m.m00 * m.m21 * m.m32 - m.m00 * m.m22 * m.m31 - m.m20 * m.m01 * m.m32 +
                              m.m20 * m.m02 * m.m31 + m.m30 * m.m01 * m.m22 - m.m30 * m.m02 * m.m21) * invDet;
                result.m32 = (-m.m00 * m.m11 * m.m32 + m.m00 * m.m12 * m.m31 + m.m10 * m.m01 * m.m32 -
                              m.m10 * m.m02 * m.m31 - m.m30 * m.m01 * m.m12 + m.m30 * m.m02 * m.m11) * invDet;
                result.m33 = (m.m00 * m.m11 * m.m22 - m.m00 * m.m12 * m.m21 - m.m10 * m.m01 * m.m22 +
                              m.m10 * m.m02 * m.m21 + m.m20 * m.m01 * m.m12 - m.m20 * m.m02 * m.m11) * invDet;
            }
            else
            {
                result = MyMatrix4x4.identity;
            }

            return result;
        }

        public static MyMatrix4x4 Transpose(MyMatrix4x4 m) // Intercambia los valores de las filas por los de las columnas
                                                           // Ejemplo -> A = | 1 2 3 |   -> | 1 4 7 |
                                                           //                | 4 5 6 |      | 2 5 8 |
                                                           //                | 7 8 9 |      | 3 6 9 |
        {
            MyMatrix4x4 result;

            result.m00 = m.m00;
            result.m01 = m.m10;
            result.m02 = m.m20;
            result.m03 = m.m30;

            result.m10 = m.m01;
            result.m11 = m.m11;
            result.m12 = m.m21;
            result.m13 = m.m31;

            result.m20 = m.m02;
            result.m21 = m.m12;
            result.m22 = m.m22;
            result.m23 = m.m32;

            result.m30 = m.m03;
            result.m31 = m.m13;
            result.m32 = m.m23;
            result.m33 = m.m33;

            return result;
        }

        public static Vector4 GetRow(MyMatrix4x4 matrix, int index)
        {
            switch (index)
            {
                case 0: return new Vector4(matrix.m00, matrix.m01, matrix.m02, matrix.m03);
                case 1: return new Vector4(matrix.m10, matrix.m11, matrix.m12, matrix.m13);
                case 2: return new Vector4(matrix.m20, matrix.m21, matrix.m22, matrix.m23);
                case 3: return new Vector4(matrix.m30, matrix.m31, matrix.m32, matrix.m33);
                default:
                    throw new IndexOutOfRangeException("Invalid matrix row index!");
            }
        }

        #endregion

    }
}