using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : SingletonMonoBehaviour<PauseManager>
{
    /// <summary>Trueの時、ポーズ処理が可能になる</summary>
    private bool _enablePause = true;
    private bool _isPaused;

    /// <summary>何かしらの演出中で、中断させたくない場合に、これをFalseにする</summary>
    public bool EnablePause { get => _enablePause; set => _enablePause = value; }
    public bool IsPaused => _isPaused;
    /// <summary>
    /// 現在の状態に応じて、ポーズまたはポーズを終了する。
    /// </summary>
    public void PauseOrResume()
    {
        if (_isPaused)
        {
            Debug.Log("<color=red>[PauseManager]</color> 全体の演算を再開。");
            ResumeAll();
        }
        else
        {
            Debug.Log("<color=red>[PauseManager]</color> 全体の演算を停止。");
            PauseAll();
        }
    }

    /// <summary>ポーズ状態にする。</summary>
    public void PauseAll()
    {
        //IPausableインターフェイスを継承しているオブジェクトを対象に処理。
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Pause());

        //背景などのパーティクルもまとめて止めるためParticleSystemは別途処理する。
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Pause());

        _isPaused = true;
    }


    /// <summary>ポーズ状態を終了させる。</summary>
    public void ResumeAll()
    {
        //IPausableインターフェイスを継承しているオブジェクトを対象に処理。
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Resume());

        //背景などのパーティクルもまとめて止めるためParticleSystemは別途処理する。
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Play());

        _isPaused = false;
    }
}