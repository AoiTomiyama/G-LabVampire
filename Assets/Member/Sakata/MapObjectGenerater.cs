using System.Collections.Generic;
using UnityEngine;

public class MapObjectGenerater : MonoBehaviour
{
    public GameObject[] Prefabs; // 生成するプレハブの配列
    public Transform Player; // プレイヤーのTransform
    public float SpawnDistance = 10.0f; // プレイヤーがこの距離を移動したら生成
    public float MaxDistance = 20.0f; // オブジェクトがプレイヤーからこの距離を超えたら破棄
    private Vector3 lastSpawnPosition; // 最後に生成した位置
    private List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトを管理
    private Vector3[] initialPositions; // 各プレハブの初期位置

    void Start()
    {
        // プレハブごとの初期位置を保持
        initialPositions = new Vector3[Prefabs.Length];
        for (int i = 0; i < Prefabs.Length; i++)
        {
            initialPositions[i] = Prefabs[i].transform.position;
        }

        // 初期位置にプレハブを生成
        SpawnInitialObjects();

        // 最後に生成した位置をプレイヤーの現在位置で初期化
        lastSpawnPosition = Player.position;
    }

    void Update()
    {
        // プレイヤーが一定距離移動したら新しいオブジェクトを生成
        if (Vector3.Distance(Player.position, lastSpawnPosition) >= SpawnDistance)
        {
            SpawnObjects(Player.position - lastSpawnPosition); // 移動量を渡す
            lastSpawnPosition = Player.position;
        }

        // 古いオブジェクトを破棄
        RemoveOldObjects();
    }

    void SpawnInitialObjects()
    {
        // 初期位置にプレハブを生成
        for (int i = 0; i < Prefabs.Length; i++)
        {
            // 初期位置に生成
            GameObject newObject = Instantiate(Prefabs[i], initialPositions[i], Quaternion.identity);
            spawnedObjects.Add(newObject);
        }
    }

    void SpawnObjects(Vector3 offset)
    {
        // プレハブをすべて同時に生成
        for (int i = 0; i < Prefabs.Length; i++)
        {
            // プレイヤーの移動を加味した位置に生成
            Vector3 spawnPosition = initialPositions[i] + offset;
            GameObject newObject = Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
        }
    }

    void RemoveOldObjects()
    {
        // プレイヤーから一定距離を超えたオブジェクトを破棄
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(Player.position, spawnedObjects[i].transform.position) > MaxDistance)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}
