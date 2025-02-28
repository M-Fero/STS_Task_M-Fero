using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeVisualizer : MonoBehaviour
{
    private ConeDetector coneDetector;
    private float viewRadius;
    private float viewAngle;
    private MeshFilter meshFilter;
    private Mesh coneMesh;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        coneDetector = GetComponent<ConeDetector>();
        coneMesh = new Mesh();
        meshFilter.mesh = coneMesh;
        viewRadius = coneDetector.ViewRadius;
        viewAngle = coneDetector.ViewAngle;
    }

    void Update()
    {
        UpdateConeMesh();
    }

    private void UpdateConeMesh()
    {
        Vector3[] vertices = new Vector3[3];
        int[] triangles = { 0, 1, 2 };

        vertices[0] = Vector3.zero;
        vertices[1] = GetConeEdgeDirection(-viewAngle / 2) * viewRadius;
        vertices[2] = GetConeEdgeDirection(viewAngle / 2) * viewRadius;

        coneMesh.Clear();
        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.RecalculateNormals();
    }

    private Vector3 GetConeEdgeDirection(float angle)
    {
        return Quaternion.Euler(0, angle, 0) * Vector3.forward;
    }
}