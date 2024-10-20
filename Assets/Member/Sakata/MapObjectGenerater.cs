using System.Collections.Generic;
using UnityEngine;

public class MapObjectGenerater : MonoBehaviour
{
    public GameObject[] Prefabs; // ��������v���n�u�̔z��
    public Transform Player; // �v���C���[��Transform
    public float SpawnDistance = 10.0f; // �v���C���[�����̋������ړ������琶��
    public float MaxDistance = 20.0f; // �I�u�W�F�N�g���v���C���[���炱�̋����𒴂�����j��
    private Vector3 lastSpawnPosition; // �Ō�ɐ��������ʒu
    private List<GameObject> spawnedObjects = new List<GameObject>(); // �������ꂽ�I�u�W�F�N�g���Ǘ�
    private Vector3[] initialPositions; // �e�v���n�u�̏����ʒu

    void Start()
    {
        // �v���n�u���Ƃ̏����ʒu��ێ�
        initialPositions = new Vector3[Prefabs.Length];
        for (int i = 0; i < Prefabs.Length; i++)
        {
            initialPositions[i] = Prefabs[i].transform.position;
        }

        // �����ʒu�Ƀv���n�u�𐶐�
        SpawnInitialObjects();

        // �Ō�ɐ��������ʒu���v���C���[�̌��݈ʒu�ŏ�����
        lastSpawnPosition = Player.position;
    }

    void Update()
    {
        // �v���C���[����苗���ړ�������V�����I�u�W�F�N�g�𐶐�
        if (Vector3.Distance(Player.position, lastSpawnPosition) >= SpawnDistance)
        {
            SpawnObjects(Player.position - lastSpawnPosition); // �ړ��ʂ�n��
            lastSpawnPosition = Player.position;
        }

        // �Â��I�u�W�F�N�g��j��
        RemoveOldObjects();
    }

    void SpawnInitialObjects()
    {
        // �����ʒu�Ƀv���n�u�𐶐�
        for (int i = 0; i < Prefabs.Length; i++)
        {
            // �����ʒu�ɐ���
            GameObject newObject = Instantiate(Prefabs[i], initialPositions[i], Quaternion.identity);
            spawnedObjects.Add(newObject);
        }
    }

    void SpawnObjects(Vector3 offset)
    {
        // �v���n�u�����ׂē����ɐ���
        for (int i = 0; i < Prefabs.Length; i++)
        {
            // �v���C���[�̈ړ������������ʒu�ɐ���
            Vector3 spawnPosition = initialPositions[i] + offset;
            GameObject newObject = Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
        }
    }

    void RemoveOldObjects()
    {
        // �v���C���[�����苗���𒴂����I�u�W�F�N�g��j��
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
