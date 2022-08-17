using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform.gameObject;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void OnEnable()
    {
        StartCoroutine(FollowPlayer());
    }

    WaitForSeconds pathfindingWait = new WaitForSeconds(.3f);
    IEnumerator FollowPlayer()
    {
        while(true)
        {
            yield return pathfindingWait;
            agent.SetDestination(player.transform.position);
        }
    }

}
