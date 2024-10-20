using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public GameObject mapPrefab;
    public Transform player;
    public Camera mainCamera;
    [SerializeField] public int TileSize = 10;
    [SerializeField] public float Distance = 10.0f;
    [SerializeField] public float SpawnSpacing = 1.0f;
    private Dictionary<Vector2Int, GameObject> spawnedTiles = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int PlayerTilePos;

   
    void Start()
    {
        UpdateMap();
        PlayerTilePos = GetPlayerTilePos();
    }

    void Update()
    {
        Vector2Int currentPlayerTilePos = GetPlayerTilePos();

        if (PlayerTilePos != currentPlayerTilePos)
        {
            UpdateMap();
            PlayerTilePos = currentPlayerTilePos;
            
        }
    }

    void UpdateMap()
    {
        // プレイヤーの位置を基に生成範囲を設定
        Vector2Int playerTilePos = GetPlayerTilePos();
        int range = 2; // プレイヤーの周囲に生成するタイルの範囲（調整可能）

        // タイル生成
        for (int x = playerTilePos.x - range; x <= playerTilePos.x + range; x++)
        {
            for (int y = playerTilePos.y - range; y <= playerTilePos.y + range; y++)
            {
                Vector2Int tilePosInt = new Vector2Int(x, y);
                if (!spawnedTiles.ContainsKey(tilePosInt))
                {
                    SpawnTile(new Vector2(x, y));
                }
            }
        }

        // 不要なタイルを削除
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();

        foreach (var tile in spawnedTiles)
        {
            // プレイヤーからの距離が Distance を超えたタイルを削除
            if (Mathf.Abs(tile.Key.x - playerTilePos.x) > Distance ||
                Mathf.Abs(tile.Key.y - playerTilePos.y) > Distance)
            {
                tilesToRemove.Add(tile.Key);
            }
        }

        foreach (var tilePos in tilesToRemove)
        {
            if (spawnedTiles[tilePos] != null)
            {
                Destroy(spawnedTiles[tilePos]);
            }
            spawnedTiles.Remove(tilePos);
        }
    }

    Vector2Int GetPlayerTilePos()
    {
        return new Vector2Int
        (
            Mathf.FloorToInt(player.position.x / (TileSize + SpawnSpacing)),  // タイルサイズと間隔を考慮
            Mathf.FloorToInt(player.position.y / TileSize )
        );
    }

    void SpawnTile(Vector2 tilePos)
    {
        // タイルサイズと間隔を加味
        Vector3 worldPos = new Vector3(
            tilePos.x * (TileSize + SpawnSpacing),  tilePos.y * TileSize,0);

        // 新しいタイルを生成
        GameObject newTile = Instantiate(mapPrefab, worldPos, Quaternion.identity);

        // タイル座標を辞書に保存
        Vector2Int tilePosInt = new Vector2Int(Mathf.FloorToInt(tilePos.x), Mathf.FloorToInt(tilePos.y));
        spawnedTiles[tilePosInt] = newTile;
    }

}
