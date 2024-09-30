using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public GameObject mapPrefab; // マップタイルのプレハブ
    public Transform player; // プレイヤーのTransform
    public Camera mainCamera;
    [SerializeField] public int tileSize = 10; // 1つのタイルのサイズ
    public float Distance = 1.0f;

    private Dictionary<Vector2Int, GameObject> spawnedTiles = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        InitializeMap();
        UpdateMap();
    }
    void Update()
    {
        UpdateMap();
    }
    void InitializeMap()
    {
        UpdateMap();
    }
    void UpdateMap()
    {

        Vector2 playerTilePos = new Vector2(
            player.position.x / tileSize,
            player.position.y / tileSize
        );

        // 周辺タイルを生成

        // カメラの範囲を計算する
        Vector3 cameraBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 cameraTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // カメラの範囲から、生成するタイルの範囲を計算
        int minTileX = Mathf.FloorToInt(cameraBottomLeft.x / tileSize) - 1;
        int maxTileX = Mathf.FloorToInt(cameraTopRight.x / tileSize) + 1;
        int minTileY = Mathf.FloorToInt(cameraBottomLeft.y / tileSize) - 1;
        int maxTileY = Mathf.FloorToInt(cameraTopRight.y / tileSize) + 1;

        // カメラの範囲に合わせてタイルを生成
        for (int x = minTileX; x <= maxTileX; x++)
        {
            for (int y = minTileY; y <= maxTileY; y++)
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

    void SpawnTile(Vector2 tilePos)
    {
        // tilePos を使ってタイルのワールド座標を計算し、生成する
        Vector3 worldPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);
        GameObject newTile = Instantiate(mapPrefab, worldPos, Quaternion.identity);

        // タイルを辞書で管理するため、tilePos を整数に変換してキーにする
        Vector2Int tilePosInt = new Vector2Int(Mathf.FloorToInt(tilePos.x), Mathf.FloorToInt(tilePos.y));
        spawnedTiles[tilePosInt] = newTile;
    }
}

