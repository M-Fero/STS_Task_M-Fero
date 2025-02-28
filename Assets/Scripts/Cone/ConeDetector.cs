using System;
using UnityEngine;

public class ConeDetector : MonoBehaviour
{
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private LayerMask obstacleMask;
    private Transform player;
    public Action<bool> OnPlayerDetected;
    private bool playerPreviouslyDetected;
    public float ViewRadius => viewRadius;
    public float ViewAngle => viewAngle;

    private void Start()
    {
        player = GameManager.Instance.Player;

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }


    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        bool playerDetected = IsWithinViewRadius() && IsWithinViewAngle() && HasLineOfSight();

        if (playerDetected != playerPreviouslyDetected)
        {
            playerPreviouslyDetected = playerDetected;
            OnPlayerDetected?.Invoke(playerDetected);

            Debug.DrawLine(transform.position, player.position, playerDetected ? Color.green : Color.red, 0.5f);
        }
    }

    private bool IsWithinViewRadius()
    {
        return Vector3.Distance(transform.position, player.position) <= viewRadius;
    }

    private bool IsWithinViewAngle()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2;
    }

    private bool HasLineOfSight()
    {
        return !Physics.Linecast(transform.position, player.position, obstacleMask);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}