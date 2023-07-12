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

        public static MyMatrix4x4 LookAt(Vec3 from, Vec3 to, Vec3 up) // Se calculan las matrices rotacion y traslacion y se multiplican para devolver esa matriz
                                                                      // Sirve para alinear un objeto, comunmente camaras hacia un objetivo especifico
        {
            Vec3 forward = (to - from).normalized;
            Vec3 right = Vec3.Cross(up, forward).normalized;
            Vec3 actualUp = Vec3.Cross(forward, right).normalized;

            MyMatrix4x4 rotation = new MyMatrix4x4();
            rotation.m00 = right.x;
            rotation.m01 = right.y;
            rotation.m02 = right.z;
            rotation.m03 = 0f;

            rotation.m10 = actualUp.x;
            rotation.m11 = actualUp.y;
            rotation.m12 = actualUp.z;
            rotation.m13 = 0f;

            rotation.m20 = -forward.x;
            rotation.m21 = -forward.y;
            rotation.m22 = -forward.z;
            rotation.m23 = 0f;

            rotation.m30 = 0f;
            rotation.m31 = 0f;
            rotation.m32 = 0f;
            rotation.m33 = 1f;

            MyMatrix4x4 translation = new MyMatrix4x4();
            translation.m00 = 1f;
            translation.m01 = 0f;
            translation.m02 = 0f;
            translation.m03 = -from.x;

            translation.m10 = 0f;
            translation.m11 = 1f;
            translation.m12 = 0f;
            translation.m13 = -from.y;

            translation.m20 = 0f;
            translation.m21 = 0f;
            translation.m22 = 1f;
            translation.m23 = -from.z;

            translation.m30 = 0f;
            translation.m31 = 0f;
            translation.m32 = 0f;
            translation.m33 = 1f;

            MyMatrix4x4 result = rotation * translation;

            return result;
        }

        public static MyMatrix4x4 Rotate(Quat q) // Devuelve una matriz de rotacion, cada elemento de la matriz se obtiene operando sobre los componentes del quaternion
                                                 // Sirve para rotar objetos en el espacio continuamente a la hora de generar graficos 3D, por ejemplo en la Matriz RTS
        {
            MyMatrix4x4 result;

            float xx = q.x * q.x;
            float xy = q.x * q.y;
            float xz = q.x * q.z;
            float xw = q.x * q.w;

            float yy = q.y * q.y;
            float yz = q.y * q.z;
            float yw = q.y * q.w;

            float zz = q.z * q.z;
            float zw = q.z * q.w;

            result.m00 = 1f - 2f * (yy + zz);
            result.m01 = 2f * (xy - zw);
            result.m02 = 2f * (xz + yw);
            result.m03 = 0f;

            result.m10 = 2f * (xy + zw);
            result.m11 = 1f - 2f * (xx + zz);
            result.m12 = 2f * (yz - xw);
            result.m13 = 0f;

            result.m20 = 2f * (xz - yw);
            result.m21 = 2f * (yz + xw);
            result.m22 = 1f - 2f * (xx + yy);
            result.m23 = 0f;

            result.m30 = 0f;
            result.m31 = 0f;
            result.m32 = 0f;
            result.m33 = 1f;

            return result;
        }

        public static MyMatrix4x4 Scale(Vec3 vector) // Se escalan los elementos en base a un parametro en forma de diagonal desde top-left hasta bottom-right
                                                     // Sirve para escalar objetos en el espacio continuamente a la hora de generar graficos 3D, por ejemplo en la Matriz RTS
        {
            MyMatrix4x4 result;

            result.m00 = vector.x;
            result.m01 = 0f;
            result.m02 = 0f;
            result.m03 = 0f;

            result.m10 = 0f;
            result.m11 = vector.y;
            result.m12 = 0f;
            result.m13 = 0f;

            result.m20 = 0f;
            result.m21 = 0f;
            result.m22 = vector.z;
            result.m23 = 0f;

            result.m30 = 0f;
            result.m31 = 0f;
            result.m32 = 0f;
            result.m33 = 1f;

            return result;
        }

        public static MyMatrix4x4 Translate(Vec3 vector) // Los elementos de la matriz de traslacion resultante en la ultima columna se remplazan por el vector designado  
                                                          // Sirve para trasladar objetos en el espacio continuamente a la hora de generar graficos 3D, por ejemplo en la Matriz RTS
        {
            MyMatrix4x4 result;

            result.m00 = 1f;
            result.m01 = 0f;
            result.m02 = 0f;
            result.m03 = vector.x;

            result.m10 = 0f;
            result.m11 = 1f;
            result.m12 = 0f;
            result.m13 = vector.y;

            result.m20 = 0f;
            result.m21 = 0f;
            result.m22 = 1f;
            result.m23 = vector.z;

            result.m30 = 0f;
            result.m31 = 0f;
            result.m32 = 0f;
            result.m33 = 1f;

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

        public static MyMatrix4x4 TRS(Vec3 pos, Quat q, Vec3 s) // Se devuelve una matriz de transformacion que se calcula a partir de la trasalcion, rotatacion y escala en este order
        {
            MyMatrix4x4 translationMatrix = Translate(pos);
            MyMatrix4x4 rotationMatrix = Rotate(q);
            MyMatrix4x4 scaleMatrix = Scale(s);

            MyMatrix4x4 result = translationMatrix * rotationMatrix * scaleMatrix;

            return result;
        }

        public Vector4 GetColumn(int index) // Se devuelve un vector4 que se compone de los valores de la columna dependiente del indice pasado por parametro
        {
            Vector4 column;

            if (index == 0)
            {
                column.x = m00;
                column.y = m10;
                column.z = m20;
                column.w = m30;
            }
            else if (index == 1)
            {
                column.x = m01;
                column.y = m11;
                column.z = m21;
                column.w = m31;
            }
            else if (index == 2)
            {
                column.x = m02;
                column.y = m12;
                column.z = m22;
                column.w = m32;
            }
            else if (index == 3)
            {
                column.x = m03;
                column.y = m13;
                column.z = m23;
                column.w = m33;
            }
            else
            {
                column = Vector4.zero;
            }

            return column;
        }

        public static Vec3 GetPosition(MyMatrix4x4 matrix) // La posicion es dependiente de la cuarta columna de la matriz designada
        {
            Vec3 position;

            position.x = matrix.m03;
            position.y = matrix.m13;
            position.z = matrix.m23;

            return position;
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

        #region Operators

        public static Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector)
        {
            Vector4 result;

            result.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w;
            result.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w;
            result.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w;
            result.w = lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w;

            return result;
        }

        public static MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
        {
            MyMatrix4x4 result;

            result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
            result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
            result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
            result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;

            result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
            result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
            result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
            result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;

            result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
            result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
            result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
            result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;

            result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
            result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
            result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
            result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;

            return result;
        }


        #endregion
    }
}