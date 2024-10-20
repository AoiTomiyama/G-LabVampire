using System.Collections;
using UnityEngine;

public class MapObjectGenerater : MonoBehaviour
{
    public GameObject[] Prefabs; // �v���n�u�̔z��
    public Transform SpawnPoint; // �����ʒu
    public float SpawnInterval = 2.0f; // �����Ԋu�i�b�j
    private Vector3[] spawnOffsets; // �e�v���n�u�̐����ʒu�̃I�t�Z�b�g

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            // �S�Ẵv���n�u�𓯎��ɐ���
            for (int i = 0; i < Prefabs.Length; i++)
            {
                // �e�v���n�u�̐����ʒu�ɃI�t�Z�b�g��K�p���Đ���
                Vector3 spawnPosition = SpawnPoint.position + spawnOffsets[i];
                Instantiate(Prefabs[i], spawnPosition, Quaternion.identity);
            }

            // ���̐����܂ň��Ԋu�ҋ@
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
