using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    /// <summary>Trueの時、ポーズ処理が可能になる</summary>
    private bool _enablePause = true;
    private bool _isPaused;

    [Header("停止時と再開時の処理（スクリプトを伴わないものに使う")]
    [SerializeField]
    private UnityEvent OnPause;
    [SerializeField]
    private UnityEvent OnResume;

    /// <summary>何かしらの演出中で、中断させたくない場合に、これをFalseにする</summary>
    public bool EnablePause { get => _enablePause; set => _enablePause = value; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && _enablePause)
        {
            PauseOrResume();
        }
    }
    /// <summary>
    /// 現在の状態に応じて、ポーズまたはポーズを終了する。
    /// </summary>
    public void PauseOrResume()
    {
        if (_isPaused)
        {
            ResumeAll();
        }
        else
        {
            PauseAll();
        }
    }

    /// <summary>ポーズ状態にする。</summary>
    private void PauseAll()
    {
        //IPausableインターフェイスを継承しているオブジェクトを対象に処理。
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Pause());

        //背景などのパーティクルもまとめて止めるためParticleSystemは別途処理する。
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Pause());

        OnPause?.Invoke();

        _isPaused = !_isPaused;
    }


    /// <summary>ポーズ状態を終了させる。</summary>
    public void ResumeAll()
    {
        //IPausableインターフェイスを継承しているオブジェクトを対象に処理。
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Resume());

        //背景などのパーティクルもまとめて止めるためParticleSystemは別途処理する。
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Play());

        OnResume?.Invoke();

        _isPaused = !_isPaused;
    }
}