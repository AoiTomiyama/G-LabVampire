using UnityEngine;

/// <summary>
/// シングルトン作成用の抽象クラス。
/// シングルトン化させたいオブジェクトにこれを継承させる。
/// </summary>
/// <typeparam name="T">継承したクラス名を入れる</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            //何らかのアクセスがあった場合、インスタンス化されているものを探し、変数に代入する。
            if (!_instance)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (!_instance)
                {
                    // 1つもインスタンス化されていなかった場合、警告を表示する。
                    Debug.LogError(typeof(T) + "は存在しません");
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (this != Instance)
        {
            // 既に別のインスタンスが存在した場合、自身を削除
            Debug.Log("重複を削除");
            Destroy(gameObject);
            return;
        }
        else
        {
            // 自身をDDOL化する。
            DontDestroyOnLoad(gameObject);
        }
    }
}