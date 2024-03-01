using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIA : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public State currentState;

    private Transform player;
    private NavMeshAgent agent;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float detectionRange = 10;
    [SerializeField] private float attackRange = 2;

    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }
    void Start()
    {
        SetRandomPoint();
        currentState = State.Patrolling;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            case State.Chasing:
                Chase();
            break;
            case State.Attacking:
                Attack();
            break;
        }
        
    }
    
    void Patrol()
    {
        if(IsInRange(detectionRange) == true)
        {
            currentState = State.Chasing;
        }
        if(agent.remainingDistance < 0.5f)
        {
            SetRandomPoint();
        }
    }

    void Chase()
    {
        if(IsInRange(detectionRange) == false)
        {
            SetRandomPoint();
            currentState = State.Patrolling;
        }
        if(IsInRange(attackRange) == true)
        {
            currentState = State.Attacking;
        }
        agent.destination = player.position;
    }

    void Attack()
    {
        Debug.Log("Atacado!!");
        currentState = State.Chasing;
    }

    void SetRandomPoint()
    {
        agent.destination = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
    }

    bool IsInRange(float range)
    {
        if(Vector3.Distance(transform.position, player.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        foreach (Transform point in patrolPoints)
        {
            Gizmos.DrawWireSphere(point.position, 1f);
        }
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
