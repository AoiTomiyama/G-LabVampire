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
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;
        float buffer = TileSize * 2.0f;

        // �J�����̈ʒu���l�����������͈͂�ݒ�
        Vector3 playerPos = player.position;
        Vector3 generationBottomLeft = playerPos - new Vector3(cameraWidth / 2 + buffer, cameraHeight / 2 + buffer, 0);
        Vector3 generationTopRight = playerPos + new Vector3(cameraWidth / 2 + buffer, cameraHeight / 2 + buffer, 0);

        // �^�C�����W�𐮐��ɕϊ�
        int minTileX = Mathf.FloorToInt(generationBottomLeft.x / TileSize) - 1;
        int maxTileX = Mathf.FloorToInt(generationTopRight.x / TileSize) + 1;
        int minTileY = Mathf.FloorToInt(generationBottomLeft.y / TileSize) - 1;
        int maxTileY = Mathf.FloorToInt(generationTopRight.y / TileSize) + 1;

        // �^�C������
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

        // �s�v�ȃ^�C�����폜
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();
        Vector2Int playerTilePos = GetPlayerTilePos();

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
            Mathf.FloorToInt(player.position.x / (TileSize + SpawnSpacing)),  // �^�C���T�C�Y�ƊԊu���l��
            Mathf.FloorToInt(player.position.y / TileSize )
        );
    }

    void SpawnTile(Vector2 tilePos)
    {
        // �^�C���T�C�Y�ƊԊu������
        Vector3 worldPos = new Vector3(
            tilePos.x * (TileSize + SpawnSpacing),  tilePos.y * TileSize,0);

        // �V�����^�C���𐶐�
        GameObject newTile = Instantiate(mapPrefab, worldPos, Quaternion.identity);

        // �^�C�����W�������ɕۑ�
        Vector2Int tilePosInt = new Vector2Int(Mathf.FloorToInt(tilePos.x), Mathf.FloorToInt(tilePos.y));
        spawnedTiles[tilePosInt] = newTile;
    }

}
