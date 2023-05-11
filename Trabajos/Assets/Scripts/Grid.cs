using UnityEngine;
using CustomMath;

public class Grid : MonoBehaviour
{
    [Header("Customize")]
    [SerializeField] private float delta = 0.1f;
    [SerializeField] private int size = 10;

    private static float gridDelta;
    private static int gridSize;
    private Vec3[,,] grid = new Vec3[gridSize, gridSize, gridSize];

    // Start is called before the first frame update
    void Start()
    {
        gridDelta = delta;
        gridSize = size;

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
