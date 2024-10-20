using System.Collections;
using UnityEngine;

public class MapObjectGenerater : MonoBehaviour
{
    public GameObject[] Prefabs; // プレハブの配列
    public Transform SpawnPoint; // 生成位置
    public float SpawnInterval = 2.0f; // 生成間隔（秒）
    private Vector3[] spawnOffsets; // 各プレハブの生成位置のオフセット

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            // 全てのプレハブを同時に生成
            for (int i = 0; i < Prefabs.Length; i++)
            {
                // 各プレハブの生成位置にオフセットを適用して生成
                Vector3 spawnPosition = SpawnPoint.position + spawnOffsets[i];
                Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            }

            // 次の生成まで一定間隔待機
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
