using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Detect,
        Chase
    }

    public EnemyState enemyState;
    [SerializeField] private PatrolPath patrolPath;
    [SerializeField] private float idleTimer;
    [SerializeField] private float detectTimer;
    [SerializeField] private ConeDetector coneDetector;
    private Transform player;
    [SerializeField] private float sprintSpeed;
    private Coroutine detectCoroutine;
    private Coroutine idleCoroutine;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 localVelocity;
    private readonly string movementSpeedID = "MovementSpeed";
    private int wayPointsIndex;
    private bool isIdleCoroutineRunning;
    private bool isDetectCoroutineRunning;
    private float walkSpeed;
    public float GetDetectTimer() => detectTimer;
    private bool isPlayerCaught;

    private void Awake()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        walkSpeed = navMeshAgent.speed;

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        coneDetector.OnPlayerDetected += OnPlayerFound;
    }

    private void OnDisable()
    {
        coneDetector.OnPlayerDetected -= OnPlayerFound;
    }

    private void Start()
    {
        if (patrolPath != null)
        {
            navMeshAgent.SetDestination(patrolPath.GetWayPoint(wayPointsIndex));
        }

        player = GameManager.Instance.Player;

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Patrol:
                PatrolState();
                break;

            case EnemyState.Chase:
                ChaseState();
                break;

            case EnemyState.Detect:
                DetectState();
                break;
        }

        UpdateAnimator();
    }

    private void OnPlayerFound(bool playerFound)
    {
        StopActiveCoroutines();

        if (playerFound)
        {
            enemyState = EnemyState.Detect;
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.SetDestination(patrolPath.GetWayPoint(wayPointsIndex));
            enemyState = EnemyState.Patrol;
            navMeshAgent.isStopped = false;
        }
    }

    private void DetectState()
    {
        if (isDetectCoroutineRunning) return;
        isDetectCoroutineRunning = true;
        detectCoroutine = StartCoroutine(DetectCoroutine());
    }

    private IEnumerator DetectCoroutine()
    {
        yield return new WaitForSeconds(detectTimer);
        enemyState = EnemyState.Chase;
        navMeshAgent.isStopped = false;
        isDetectCoroutineRunning = false;
    }

    private void PatrolState()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.speed = walkSpeed;
            enemyState = EnemyState.Idle;
            navMeshAgent.isStopped = true;
        }
    }

    private void IdleState()
    {
        if (isIdleCoroutineRunning) return;
        isIdleCoroutineRunning = true;
        idleCoroutine = StartCoroutine(IdleStateTime());
    }

    private IEnumerator IdleStateTime()
    {
        yield return new WaitForSeconds(idleTimer);
        enemyState = EnemyState.Patrol;
        navMeshAgent.isStopped = false;
        isIdleCoroutineRunning = false;
        GoToNextWayPoint();
    }

    private void ChaseState()
    {
        if (!player) return;
        navMeshAgent.SetDestination(player.transform.position);
        navMeshAgent.speed = sprintSpeed;
        if (!navMeshAgent.pathPending &&
            navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            !isPlayerCaught)
        {
            GameManager.Instance.ShowLose();
            isPlayerCaught = true;
        }
    }

    private void GoToNextWayPoint()
    {
        wayPointsIndex = patrolPath.GetNextIndex(wayPointsIndex);
        navMeshAgent.SetDestination(patrolPath.GetWayPoint(wayPointsIndex));
    }

    private void StopActiveCoroutines()
    {
        if (detectCoroutine != null)
        {
            StopCoroutine(detectCoroutine);
            isDetectCoroutineRunning = false;
            detectCoroutine = null;
        }

        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            isIdleCoroutineRunning = false;
            idleCoroutine = null;
        }
    }

    void UpdateAnimator()
    {
        localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
        animator.SetFloat(movementSpeedID, localVelocity.z);
    }
}