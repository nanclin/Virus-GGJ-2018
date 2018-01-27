using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

    public VirusCharacterController CharacterPrefab;
    public float AreaSize = 50;
    public Camera MainCamera;
    public int SpawnCount = 3;

    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        instance = this;
    }

    void Start() {
        SpawnCharacters(SpawnCount);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Time.timeScale = 0;
        } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Time.timeScale = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Time.timeScale = 2;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Time.timeScale = 0.5f;
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            Time.timeScale = 0.1f;
        } 
    }

    public void RestartGame() {
//        UiController.Instance.OpenMainUi();
        RemoveAllCharacters();
        SpawnCharacters(SpawnCount);
    }

    public void EndGame() {
        UiController.Instance.OpenPostmatch();
    }

    private void SpawnCharacters(int count) {
        for (int i = 0; i < count; i++) {
            VirusCharacterController character = Instantiate(CharacterPrefab);
            Vector3 newPos = GetRandomPointOnNavMesh();
            character.transform.position = newPos;
        }
    }

    private void RemoveAllCharacters() {
        foreach (var character in VirusCharacterController.AllCharacters) {
            Destroy(character.gameObject);
        }
        VirusCharacterController.AllCharacters.Clear();
    }

    private Vector3 GetRandomPointOnNavMesh() {
        for (int i = 0; i < 10; i++) {
            NavMeshHit hit;
            NavMesh.SamplePosition(GetRandomArenaPosition(), out hit, 3, 1);
            Vector3 randomPos = hit.position;
            randomPos.y = 0;
            return randomPos;
        }
        throw new Exception("no random point on NavMesh found");
    }

    public Vector3 GetRandomArenaPosition() {
        float size = AreaSize * 0.5f;
        float x = Random.Range(-size, size);
        float z = Random.Range(-size, size);
        return new Vector3(x, 0, z);
    }
}
