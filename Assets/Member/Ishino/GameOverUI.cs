using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Unity.VisualScripting;

public class GameOverUI : MonoBehaviour
{
    // UnityのTextコンポーネントをInspectorで設定する
    public Text playerLevelText;  // プレイヤーレベルを表示するテキスト
    public Text weaponsDataText;  // 武器データを表示するテキスト
    public Text itemsDataText;    // アイテムデータを表示するテキスト

    // ゲームオーバー時に呼び出されるメソッド
    public void DisplayGameOverInfo()
    {
        // インスタンスからデータを取得
        var dataManager = DataManagerBetweenScenes.Instance;

        // プレイヤーレベルの表示
        playerLevelText.text = $"{dataManager.PlayerLevelOnEnd}";
        Debug.Log(playerLevelText.text);

        // 武器データの表示
        StringBuilder weaponsBuilder = new StringBuilder();
        foreach (var weapon in dataManager.WeaponsData)
        {
            weaponsBuilder.AppendLine($"{weapon.Key} Lv:{weapon.Value}");
        }
        weaponsDataText.text = weaponsBuilder.ToString();

        // アイテムデータの表示
        StringBuilder itemsBuilder = new StringBuilder();
        foreach (var item in dataManager.ItemsData)
        {
            itemsBuilder.AppendLine($"{item.Key} Lv:{item.Value}");
        }
        itemsDataText.text = itemsBuilder.ToString();
    }
}
