using System.Collections.Generic;
using UnityEngine;

public class MapObjectGenerater : MonoBehaviour
{
    public GameObject[] Prefabs; // ��������v���n�u�̔z��
    public Transform Player; // �v���C���[��Transform
    public float SpawnDistance = 5.0f; // �v���C���[�����̋������ړ������琶��
    public float MaxDistance = 20.0f; // �I�u�W�F�N�g���v���C���[���炱�̋����𒴂�����j��
    private Rigidbody2D rigidbody2D;
    private Vector3 lastSpawnPosition; // �Ō�ɐ��������ʒu
    private List<GameObject> spawnedObjects = new List<GameObject>(); // �������ꂽ�I�u�W�F�N�g���Ǘ�
    private Vector3 previousMovementDirection; // ���O�̈ړ�����

    void Start()
    {
        rigidbody2D = Player.GetComponent<Rigidbody2D>(); // Rigidbody2D���擾
        lastSpawnPosition = Player.position; // �Ō�̐����ʒu��������
        previousMovementDirection = Vector3.up; // �����ړ���������i�f�t�H���g�j�ɐݒ�
        SpawnInitialObjects(); // �����ʒu�ɃI�u�W�F�N�g�𐶐�
    }

    void Update()
    {
        // �v���C���[����苗���ړ�������V�����I�u�W�F�N�g�𐶐�
        if (Vector3.Distance(Player.position, lastSpawnPosition) >= SpawnDistance)
        {
            Debug.Log("Spawning new objects...");
            SpawnObjects();
        }

        // �Â��I�u�W�F�N�g��j��
        RemoveOldObjects();
    }

    void SpawnInitialObjects()
    {
        // �e�v���n�u�̏����ʒu�ɐ���
        for (int i = 0; i < Prefabs.Length; i++)
        {
            Vector3 prefabInitialPosition = Prefabs[i].transform.position; // �v���n�u�̏����ʒu
            Vector3 spawnPosition = Player.position + prefabInitialPosition; // �v���C���[�̈ʒu����ɂ���
            GameObject newObject = Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
            Debug.Log($"Spawned initial object: {newObject.name} at {spawnPosition}");
        }
        lastSpawnPosition = Player.position; // ����������̍X�V
    }

    void SpawnObjects()
    {
        // �v���C���[�̈ʒu���擾
        Vector3 playerPosition = Player.position;

        // �v���C���[�̐i�s�������v�Z
        Vector3 movementDirection = rigidbody2D.velocity.normalized; // Rigidbody2D���瑬�x���擾

        // �v���C���[�������Ă��Ȃ��ꍇ�͒��O�̈ړ��������g�p
        if (movementDirection == Vector3.zero)
        {
            movementDirection = previousMovementDirection; // ���O�̈ړ��������g�p
        }
        else
        {
            previousMovementDirection = movementDirection; // �ړ��������X�V
        }

        // �v���n�u��i�s�����̏�����ɐ���
        for (int i = 0; i < Prefabs.Length; i++)
        {
            // �v���n�u�̏����ʒu����ɐ����ʒu������
            Vector3 prefabInitialPosition = Prefabs[i].transform.localPosition; // �v���n�u�̑��Έʒu���擾

            // �i�s�����Ɋ�Â��Đ����ʒu������
            Vector3 spawnPosition = playerPosition + movementDirection * SpawnDistance * 0.7f;

            // �v���n�u�̏����ʒu���I�t�Z�b�g
            spawnPosition += prefabInitialPosition;

            // ��������ʒu�����������f�o�b�O�p�Ƀ��O�o��
            Debug.Log($"Spawning {Prefabs[i].name} at {spawnPosition}");

            GameObject newObject = Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newObject);
        }

        // �Ō�̐����ʒu���X�V
        lastSpawnPosition = playerPosition; // �v���C���[�̈ʒu�ōX�V
    }

    void RemoveOldObjects()
    {
        // �v���C���[�����苗���𒴂����I�u�W�F�N�g��j��
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
