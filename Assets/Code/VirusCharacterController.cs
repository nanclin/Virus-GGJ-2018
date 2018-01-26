using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusCharacterController : MonoBehaviour {

    public Transform target;
    public float RotateSpeed;
    public float WalkSpeed;

    private bool FollowTarget;

    // Use this for initialization
    void Start() {
        FollowTarget = true;
    }
	
    // Update is called once per frame
    void Update() {
        if (FollowTarget) {
            FollowingTarget();
        }
    }

    private void FollowingTarget() {
        Vector3 targetDir = target.position - transform.position;
        float rotateStep = RotateSpeed * Time.deltaTime;
        targetDir.y = 0;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotateStep, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);

        float walkStep = WalkSpeed * Time.deltaTime;
        transform.position += transform.forward * walkStep;

        float distanceSquared = Vector3.SqrMagnitude(target.position - transform.position);
        Debug.Log(distanceSquared);
        if (distanceSquared < 2f) {
            OnTargetReached();
        }
    }

    private void OnTargetReached() {
        FollowTarget = false;
    }
}
