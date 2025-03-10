using UnityEngine;
using Trabajos;

public class Grid : MonoBehaviour
{
    [Header("Customize")]
    [SerializeField] public bool isGridActive = false;

    [SerializeField] public static int gridSize = 10;
    [SerializeField] public static float gridDelta = 1f;
    [SerializeField] public static Vec3[,,] grid = new Vec3[gridSize, gridSize, gridSize];

    void Awake()
    {

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int k = 0; k < grid.GetLength(2); k++)
                {
                    grid[i,j,k] = new Vec3(i,j,k) * gridDelta;
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int k = 0; k < grid.GetLength(2); k++)
                {
                    Gizmos.DrawSphere(new Vec3(i, j, k) * gridDelta, 0.1f);
                }
            }
        }
    }
}
