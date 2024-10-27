using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : SingletonMonoBehaviour<PauseManager>
{
    /// <summary>True�̎��A�|�[�Y�������\�ɂȂ�</summary>
    private bool _enablePause = true;
    private bool _isPaused;

    /// <summary>��������̉��o���ŁA���f���������Ȃ��ꍇ�ɁA�����False�ɂ���</summary>
    public bool EnablePause { get => _enablePause; set => _enablePause = value; }
    public bool IsPaused => _isPaused;
    /// <summary>
    /// ���݂̏�Ԃɉ����āA�|�[�Y�܂��̓|�[�Y���I������B
    /// </summary>
    public void PauseOrResume()
    {
        if (_isPaused)
        {
            Debug.Log("<color=red>[PauseManager]</color> �S�̂̉��Z���ĊJ�B");
            ResumeAll();
        }
        else
        {
            Debug.Log("<color=red>[PauseManager]</color> �S�̂̉��Z���~�B");
            PauseAll();
        }
    }

    /// <summary>�|�[�Y��Ԃɂ���B</summary>
    public void PauseAll()
    {
        //IPausable�C���^�[�t�F�C�X���p�����Ă���I�u�W�F�N�g��Ώۂɏ����B
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Pause());

        //�w�i�Ȃǂ̃p�[�e�B�N�����܂Ƃ߂Ď~�߂邽��ParticleSystem�͕ʓr��������B
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Pause());

        _isPaused = true;
    }


    /// <summary>�|�[�Y��Ԃ��I��������B</summary>
    public void ResumeAll()
    {
        //IPausable�C���^�[�t�F�C�X���p�����Ă���I�u�W�F�N�g��Ώۂɏ����B
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Resume());

        //�w�i�Ȃǂ̃p�[�e�B�N�����܂Ƃ߂Ď~�߂邽��ParticleSystem�͕ʓr��������B
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Play());

        _isPaused = false;
    }
}