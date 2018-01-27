using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class VirusCharacterController : MonoBehaviour {

    enum WalkState {
        None,
        WalkNavMesh,
        FollowMouse,
        FollowCharacter,
    }

    public static List<VirusCharacterController> AllCharacters = new List<VirusCharacterController>();
    private static Vector3 MousePosWorld;
    public static int SpawnedCount = 0;

    public Transform target;
    public float RotateSpeed;
    public float WalkSpeed;
    public float RunSpeed;
    public float AvoidRadius = 5;

    public List<Vector3> PathPoints = new List<Vector3>();

    private WalkState CurrentState = WalkState.None;
    private bool FollowTarget;
    private Vector3 TargetPosition;
    private int NavCornerIndex;
    private NavMeshPath NavMeshPath;
    private float elapsed = 0.0f;
    private bool IsInRange;
    private float CurrentSpeed;

    private List<VirusCharacterController> CharactersInRange = new List<VirusCharacterController>();

    public bool IsInfected { get; private set; }

    private bool NavFound = false;

	public Animator characterAnimator;

	public SkinnedMeshRenderer skinnedMeshRenderer;
	public Material normalMaterial;
	public Material zombieMaterial;

    // Use this for initialization
    void Start() {

		skinnedMeshRenderer.material = normalMaterial;
		CurrentSpeed = WalkSpeed;
		characterAnimator.SetTrigger("NormalWalk");
		NavMeshPath = new NavMeshPath();
        AllCharacters.Add(this);
        this.name = "char_" + SpawnedCount.ToString();

        if (SpawnedCount == 0) {
            OnInfected();
        }

        EnterState(WalkState.WalkNavMesh);

        SpawnedCount++;
    }
	
    // Update is called once per frame
    void Update() {

        if (IsInfected) {
            if (Input.GetMouseButtonDown(0)) {
                EnterState(WalkState.FollowMouse);
            }

            if (Input.GetMouseButtonUp(0)) {
                EnterState(WalkState.WalkNavMesh);
            }
        }

        ExecuteCurrentState();
    }

    private void EnterState(WalkState newState) {

        ExitCurrentState();

        switch (newState) {
            case WalkState.WalkNavMesh:
                OnEnterStateWalkNavMesh();
                break;
            case WalkState.FollowMouse:
                OnEnterStateFollowMouse();
                break;
            case WalkState.FollowCharacter:
                OnEnterStateFollowCharacter();
                break;
        }

        CurrentState = newState;
    }

    private void ExitCurrentState() {
        switch (CurrentState) {
            case WalkState.WalkNavMesh:
                OnExitStateWalkNavMesh();
                break;
            case WalkState.FollowMouse:
                OnExitStateFollowMouse();
                break;
            case WalkState.FollowCharacter:
                OnExitStateFollowCharacter();
                break;
        }
        CurrentState = WalkState.None;
    }

    private void ExecuteCurrentState() {
        switch (CurrentState) {
            case WalkState.WalkNavMesh:
                OnExecuteStateNavMesh();
                break;
            case WalkState.FollowMouse:
                OnExecuteStateFollowMouse();
                break;
            case WalkState.FollowCharacter:
                OnExecuteStateFollowCharacter();
                break;
        }
    }

#region Enter State Methods

    private void OnEnterStateWalkNavMesh() {
        NavFound = false;
        FindNavPath();
    }

    private void OnEnterStateFollowMouse() {
        CurrentState = WalkState.FollowMouse;
        CurrentSpeed = RunSpeed;
    }

    private void OnEnterStateFollowCharacter() {

    }

#endregion

#region Exit State Methods

    private void OnExitStateWalkNavMesh() {
        NavMeshPath.ClearCorners();
    }

    private void OnExitStateFollowMouse() {
        CurrentSpeed = WalkSpeed;
    }

    private void OnExitStateFollowCharacter() {

    }

#endregion

#region Execute State Methods

    private void OnExecuteStateNavMesh() {
        if (FollowTarget) {
            FollowingTarget(TargetPosition, OnReachedNavCorner);
        }

        elapsed += Time.deltaTime;
        float delay = 0.1f;
        if (elapsed > delay && NavFound == false) {
            elapsed -= delay;
            FollowTarget = true;
            NavFound = true;
            FindNavPath();
        }
    }

    private void OnExecuteStateFollowMouse() {
        Plane plane = new Plane(Vector3.up, 0);
        Camera camera = GameManager.Instance.MainCamera;
        float dist;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out dist)) {
            MousePosWorld = ray.GetPoint(dist);
        }
//        Debug.Log("ExecuteStateFollowMouse - pos: " + MousePosWorld);

        TargetPosition = MousePosWorld;
        FollowingTarget(TargetPosition, delegate {
        });
    }

    private void OnExecuteStateFollowCharacter() {
    }

