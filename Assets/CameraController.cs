using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float MoveSpeed = 5;

    // Use this for initialization
    void Start() {
		
    }
	
    // Update is called once per frame
    void Update() {
        Vector3 target = CursorController.instance.transform.position;
        float dist = (target - transform.position).magnitude;
        float speed = dist * MoveSpeed;
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }
}
