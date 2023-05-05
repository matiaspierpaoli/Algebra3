using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    public struct Plane
    {
        #region Constructors

        /// <summary>
        /// Se toma una normal y un punto en el plano y se calcula la distancia desde el punto de origen al plano usando multiplicacion de ambos vectores para luego darlo vuelta.
        /// </summary>
        /// <param name="inNormal">Normal del plano.</param>
        /// <param name="inPoint">Punto del plano.</param>
        public Plane(Vec3 inNormal, Vec3 inPoint) 
        {
            normal = inNormal.normalized;
            distance = -Vec3.Dot(inNormal, inPoint);
        }

        /// <summary>
        /// Se toma una normal y una distancia desde un origen, luego se "normaliza" la normal y se calcula la distancia desde el origen al plano simplemente dividiendo por la magnitud.
        /// </summary>
        /// <param name="inNormal">Normal del plano.</param>
        /// <param name="d">Distancia.</param>
        public Plane(Vec3 inNormal, float d) 
        {
            normal = inNormal.normalized;
            distance = d / normal.sqrMagnitude;
        }

        /// <summary>
        /// Se calcula la normal del vector del plano usando producto punto y normalizandolo, y luego la distancia es una multiplicacion de ambos vectores para luego darlo vuelta.
        /// </summary>
        /// <param name="a">Primer punto del espacio.</param>
        /// <param name="b">Segundo punto del espacio.</param>
        /// <param name="c">Tercer punto del espacio.</param>
        public Plane(Vec3 a, Vec3 b, Vec3 c)                                              
        {
            normal = Vec3.Cross(b - a, c - a).normalized;
            distance = -Vec3.Dot(normal, a);
        }
        #endregion

        #region Functions

        /// <summary>
        /// Se crea un plano con la misma normal y distancia que la del parametro plane, y se obtiene la nueva distancia multiplicando el vec3 y la normal del plano para asi "mover" de lugar el plano.
        /// </summary>
        /// <param name="plane">Plano dado.</param>
        /// <param name="translation">Trasalcion.</param>
        /// <returns> El nuevo plano trasladado.</returns>
        public static Plane Translate(Plane plane, Vec3 translation)  
        {
            Plane translatedPlane = new Plane(plane.normal, plane.distance);
            translatedPlane.distance += Vec3.Dot(translation, plane.normal);

            return translatedPlane;
        }

        /// <summary>
        /// Primero se calcula la distancia del punto al plano y con esta distancia se devuelve la resta del punto y la normal por la nueva distancia .
        /// </summary>
        /// <param name="point">Punto del espacio.</param>
        /// <returns> El punto mas cercano en el plano.</returns>
        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            float distanceToPlane = GetDistanceToPoint(point);
            return point - normal * distanceToPlane;
        }

        /// <summary>
        /// Simplemente hace que la normal mire para el otro lado y, al voltear la distancia cambia que lado del plano sea positivo.
        /// </summary>
        public void Flip()
        {
            normal = -normal;
            distance = -distance;
        }


        /// <summary>
        /// Se calcula la distancia del punto al plano
        /// </summary>
        /// <param name="point">El punto del cual se busca la distancia.</param>
        /// <returns> La multiplicacion de la resta del punto y la distancia por la normal, y la normal.</returns>
        public float GetDistanceToPoint(Vec3 point)                                                        
        {
            return Vec3.Dot(point - normal * distance, normal);
        }

        /// <summary>
        /// Se calcula la distancia del punto al plano y en el caso de que esta sea mayor que 0 quiere decir que esta en el lado positivo del plano. 
        /// </summary>
        /// <param name="point">Punto del espacio.</param>
        /// <returns> Verdadero si la distancia es mayor a 0.</returns>
        public bool GetSide(Vec3 point)         
        {
            float distanceToPlane = GetDistanceToPoint(point);
            return distanceToPlane > 0f;
        }

        /// <summary>
        /// Se calculan las distancias de ambos puntos al plano y luego con Mathf.Sign() nos fijamos que signos devuelven.  
        /// </summary>
        /// <param name="inPt0">Primer punto.</param>
        /// <param name="inPt1">Segundo punto.</param>
        /// <returns> Verdadero si ambos signos de las distancias tiene el mismo signo.</returns>
        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            float d0 = GetDistanceToPoint(inPt0);
            float d1 = GetDistanceToPoint(inPt1);

            return Mathf.Sign(d0) == Mathf.Sign(d1);
        }

        /// <summary>
        /// Se calculan dos lados y se obtiene la normal haciendo el producto punto, luego la distancia se obtiene multiplicando la normal por uno de los puntos. 
        /// </summary>
        /// <param name="a">Primer punto del espacio.</param>
        /// <param name="b">Segundo punto del espacio.</param>
        /// <param name="c">Tercer punto del espacio.</param>
        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            Vec3 side1 = b - a;
            Vec3 side2 = c - a;
            
            normal = Vec3.Cross(side1, side2).normalized;
            distance = -Vec3.Dot(normal, a);
        }

        /// <summary>
        /// La nueva normal es la del parametro normalizada y la distancia es el producto negado de la nueva normal y el punto dado como parametro. 
        /// </summary>
        /// <param name="inNormal">Nueva normal.</param>
        /// <param name="inPoint">Punto del espacio.</param>
        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            normal = inNormal.normalized;
            distance = -Vec3.Dot(inNormal.normalized, inPoint);
        }

        /// <summary>
        /// Para mover el plano sin crear uno nuevo simplemente se modifica el valor de distance sumandole el producto de la normal por el vec3 traslacion. 
        /// </summary>
        /// <param name="translation">Traslacion.</param>
        public void Translate(Vec3 translation) 
        {
            distance += Vec3.Dot(normal, translation);
        }
        #endregion

        public Vec3 normal { get; set; }
        public float distance { get; set; }
    }
}

