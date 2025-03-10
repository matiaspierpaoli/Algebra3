using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Trabajos;

public class MeshCollider : MonoBehaviour
{
    public List<MyPlane> planes; // Plano determinado por triangulos dentro de la mesh
    public bool isMeshActive; // Booleano para dibujar o no la mesh
    public List<Vec3> pointsInsideMesh;
    private List<Vec3> pointsToCheck;
    public Vec3 nearestPoint; // Punto mas cercano de la grilla hacia el objeto 

    /// <summary>
    /// Crear un ray usado para chequear si el punto esta en el plano. 
    /// A�adir un punto de origen y un destino asi como el constructor
    /// </summary>
    struct Ray
    {
        public Vec3 origin;
        public Vec3 dest;

        public Ray(Vec3 origin, Vec3 dest)
        {
            this.origin = origin;
            this.dest = dest;
        }
    }

    /// <summary>
    /// Empieza creando cada plano
    /// Chequea cada 3 vertices para que el plano pueda ser creado usando el constructor de tres Vec3 y lo a�ade a la lista
    /// Usando la normal de la mesh por cada plano se guarda la normal y la posicion para orientarlo
    /// Aux es la normal, obtenida sacando las normales de la mesh, y el punto se obtiene multiplicando la normal por la distancia
    /// </summary>
    private void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        planes = new List<MyPlane>();
        pointsInsideMesh = new List<Vec3>();
        pointsToCheck = new List<Vec3>();

        for (int i = 0; i < mesh.GetIndices(0).Length; i += 3)
        {
            Vec3 auxA = new Vec3(mesh.vertices[mesh.GetIndices(0)[i]]);
            Vec3 auxB = new Vec3(mesh.vertices[mesh.GetIndices(0)[i + 1]]);
            Vec3 auxC = new Vec3(mesh.vertices[mesh.GetIndices(0)[i + 2]]);

            planes.Add(new MyPlane(auxA, auxB, auxC));
        }

