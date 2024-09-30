using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI用

public class WeaponSelectionManager : MonoBehaviour
{
    public List<Weapon> availableWeapons = new List<Weapon>(); // 使用可能な武器リスト
    public Transform selectionPanel; // UIパネルに武器選択肢を表示する
    public GameObject selectionButtonPrefab; // ボタンのプレハブ
    public int maxWeaponLevel = 4; // 最大レベル

    // レベルアップ時に呼び出す関数
    public void LevelUp()
    {
        // レベル最大の武器は除外して、選択肢をランダムに3つ表示する
        List<Weapon> selectableWeapons = GetSelectableWeapons();

        if (selectableWeapons.Count > 0)
        {
            DisplayWeaponChoices(selectableWeapons);
        }
        else
        {
            Debug.Log("選べる武器がありません！");
        }
    }

    // レベルアップ可能な武器を選び、3つランダムに選択肢として返す
    private List<Weapon> GetSelectableWeapons()
    {
        List<Weapon> selectableWeapons = availableWeapons.FindAll(w => w.level < maxWeaponLevel);

        // ランダムに3つ選択肢を取る。ただし、武器が3つ未満の場合は全て返す
        if (selectableWeapons.Count > 3)
        {
            List<Weapon> randomWeapons = new List<Weapon>();

            while (randomWeapons.Count < 3)
            {
                Weapon randomWeapon = selectableWeapons[Random.Range(0, selectableWeapons.Count)];
                if (!randomWeapons.Contains(randomWeapon))
                {
                    randomWeapons.Add(randomWeapon);
                }
            }

            return randomWeapons;
        }
        else
        {
            return selectableWeapons;
        }
    }

    // 選択肢をUI上に表示する
    private void DisplayWeaponChoices(List<Weapon> weaponChoices)
    {
        // 既存のボタンをクリア
        foreach (Transform child in selectionPanel)
        {
            Destroy(child.gameObject);
        }

        // 各武器に対して選択ボタンを生成
        foreach (Weapon weapon in weaponChoices)
        {
            GameObject buttonObj = Instantiate(selectionButtonPrefab, selectionPanel);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();

            buttonText.text = weapon.weaponName + " (レベル " + weapon.level + ")";

            // ボタンを押した時にその武器をレベルアップ
            button.onClick.AddListener(() => UpgradeWeapon(weapon));
        }
    }

    // 武器のレベルを1つ上げる
    private void UpgradeWeapon(Weapon weapon)
    {
        if (weapon.level < maxWeaponLevel)
        {
            weapon.level++;
            Debug.Log(weapon.weaponName + " がレベル " + weapon.level + " に上がりました！");
        }

        // UIをクリアする（選択終了）
        foreach (Transform child in selectionPanel)
        {
            Destroy(child.gameObject);
        }
    }
}

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public int level;
}
