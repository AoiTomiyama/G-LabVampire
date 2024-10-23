using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;// �V���O���g���ŊǗ��B�V���O���g���͈�����������݂��Ȃ��I�u�W�F�N�g�̂���

    public AudioClip[] bgmClips; // �V�[�����ƂɍĐ�����BGM���i�[����z��

    [SerializeField, Header("SE�̑f��")]
    SeStruct[] seClips;//se���i�[����z��
    Dictionary<SE, AudioClip> _seClipDictionary;//se�̎���

    public AudioSource seSource;//se�𗬂�AudioSouse
    public AudioSource audioSource;//BGM�𗬂�AudioSouse

    void Awake()
    {        
        if (Instance == null)// �C���X�^���X���܂����݂��Ă��Ȃ���΁A���݂̃I�u�W�F�N�g���C���X�^���X�ɂ���
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//���̃I�u�W�F�N�g���󂳂Ȃ�
        }
        else
        {
            Destroy(gameObject);// �C���X�^���X���܂����݂��Ă��Ȃ���΁A���݂̃I�u�W�F�N�g���C���X�^���X�ɂ���
        }

       
        
    }

    void Start()
    {

        SceneManager.sceneLoaded += SceneLoaded; // �V�[�������[�h���ꂽ���ɌĂ΂�郁�\�b�h��ݒ�

        _seClipDictionary = seClips.Distinct().ToDictionary(item => item.seType, item => item.clip); //�z��t���w�肵�₷���悤�Ɏ����^�ɕϊ�
        
        PlayBGMForScene(SceneManager.GetActiveScene().buildIndex);

    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.buildIndex); // �V�[���ɉ�����BGM���Đ�
        Debug.Log("Scene");
    }

    public void PlayBGMForScene(int sceneIndex)
    {
        audioSource = GetComponent<AudioSource>();
        if (sceneIndex < bgmClips.Length && bgmClips[sceneIndex] != null && audioSource != null)
        {
            if (audioSource.clip != bgmClips[sceneIndex])
            {
                audioSource.clip = bgmClips[sceneIndex];
                audioSource.Play();
            }
        }
    }

    /// <summary>
    /// SE���Đ�����
    /// </summary>
    /// <param name="seID"></param>
    public void PlaySE(SE seType)
    {
        if (_seClipDictionary.ContainsKey(seType))
        {
            seSource.PlayOneShot(_seClipDictionary[seType]);
        }
        else
        {
            Debug.LogError("�����蓖�Ă�SE���Ăяo����܂���");
        }
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void ChangeBGM(AudioClip newClip)
    {
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
    public enum SE
    {
        None,
        EnemyDamage,
        PlayerDamage,
        IconPush,
        Katana,
        Naginata,
        Shikigami,
        Thunder
    }
    [Serializable]
    private struct SeStruct
    {
        public SE seType;
        public AudioClip clip;
    }
}