        for (int i = 0; i < planes.Count; i++)
        {
            Vec3 aux = new Vec3(mesh.normals[i]);

            planes[i].SetNormalAndPosition(aux, planes[i].normal * planes[i].distance);
        }
    }

    /// <summary>
    /// Se obtiene el componente Mesh. 
    /// Se limpian los planos.
    /// Se guardan los vertices definidos por la mesh para definir los planos.
    /// Despues de darlos vuelta se busca el punto mas cercano de la grilla a todos los planos
    /// Se a�aden todos los puntos a chequar a una lista
    /// Finalmente se calcula que puntos son validos si estan dentro de la mesh
    /// </summary>
    private void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        planes.Clear();

        for (int i = 0; i < mesh.GetIndices(0).Length; i += 3)
        {
            Vec3 vertexA = new Vec3(transform.TransformPoint(mesh.vertices[mesh.GetIndices(0)[i]]));
            Vec3 vertexB = new Vec3(transform.TransformPoint(mesh.vertices[mesh.GetIndices(0)[i + 1]]));
            Vec3 vertexC = new Vec3(transform.TransformPoint(mesh.vertices[mesh.GetIndices(0)[i + 2]]));

            var plane = new MyPlane(vertexA, vertexB, vertexC);
            plane.normal *= -1;
            planes.Add(plane);
        }

        for (int i = 0; i < planes.Count; i++)
        {
            planes[i].Flip();
        }

        NearestPoint();

        AddPointsToCheck();
        Debug.Log("Points: " + pointsToCheck.Count);
        AddPointsInsideOfMesh();

        Debug.Log("Colliding Points: " + pointsInsideMesh.Count);
    }

    /// <summary>
    /// Se obtiene el punto mas cercano chequeando en x, y, z y se guarda como Vec3
    /// </summary>
    private void NearestPoint()
    {
        var x = ValueNearestPosition(transform.position.x);
        var y = ValueNearestPosition(transform.position.y);
        var z = ValueNearestPosition(transform.position.z);

        nearestPoint = Grid.grid[x, y, z];
    }

    /// <summary>
    /// En resumen, la funci�n toma una posici�n flotante, la ajusta a la cuadr�cula m�s cercana (con un redondeo de tipo "round"), 
    /// y asegura que el valor est� dentro del rango v�lido de la cuadr�cula, devolviendo finalmente un entero.
    /// </summary>
    private int ValueNearestPosition(float position)
    {
        float value;

        var aux = position / Grid.gridDelta;

        if (aux - (int)aux > 0.5f)
        {
            value = aux + 1.0f;
        }
        else
        {
            value = aux;
        }

        value = Mathf.Clamp(value, 0, Grid.gridSize - 1);

        return (int)value;
    }

    /// <summary>
    /// Vac�a la lista pointsToCheck de Vec3, y establece el tama�o de la cuadr�cula como el tama�o de la cuadr�cula menos 1.
    /// Luego se calculan el punto m�ximo y m�nimo para x, y, y z usando la funci�n MaxGridSize.
    /// Despu�s, estos 6 puntos son comprobados con la funci�n Clamp para que no excedan el gridSize ni sean menores que 0.
    /// Finalmente, hay un bucle que va desde el valor m�nimo hasta el m�ximo de cada punto x, y, z y agrega el punto a la lista pointsToCheck.
    /// </summary>
    private void AddPointsToCheck()
    {
        pointsToCheck.Clear();

        int maxPointX = MaxGridSize(nearestPoint.x, transform.localScale.x, 3, 1);
        int maxPointY = MaxGridSize(nearestPoint.y, transform.localScale.y, 3, 1);
        int maxPointZ = MaxGridSize(nearestPoint.z, transform.localScale.z, 3, 1);
        int minPointX = MaxGridSize(nearestPoint.x, transform.localScale.x, 3, -1);
        int minPointY = MaxGridSize(nearestPoint.y, transform.localScale.y, 3, -1);
        int minPointZ = MaxGridSize(nearestPoint.z, transform.localScale.z, 3, -1);

        var gridSize = Grid.gridSize - 1;

        maxPointX = Mathf.Clamp(maxPointX, 0, gridSize);
        maxPointY = Mathf.Clamp(maxPointY, 0, gridSize);
        maxPointZ = Mathf.Clamp(maxPointZ, 0, gridSize);
        minPointX = Mathf.Clamp(minPointX, 0, gridSize);
        minPointY = Mathf.Clamp(minPointY, 0, gridSize);
        minPointZ = Mathf.Clamp(minPointZ, 0, gridSize);

        for (int x = minPointX; x < maxPointX; x++)
        {
            for (int y = minPointY; y < maxPointY; y++)
            {
                for (int z = minPointZ; z < maxPointZ; z++)
                {
                    pointsToCheck.Add(Grid.grid[x, y, z]);
                }
            }
        }
    }
    /// <summary>
    /// Esta funci�n necesita el nearestPoint, la escala para que funcione con diferentes escalas, el n�mero de puntos que se comprobar�n y un valor para verificar si es positivo o negativo.
    /// Una vez que tiene todos estos, convierte nearestPoint en un entero.
    /// Y obtiene la suma entre el n�mero de puntos que se comprobar�n y la multiplicaci�n entre la escala y el signo, que determinar� si es positivo o negativo.
    /// Como valor de retorno, ambos enteros se suman.
    /// </summary>
    private int MaxGridSize(float nearestPoint, float scale, int numberOfPoints, int signOfTheNumber)
    {
        int x = (int)nearestPoint;

        int y = (numberOfPoints + (int)scale - 1) * signOfTheNumber;

        return x + y;
    }

    /// <summary>
    /// Vac�a la lista pointsInsideMesh de Vec3.
    /// Comienza un bucle donde se comprueba cada punto de pointsToCheck.
    /// Se utiliza el constructor Ray para trazar un rayo desde el punto hacia un Vec3 hacia adelante con una longitud arbitraria.
    /// Se declara un contador, que luego se utilizar�.
    /// Comienza otro bucle que comprueba cada plano en la lista de planos.
    /// Comprueba si el punto est� en el plano usando la funci�n IsPointInPlane.
    /// Comprueba si el plano es v�lido con la funci�n IsValePlane, y si lo es, el contador aumenta en uno.
    /// Si el contador es impar, el punto se agrega a la lista pointsInsideMesh.
    /// </summary>
    private void AddPointsInsideOfMesh()
    {
        pointsInsideMesh.Clear();

        foreach (var point in pointsToCheck)
        {
            Ray pointRay = new Ray(point, Vec3.Forward * 10f);

            var counter = 0;

            foreach (var plane in planes)
            {
                if (IsPointInPlane(plane, pointRay, out Vec3 t))
                {
                    if (IsValidPlane(plane, t))
                    {
                        counter++;
                    }
                }
            }
            Debug.Log("Counter is: " + counter);
            // Si es par, significa que el punto empieza fuera del plano y atraviesa a trav�s de �l cruzando un n�mero par de veces
            // Si es impar, el punto empieza dentro porque solo cruza un n�mero impar de veces hasta salir
            if (counter % 2 == 1)
            {
                pointsInsideMesh.Add(point);
            }
        }
    }

    /// <summary>
    /// La funci�n pide un plano, un rayo y un punto.
    /// La funci�n usa la palabra clave out en uno de los par�metros para que el programa sepa que la variable ser� inicializada y asignada dentro de la funci�n.
    /// El punto se establece a cero porque necesitaremos el punto de intersecci�n.
    /// El valor del denominador es el producto punto entre la normal y el destino del rayo, porque esto indicar� cu�n cercanos est�n ambos.
    /// Si el valor absoluto del denominador es mayor que epsilon, contin�a; de lo contrario, ser�an perpendiculares.
    /// Luego se crean dos variables auxiliares para su uso posterior, la primera es la multiplicaci�n entre la normal y la distancia.
    /// Estas dos se multiplican para dar la distancia con signo desde un punto hasta el plano.
    /// Si la distancia es positiva, el punto est� del lado del plano donde apunta el vector normal, si es negativa, estar�a en el lado contrario.
    /// La segunda hace la resta entre ese valor y la distancia, esto traduce la distancia para ser relativa al punto.
    /// Luego se hace el producto punto nuevamente entre aux2 y la normal, lo que da como resultado la distancia firmada desde el punto de origen hasta el plano.
    /// Si esto dividido por el denominador es mayor o igual a epsilon, entonces el punto est� dentro del plano o muy cerca, porque usamos epsilon.
    /// Finalmente, el punto se iguala al origen del rayo con el destino escalado.
    /// Como resultado, este punto estar� ubicado a "t" unidades del origen en la direcci�n del destino del rayo.
    /// </summary>
    private bool IsPointInPlane(MyPlane plane, Ray pointRay, out Vec3 point)
    {
        point = Vec3.Zero;

        float denominator = Vec3.Dot(plane.normal, pointRay.dest);

        if (Mathf.Abs(denominator) > Vec3.epsilon)
        {
            Vec3 aux = plane.normal * plane.distance;
            Vec3 aux2 = aux - pointRay.origin;

            float t = Vec3.Dot(aux2, plane.normal) / denominator;

            if (t >= Vec3.epsilon)
            {
                point = pointRay.origin + pointRay.dest * t;

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Comprobar� si el plano es v�lido, pide un plano y un punto.
    /// Comienza definiendo 6 flotantes, 3 para x y 3 para y.
    /// Luego calcula el �rea de un tri�ngulo formado por los 6 flotantes usando la ecuaci�n de colisi�n de tri�ngulos con un punto.
    /// http://www.jeffreythompson.org/collision-detection/tri-point.php#:~:text=To%20test%20if%20a%20point,the%20corners%20of%20the%20triangle
    /// F�rmula de Her�n = Abs((y1 - x1) * (z2 - x2) - (z1 - x1) * (y2 - x2)) = flotante
    /// Esto dar� el �rea del tri�ngulo.
    /// �rea del tri�ngulo formado con un punto = Abs((x1 - point.x) * (y2 - point.x) - (x2 - point.x) * (y1 - point.x)) = flotante
    /// Si la suma de las �reas es igual a la original, sabemos que estamos dentro del tri�ngulo, de lo contrario no lo estamos
    /// </summary>
    private bool IsValidPlane(MyPlane plane, Vec3 point)
    {
        float x1 = plane.pointA.x;
        float x2 = plane.pointB.x;
        float x3 = plane.pointC.x;

        float y1 = plane.pointA.y;
        float y2 = plane.pointB.y;
        float y3 = plane.pointC.y;

        float completeArea = Mathf.Abs((x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1));

        float firstArea = Mathf.Abs((x1 - point.x) * (y2 - point.y) - (x2 - point.x) * (y1 - point.y));
        float secondArea = Mathf.Abs((x2 - point.x) * (y3 - point.y) - (x3 - point.x) * (y2 - point.y));
        float thirdArea = Mathf.Abs((x3 - point.x) * (y1 - point.y) - (x1 - point.x) * (y3 - point.y));

        return Mathf.Abs(firstArea + secondArea + thirdArea - completeArea) < Vec3.epsilon;
    }

    /// <summary>
    /// Esta funci�n pide un punto Vec3.
    /// Compara si el punto dado est� en el otro mesh tambi�n.
    /// La funci�n devolver� true si es as�.
    /// Utiliza la funci�n Any(), que verifica si alg�n elemento en la colecci�n cumple con una condici�n espec�fica, como pointsInsideMesh.
    /// La expresi�n lambda comparar� cada elemento en pointsInsideMesh con el punto para ver cu�l es igual
    /// </summary>
    public bool CheckPointsAgainstAnotherMesh(Vec3 point)
    {
        return pointsInsideMesh.Any(pointsInsideMesh => pointsInsideMesh == point);
    }

    private void OnDrawGizmos()
    {
        if (isMeshActive)
        {
            var color = Color.blue;

            foreach (var plane in planes)
            {
                DrawPlane(plane.normal * plane.distance, plane.normal, color);
            }

            foreach (var point in pointsInsideMesh)
            {
                Gizmos.DrawRay(point, Vec3.Forward * 10f);
            }
        }
    }

    private void DrawPlane(Vec3 pos, Vec3 normal, Color color)
    {
        Vec3 vector;

        if (normal.normalized != Vec3.Forward)
        {
            vector = Vec3.Cross(normal, Vec3.Forward).normalized * normal.magnitude;
        }
        else
        {
            vector = Vec3.Cross(normal, Vec3.Up).normalized * normal.magnitude;
        }

        var corner = pos + vector;
        var corner2 = pos - vector;

        var rot = Quat.AngleAxis(90.0f, normal);

        vector = rot * vector;

        var corner1 = pos + vector;
        var corner3 = pos - vector;

        Debug.DrawLine(corner, corner2, color);
        Debug.DrawLine(corner1, corner3, color);
        Debug.DrawLine(corner, corner1, color);
        Debug.DrawLine(corner1, corner2, color);
        Debug.DrawLine(corner2, corner3, color);
        Debug.DrawLine(corner3, corner, color);

        Debug.DrawRay(pos, normal, Color.cyan);
    }
}
