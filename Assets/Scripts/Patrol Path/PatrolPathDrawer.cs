using System.Linq;
using UnityEngine;

public class PatrolPathDrawer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Transform[] waypoints = GetComponentsInChildren<Transform>()
            .Where(t => t != transform) // Exclude parent
            .ToArray();

        // Ensure there are enough waypoints to draw a line
        if (waypoints.Length < 2) return;

        Gizmos.color = Color.green;

        for (int i = 1; i < waypoints.Length; i++)
        {
            Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
        }

        Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);


        // Optionally, draw a line from the last to the first to create a loop
        Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[1].position);

        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);
        }
    }
}