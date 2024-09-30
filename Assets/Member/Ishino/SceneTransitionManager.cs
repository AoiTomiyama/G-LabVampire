using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // フェード用黒いパネル
    public float fadeDuration = 1.0f; // フェードの時間

    private void Start()
    {
        SetImage(0);
    }

    // シーンをフェードアウトしながら移動する関数
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    // フェードアウトのコルーチン
    private IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0f;

        // フェードアウトを開始
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetImage(alpha);

            yield return null;
        }

        // シーンをロード
        SceneManager.LoadScene(sceneName);
    }

    private void SetImage(float x)//カラー変更
    {
        Color color = fadeImage.color;
        color.a = x;
        fadeImage.color = color;
    }
}
