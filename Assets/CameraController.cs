using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float MoveSpeed = 5;
    public float MaxSpeed = 10;

    private Vector3 TargetPos;
    private bool IsMouseDown;

    // Use this for initialization
    void Start() {
		
    }
	
    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            IsMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            IsMouseDown = false;
        }

        if (IsMouseDown) {
            TargetPos = CursorController.instance.transform.position;
        }

        float dist = (TargetPos - transform.position).magnitude;

        float speed = dist * MoveSpeed;
        speed = Mathf.Clamp(speed, 0, MaxSpeed);
        Vector3 newPos = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime * speed);

        float size = 25;
        newPos.x = Mathf.Clamp(newPos.x, -size, size);
        newPos.z = Mathf.Clamp(newPos.z, -size, size);

        transform.position = newPos;
    }
}
