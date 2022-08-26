using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShootTrajectory : MonoBehaviour
{
    [SerializeField] LayerMask trajectoryMask;
    LineRenderer lr;
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    public void ShowTrajectory(float length, Vector2 lookDir, Vector3 spawnPoint)
    {
        List<Vector3> trajectory = CalculateTrajectory(length, lookDir, spawnPoint);
        lr.positionCount = trajectory.Count;
        lr.SetPositions(trajectory.ToArray());
    }
    private List<Vector3> CalculateTrajectory(float length, Vector2 lookDir, Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        List<Vector3> points = new List<Vector3>();
        points.Add(transform.position);

        RaycastHit2D hit;
        Vector2 prevPoint = new Vector2(transform.position.x, transform.position.y);
        Physics2D.queriesStartInColliders = false;

        while(length > 0)
        {
            hit = Physics2D.Raycast(prevPoint, lookDir, length, trajectoryMask);

            if(hit.collider != null)
            {
                points.Add(hit.point);
                prevPoint = hit.point;
                lookDir = Vector2.Reflect(lookDir, hit.normal);
                length -= hit.distance;
            }
            else
            {
                points.Add(new Vector3(prevPoint.x + (length * lookDir.normalized.x), prevPoint.y + (length * lookDir.normalized.y), 0));
                length = 0;
            }
        }
        Physics2D.queriesStartInColliders = true;
        return points;
    }
}
