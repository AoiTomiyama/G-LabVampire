using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public GameObject mapPrefab; // マップタイルのプレハブ
    public Transform player; // プレイヤーのTransform
    [SerializeField] public int tileSize = 10; // 1つのタイルのサイズ
    public int Distance = 1; 

    private Dictionary<Vector2Int, GameObject> spawnedTiles = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        UpdateMap();
    }
    void Update()
    {
        UpdateMap();
    }

    void UpdateMap()
    {
        
        Vector2Int playerTilePos = new Vector2Int(
            Mathf.FloorToInt(player.position.x / tileSize),
            Mathf.FloorToInt(player.position.y / tileSize)
        );

        // 周辺タイルを生成
        for (int x = -Distance; x <= Distance; x++)
        {
            for (int y = -Distance; y <= Distance; y++)
            {
                Vector2Int tilePos = new Vector2Int(playerTilePos.x + x, playerTilePos.y + y);
                if (!spawnedTiles.ContainsKey(tilePos))
                {
                    SpawnTile(tilePos);
                }
            }
        }

        // 不要なタイルを削除
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();
        foreach (var tile in spawnedTiles)
        {
            if (Mathf.Abs(tile.Key.x - playerTilePos.x) > Distance ||
                Mathf.Abs(tile.Key.y - playerTilePos.y) > Distance)
            {
                tilesToRemove.Add(tile.Key);
            }
        }

        foreach (var tilePos in tilesToRemove)
        {
            Destroy(spawnedTiles[tilePos]);
            spawnedTiles.Remove(tilePos);
        }
    }

    bool CanSpawnTile(Vector2Int tilePos)
    {
        // タイルのワールド位置を計算
        Vector3 worldPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);

        // タイルのサイズを考慮して重なりを確認
        Collider2D[] colliders = Physics2D.OverlapBoxAll(worldPos, new Vector2(tileSize, tileSize), 0);
        return colliders.Length == 0; // 重なりがない場合はtrueを返す
    }
    void SpawnTile(Vector2Int tilePos)
    {
        Vector3 worldPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);
        GameObject newTile = Instantiate(mapPrefab, worldPos, Quaternion.identity);
        spawnedTiles[tilePos] = newTile;
    }
}
