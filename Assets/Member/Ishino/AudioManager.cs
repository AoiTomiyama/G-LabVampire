using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] bgmClips; // シーンごとに再生するBGMを格納する配列
    public AudioClip[] seClips; //seを格納する配列
    public AudioSource seSource;//seを流すAudioSouse
    public AudioSource audioSource;//BGMを流すAudioSouse

    public static AudioManager instance;//シングルトンで管理。シングルトンは一つだけしか存在しないオブジェクトのこと

    void Awake()
    {
        // インスタンスがまだ存在していなければ、現在のオブジェクトをインスタンスにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // このオブジェクトを壊さない
        }
        else
        {
            // すでに存在しているインスタンスがあれば、現在のオブジェクトを破棄
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // シーンがロードされた時に呼ばれるメソッドを設定
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.buildIndex); // シーンに応じたBGMを再生
    }

    public void PlayBGMForScene(int sceneIndex)
    {
        if (sceneIndex < bgmClips.Length && bgmClips[sceneIndex] != null)
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
    public void PlaySE(int seID)
    {
        if (seID < seClips.Length && seClips[seID] != null)
        {
            seSource.PlayOneShot(seClips[seID]);
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
}
