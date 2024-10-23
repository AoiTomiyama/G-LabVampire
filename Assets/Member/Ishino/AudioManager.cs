using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public AudioClip[] bgmClips; // シーンごとに再生するBGMを格納する配列

    [SerializeField, Header("SEの素材")]
    private SeStruct[] seClips;//seを格納する配列
    private Dictionary<SE, AudioClip> _seClipDictionary;//seの辞書

    public AudioSource seSource;//seを流すAudioSouse
    public AudioSource audioSource;//BGMを流すAudioSouse

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded; // シーンがロードされた時に呼ばれるメソッドを設定

        _seClipDictionary = seClips.Distinct().ToDictionary(item => item.seType, item => item.clip); //配列tを指定しやすいように辞書型に変換

        PlayBGMForScene(SceneManager.GetActiveScene().buildIndex);

    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.buildIndex); // シーンに応じたBGMを再生
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
    /// SEを再生する
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
            Debug.LogError("未割り当てのSEが呼び出されました");
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
