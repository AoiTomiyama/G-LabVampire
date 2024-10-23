using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public AudioClip[] bgmClips; // �V�[�����ƂɍĐ�����BGM���i�[����z��

    [SerializeField, Header("SE�̑f��")]
    private SeStruct[] seClips;//se���i�[����z��
    private Dictionary<SE, AudioClip> _seClipDictionary;//se�̎���

    public AudioSource seSource;//se�𗬂�AudioSouse
    public AudioSource audioSource;//BGM�𗬂�AudioSouse

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded; // �V�[�������[�h���ꂽ���ɌĂ΂�郁�\�b�h��ݒ�

        _seClipDictionary = seClips.Distinct().ToDictionary(item => item.seType, item => item.clip); //�z��t���w�肵�₷���悤�Ɏ����^�ɕϊ�

        PlayBGMForScene(SceneManager.GetActiveScene().buildIndex);

    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.buildIndex); // �V�[���ɉ�����BGM���Đ�
        Debug.Log("Scene");
    }

    public void PlayBGMForScene(int sceneIndex)
    {
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
