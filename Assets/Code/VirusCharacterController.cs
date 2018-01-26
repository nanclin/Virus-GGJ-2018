using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusCharacterController : MonoBehaviour {

    public Transform target;
    public float RotateSpeed;
    public float WalkSpeed;

    // Use this for initialization
    void Start() {
		
    }
	
    // Update is called once per frame
    void Update() {
        Vector3 targetDir = target.position - transform.position;
        float rotateStep = RotateSpeed * Time.deltaTime;
        targetDir.y = 0;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotateStep, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);

        float walkStep = WalkSpeed * Time.deltaTime;
        transform.position += transform.forward * walkStep;
    }
}
