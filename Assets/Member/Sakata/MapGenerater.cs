using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public GameObject mapPrefab; // �}�b�v�^�C���̃v���n�u
    public Transform player; // �v���C���[��Transform
    public Camera mainCamera;
    [SerializeField] public int tileSize = 10; // 1�̃^�C���̃T�C�Y
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

        // ���Ӄ^�C���𐶐�

        // �J�����͈̔͂��v�Z����
        Vector3 cameraBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 cameraTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // �J�����͈̔͂���A��������^�C���͈̔͂��v�Z
        int minTileX = Mathf.FloorToInt(cameraBottomLeft.x / tileSize) - 1;
        int maxTileX = Mathf.FloorToInt(cameraTopRight.x / tileSize) + 1;
        int minTileY = Mathf.FloorToInt(cameraBottomLeft.y / tileSize) - 1;
        int maxTileY = Mathf.FloorToInt(cameraTopRight.y / tileSize) + 1;

        // �J�����͈̔͂ɍ��킹�ă^�C���𐶐�
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
        // tilePos ���g���ă^�C���̃��[���h���W���v�Z���A��������
        Vector3 worldPos = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0);
        GameObject newTile = Instantiate(mapPrefab, worldPos, Quaternion.identity);

        // �^�C���������ŊǗ����邽�߁AtilePos �𐮐��ɕϊ����ăL�[�ɂ���
        Vector2Int tilePosInt = new Vector2Int(Mathf.FloorToInt(tilePos.x), Mathf.FloorToInt(tilePos.y));
        spawnedTiles[tilePosInt] = newTile;
    }
}

