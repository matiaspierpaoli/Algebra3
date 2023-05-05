using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public struct Plane
    {
        #region Constructors
        public Plane(Vec3 inNormal, Vec3 inPoint) // Se toma una normal y un punto en el plano y se calcula la distancia desde el punto de origen al plano usando multiplicacion de ambos vectores
                                                  // para luego darlo vuelta con el -
        {
            normal = inNormal.normalized;
            distance = -Vec3.Dot(inNormal, inPoint);
        }

        public Plane(Vec3 inNormal, float d) // Se toma una normal y una distancia desde un origen, luego se "normaliza" la normal y se calcula la distancia desde el origen
                                             // al plano simplemente dividiendo por la magnitud.
        {
            normal = inNormal.normalized;
            distance = d / normal.sqrMagnitude;

        }

        public Plane(Vec3 a, Vec3 b, Vec3 c) // Tomando tres puntos en el espacio, se calcula la normal del vector del plano usando producto punto y normalizandolo,
                                             // finalmente se calcula la distancia desde el origen a sobre la normal al plano
        {
            normal = Vec3.Cross(b - a, c - a).normalized;
            distance = -Vec3.Dot(normal, a);
        }
        #endregion

        public Vec3 normal { get; set; }
        public float distance { get; set; }
        //public Plane flipped { get; }
    }
}

