using UnityEngine;

public class MeshColliderChecker : MonoBehaviour
{
    [SerializeField] MeshCollider[] meshes;
    [SerializeField] Material positiveCollisionMaterial;
    [SerializeField] Material negativeCollisionMaterial;
    [SerializeField] bool areColliding;

    private void Update()
    {
        areColliding = false;

        foreach (var point in meshes[0].pointsInsideMesh)
        {
            if (meshes[1].CheckPointsAgainstAnotherMesh(point))
            {
                areColliding = true;
            }
        }

        if (areColliding)
        {
            meshes[0].GetComponent<MeshRenderer>().material = positiveCollisionMaterial;
            meshes[1].GetComponent<MeshRenderer>().material = positiveCollisionMaterial;
        }
        else
        {
            meshes[0].GetComponent<MeshRenderer>().material = negativeCollisionMaterial;
            meshes[1].GetComponent<MeshRenderer>().material = negativeCollisionMaterial;
        }
    }
}