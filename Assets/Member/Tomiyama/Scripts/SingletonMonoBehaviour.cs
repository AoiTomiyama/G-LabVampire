using UnityEngine;

/// <summary>
/// �V���O���g���쐬�p�̒��ۃN���X�B
/// �V���O���g�������������I�u�W�F�N�g�ɂ�����p��������B
/// </summary>
/// <typeparam name="T">�p�������N���X��������</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            //���炩�̃A�N�Z�X���������ꍇ�A�C���X�^���X������Ă�����̂�T���A�ϐ��ɑ������B
            if (!_instance)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (!_instance)
                {
                    // 1���C���X�^���X������Ă��Ȃ������ꍇ�A�x����\������B
                    Debug.LogError(typeof(T) + "�͑��݂��܂���");
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (this != Instance)
        {
            // ���ɕʂ̃C���X�^���X�����݂����ꍇ�A���g���폜
            Debug.Log("�d�����폜");
            Destroy(gameObject);
            return;
        }
        else
        {
            // ���g��DDOL������B
            DontDestroyOnLoad(gameObject);
        }
    }
}