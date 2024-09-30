using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // �t�F�[�h�p�����p�l��
    public float fadeDuration = 1.0f; // �t�F�[�h�̎���

    private void Start()
    {
        SetImage(0);
    }

    // �V�[�����t�F�[�h�A�E�g���Ȃ���ړ�����֐�
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    // �t�F�[�h�A�E�g�̃R���[�`��
    private IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0f;

        // �t�F�[�h�A�E�g���J�n
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetImage(alpha);

            yield return null;
        }

        // �V�[�������[�h
        SceneManager.LoadScene(sceneName);
    }

    private void SetImage(float x)//�J���[�ύX
    {
        Color color = fadeImage.color;
        color.a = x;
        fadeImage.color = color;
    }
}
