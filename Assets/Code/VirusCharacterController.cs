using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusCharacterController : MonoBehaviour {

    public Transform target;
    public float speed;

    // Use this for initialization
    void Start() {
		
    }
	
    // Update is called once per frame
    void Update() {
        Vector3 targetDir = target.position - transform.position;
        float step = speed * Time.deltaTime;
        targetDir.y = 0;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
