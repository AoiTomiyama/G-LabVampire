using System.Collections.Generic;
using UnityEngine;

public class MapObjectGenerater : MonoBehaviour
{
    public GameObject[] Prefabs; // 生成するプレハブの配列
    public Transform Player; // プレイヤーのTransform
    public float SpawnDistance = 5.0f; // プレイヤーがこの距離を移動したら生成
    public float MaxDistance = 20.0f; // オブジェクトがプレイヤーからこの距離を超えたら破棄
    private Rigidbody2D rigidbody2D;
    private Vector3 lastSpawnPosition; // 最後に生成した位置
    private List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトを管理
    private Vector3 previousMovementDirection; // 直前の移動方向

    void Start()
    {
        rigidbody2D = Player.GetComponent<Rigidbody2D>(); // Rigidbody2Dを取得
        lastSpawnPosition = Player.position; // 最後の生成位置を初期化
        previousMovementDirection = Vector3.up; // 初期移動方向を上（デフォルト）に設定
        SpawnInitialObjects(); // 初期位置にオブジェクトを生成
    }

    void Update()
    {
        // プレイヤーが一定距離移動したら新しいオブジェクトを生成
        if (Vector3.Distance(Player.position, lastSpawnPosition) >= SpawnDistance)
        {
            Debug.Log("Spawning new objects...");
            SpawnObjects();
        }

        // 古いオブジェクトを破棄
        RemoveOldObjects();
    }

    void SpawnInitialObjects()
    {
        // 各プレハブの初期位置に生成
        for (int i = 0; i < Prefabs.Length; i++)
        {
            Vector3 prefabInitialPosition = Prefabs[i].transform.position; // プレハブの初期位置
            Vector3 spawnPosition = Player.position + prefabInitialPosition; // プレイヤーの位置を基準にする
            GameObject newObject = Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
            Debug.Log($"Spawned initial object: {newObject.name} at {spawnPosition}");
        }
        lastSpawnPosition = Player.position; // 初期生成後の更新
    }

    void SpawnObjects()
    {
        // プレイヤーの位置を取得
        Vector3 playerPosition = Player.position;

        // プレイヤーの進行方向を計算
        Vector3 movementDirection = rigidbody2D.velocity.normalized; // Rigidbody2Dから速度を取得

        // プレイヤーが動いていない場合は直前の移動方向を使用
        if (movementDirection == Vector3.zero)
        {
            movementDirection = previousMovementDirection; // 直前の移動方向を使用
        }
        else
        {
            previousMovementDirection = movementDirection; // 移動方向を更新
        }

        // プレハブを進行方向の少し先に生成
        for (int i = 0; i < Prefabs.Length; i++)
        {
            // プレハブの初期位置を基に生成位置を決定
            Vector3 prefabInitialPosition = Prefabs[i].transform.localPosition; // プレハブの相対位置を取得

            // 進行方向に基づいて生成位置を決定
            Vector3 spawnPosition = playerPosition + movementDirection * SpawnDistance * 0.7f;

            // プレハブの初期位置をオフセット
            spawnPosition += prefabInitialPosition;

            // 生成する位置が正しいかデバッグ用にログ出力
            Debug.Log($"Spawning {Prefabs[i].name} at {spawnPosition}");

            GameObject newObject = Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
        }

        // 最後の生成位置を更新
        lastSpawnPosition = playerPosition; // プレイヤーの位置で更新
    }

    void RemoveOldObjects()
    {
        // プレイヤーから一定距離を超えたオブジェクトを破棄
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            float distance = Vector3.Distance(Player.position, spawnedObjects[i].transform.position);
            if (distance > MaxDistance)
            {
                Debug.Log($"Destroying object: {spawnedObjects[i].name} at distance: {distance}");
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}