#endregion

    void OnTriggerEnter(Collider other) {
        if (IsInfected) {
            if (other.gameObject.tag != "Character") return;
            VirusCharacterController enemy = other.gameObject.GetComponent<VirusCharacterController>();
            if (enemy.IsInfected) return;
            enemy.EnterState(CurrentState);
            enemy.OnInfected();

			//TODO: Call this 2 lines when you should attack the enemy
			string attackName = Random.Range(0f, 1f) < 0.5f ? "Attack1" : "Attack2";
			characterAnimator.SetTrigger(attackName);

		}
    }

    private void OnInfected() {
        IsInfected = true;
		characterAnimator.SetTrigger("ZombieWalk");
		skinnedMeshRenderer.material = zombieMaterial;
	}

    private void FollowingTarget(Vector3 targetPosition, Action onTargetReached) {

        Vector3 startDir = transform.forward;
        Vector3 targetDir = Vector3.zero;
        Vector3 avoidDir = Vector3.zero;
        Vector3 wantedDir;// target dir + avoid dir

        targetDir = targetPosition - transform.position;
        targetDir.y = 0;
        targetDir.Normalize();
        Debug.DrawRay(transform.position, targetDir * 3, Color.blue);

        IsInRange = false;
        CharactersInRange.Clear();

        // avoid other characters
        for (int i = 0; i < AllCharacters.Count; i++) {
            VirusCharacterController character = AllCharacters[i];

            // skip itself
            if (character == this) continue;

            Vector3 enemyDir = character.transform.position - transform.position;
            enemyDir.y = 0;
            float dist = enemyDir.magnitude;

//            Debug.DrawLine(character.transform.position, transform.position, Color.white);

            // if in range of enemy
            if (dist < AvoidRadius) {
                float avoidFactor01 = 1 - dist / AvoidRadius;
                // adjust direction away from enemy
                avoidDir += -enemyDir.normalized * avoidFactor01;
                Debug.DrawRay(transform.position, -enemyDir.normalized * avoidFactor01 * 3, Color.red);
                IsInRange = true;
                CharactersInRange.Add(character);
//                character.IsInfected = true;
            }
        }

        float rotateStep = RotateSpeed * Time.deltaTime;

        wantedDir = targetDir + avoidDir;
        wantedDir.Normalize();
        Debug.DrawRay(transform.position, wantedDir, Color.yellow);

        Vector3 newDir = Vector3.RotateTowards(startDir, wantedDir, rotateStep, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.green);

        transform.rotation = Quaternion.LookRotation(newDir);


        // walk forward
        float walkStep = CurrentSpeed * Time.deltaTime;

        Vector3 newPos = transform.position + newDir * walkStep;
        newPos.y = 0;
        transform.position = newPos;
//
        // is target reached?
        float distanceSquared = Vector3.SqrMagnitude(targetPosition - transform.position);
        if (distanceSquared < 2f) {
            onTargetReached();
        }
    }

    private void FindNavPath() {
        Vector3 startPos = transform.position;
        for (int i = 0; i < 10; i++) { // try finding path
            bool pathFound = NavMesh.CalculatePath(startPos, GameManager.Instance.GetRandomArenaPosition(), NavMesh.AllAreas, NavMeshPath);
            if (pathFound) break;
            startPos += transform.forward;
        }

        if (NavMeshPath.corners.Length == 0) {
            FollowTarget = false;
            Debug.Log("no path found");
            return;
        }

        NavCornerIndex = 0;
        TargetPosition = NavMeshPath.corners[NavCornerIndex];
    }

    private void OnReachedNavCorner() {
        NavCornerIndex++;
        if (NavCornerIndex >= NavMeshPath.corners.Length) {
            FindNavPath();
        } else {
            TargetPosition = NavMeshPath.corners[NavCornerIndex];
        }
    }

    private void OnRandomTargetReached() {
        TargetPosition = GameManager.Instance.GetRandomArenaPosition();
    }

#region Gizmos

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TargetPosition, 0.1f);

        if (NavMeshPath == null) return;

//        Gizmos.color = Color.magenta;
//        for (int i = 0; i < NavMeshPath.corners.Length; i++) {
//            Vector3 corner = NavMeshPath.corners[i];
////            Gizmos.DrawWireSphere(corner, i == 0 ? 0.5f : 0.2f);
//            if (i > 0) {
//                Gizmos.DrawLine(NavMeshPath.corners[i - 1], corner);
//            }
//        }

//        // avoid radius
//        Gizmos.color = IsInRange ? Color.red : Color.gray;
//        Gizmos.DrawWireSphere(transform.position, AvoidRadius);

        // mouse position on world floor
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(MousePosWorld, 1f);

        // infected indicator
        if (IsInfected) {
            switch (CurrentState) {
                case WalkState.WalkNavMesh:
                    Gizmos.color = Color.cyan;
                    break;
                case WalkState.FollowMouse:
                    Gizmos.color = Color.yellow;
                    break;
                case WalkState.FollowCharacter:
                    Gizmos.color = Color.red;
                    break;
            }
            Gizmos.DrawWireCube(transform.position, new Vector3(1.5f, 3, 1.5f));
        }
    }

#endregion
}
