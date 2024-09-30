using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public GameObject mapPrefab; // �}�b�v�^�C���̃v���n�u
    public Transform player; // �v���C���[��Transform
    [SerializeField] public int tileSize = 10; // 1�̃^�C���̃T�C�Y
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

        // ���Ӄ^�C���𐶐�
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

        // �s�v�ȃ^�C�����폜
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
        // �^�C���̃��[���h�ʒu���v�Z
        Vector3 worldPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);

        // �^�C���̃T�C�Y���l�����ďd�Ȃ���m�F
        Collider2D[] colliders = Physics2D.OverlapBoxAll(worldPos, new Vector2(tileSize, tileSize), 0);
        return colliders.Length == 0; // �d�Ȃ肪�Ȃ��ꍇ��true��Ԃ�
    }
    void SpawnTile(Vector2Int tilePos)
    {
        Vector3 worldPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);
        GameObject newTile = Instantiate(mapPrefab, worldPos, Quaternion.identity);
        spawnedTiles[tilePos] = newTile;
    }
}
