using UnityEngine;
using System.Collections.Generic;

public class PetMovement : MonoBehaviour
{
    public List<Transform> waypoints; // 이동할 경로 포인트 리스트
    public float speed = 2f; // 이동 속도
    private int currentWaypointIndex = 0; // 현재 목표로 하는 경로 포인트 인덱스

    void Update()
    {
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}
