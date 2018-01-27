using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

    public VirusCharacterController CharacterPrefab;
    public float AreaSize = 50;

    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        instance = this;
    }

    void Start() {
        SpawnCharacters(2);
    }

    public void RestartGame() {
        UiController.Instance.OpenMainUi();
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
