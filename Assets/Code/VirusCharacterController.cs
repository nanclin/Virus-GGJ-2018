﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VirusCharacterController : MonoBehaviour {

    public Transform target;
    public float RotateSpeed;
    public float WalkSpeed;

    public List<Vector3> PathPoints = new List<Vector3>();

    private bool FollowTarget;
    private Vector3 CurrentTargetPoint;
    private int CurrentPathPointIndex = 0;

    // Use this for initialization
    void Start() {
        FollowTarget = true;
        CurrentTargetPoint = PathPoints[0];
    }
	
    // Update is called once per frame
    void Update() {
        if (FollowTarget) {
            FollowingTarget(CurrentTargetPoint, OnTargetReached);
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

    private void OnTargetReached() {
//        FollowTarget = false;
        CurrentPathPointIndex = (CurrentPathPointIndex + 1) % PathPoints.Count;
        CurrentTargetPoint = PathPoints[CurrentPathPointIndex];
    }

    private void OnDrawGizmosSelected() {
        for (int i = 0; i < PathPoints.Count; i++) {
            Vector3 point = PathPoints[i];
            Gizmos.DrawWireSphere(point, i == 0 ? 0.5f : 0.2f);
            if (i > 0) {
                Gizmos.DrawLine(PathPoints[i - 1], point);
            }
        }
    }
}
