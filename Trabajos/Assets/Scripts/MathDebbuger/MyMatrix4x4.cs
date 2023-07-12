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

        private Quat GetRotation()
        {
            Vec3 forward = new Vec3(m02, m12, m22).normalized; 
            Vec3 upwards = new Vec3(m01, m11, m21).normalized; 

            return Quat.LookRotation(forward, upwards);
        }

        #endregion

        #region Methods




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