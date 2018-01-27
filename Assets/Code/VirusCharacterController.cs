using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class VirusCharacterController : MonoBehaviour {

    public Transform target;
    public float RotateSpeed;
    public float WalkSpeed;
    public float AreaSize = 50;

    public List<Vector3> PathPoints = new List<Vector3>();

    private bool FollowTarget;
    private Vector3 CurrentTargetPoint;
    private int CurrentPathPointIndex = 0;
    private int NavCornerIndex;
    private NavMeshPath NavMeshPath;
    private float elapsed = 0.0f;

    private bool NavFound = false;
    // Use this for initialization
    void Start() {
        CurrentTargetPoint = target.position;
        NavMeshPath = new NavMeshPath();
    }
	
    // Update is called once per frame
    void Update() {
        if (FollowTarget) {
//            FollowingTarget(CurrentTargetPoint, OnTargetReachedPathPoint);
//            FollowingTarget(CurrentTargetPoint, OnRandomTargetReached);
            FollowingTarget(CurrentTargetPoint, OnReachedNavCorner);
        }

        elapsed += Time.deltaTime;
        if (elapsed > 1.0f && NavFound == false) {
            elapsed -= 1.0f;
            FollowTarget = true;
            NavFound = true;
            FindNavPath();
        }
    }

    private void FollowingTarget(Vector3 targetPosition, Action onTargetReached) {
        Vector3 targetDir = targetPosition - transform.position;
        float rotateStep = RotateSpeed * Time.deltaTime;
        targetDir.y = 0;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotateStep, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);

        float walkStep = WalkSpeed * Time.deltaTime;
        transform.position += transform.forward * walkStep;

        float distanceSquared = Vector3.SqrMagnitude(targetPosition - transform.position);
//        Debug.Log(distanceSquared);
        if (distanceSquared < 2f) {
            onTargetReached();
        }
    }

    private Vector3 GetRandomPosition() {
        float size = AreaSize * 0.5f;
        float x = Random.Range(-size, size);
        float z = Random.Range(-size, size);
        return new Vector3(x, 0, z);
    }

    private void FindNavPath() {
        for (int i = 0; i < 10; i++) { // try finding path
            bool pathFound = NavMesh.CalculatePath(transform.position, GetRandomPosition(), NavMesh.AllAreas, NavMeshPath);
            if (pathFound) break;
        }

        if (NavMeshPath.corners.Length == 0) {
            FollowTarget = false;
            Debug.Log("no path found");
            return;
        }

        NavCornerIndex = 0;
        CurrentTargetPoint = NavMeshPath.corners[NavCornerIndex];
    }

    private void OnTargetReachedPathPoint() {
//        FollowTarget = false;
        CurrentPathPointIndex = (CurrentPathPointIndex + 1) % PathPoints.Count;
        CurrentTargetPoint = PathPoints[CurrentPathPointIndex];
    }

    private void OnReachedNavCorner() {
        NavCornerIndex++;
        if (NavCornerIndex >= NavMeshPath.corners.Length) {
            FindNavPath();
        } else {
            CurrentTargetPoint = NavMeshPath.corners[NavCornerIndex];
        }
    }

    private void OnRandomTargetReached() {
        CurrentTargetPoint = GetRandomPosition();
    }

#region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        for (int i = 0; i < PathPoints.Count; i++) {
            Vector3 point = PathPoints[i];
            Gizmos.DrawWireSphere(point, i == 0 ? 0.5f : 0.2f);
            if (i > 0) {
                Gizmos.DrawLine(PathPoints[i - 1], point);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CurrentTargetPoint, 0.5f);

        if (NavMeshPath == null) return;

        Gizmos.color = Color.magenta;
        for (int i = 0; i < NavMeshPath.corners.Length; i++) {
            Vector3 corner = NavMeshPath.corners[i];
            Gizmos.DrawWireSphere(corner, i == 0 ? 0.5f : 0.2f);
            if (i > 0) {
                Gizmos.DrawLine(NavMeshPath.corners[i - 1], corner);
            }
        }
    }

#endregion
}
