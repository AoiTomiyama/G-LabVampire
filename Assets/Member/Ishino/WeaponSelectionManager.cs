using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�p

public class WeaponSelectionManager : MonoBehaviour
{
    public List<Weapon> availableWeapons = new List<Weapon>(); // �g�p�\�ȕ��탊�X�g
    public Transform selectionPanel; // UI�p�l���ɕ���I������\������
    public GameObject selectionButtonPrefab; // �{�^���̃v���n�u
    public int maxWeaponLevel = 4; // �ő僌�x��

    // ���x���A�b�v���ɌĂяo���֐�
    public void LevelUp()
    {
        // ���x���ő�̕���͏��O���āA�I�����������_����3�\������
        List<Weapon> selectableWeapons = GetSelectableWeapons();

        if (selectableWeapons.Count > 0)
        {
            DisplayWeaponChoices(selectableWeapons);
        }
        else
        {
            Debug.Log("�I�ׂ镐�킪����܂���I");
        }
    }

    // ���x���A�b�v�\�ȕ����I�сA3�����_���ɑI�����Ƃ��ĕԂ�
    private List<Weapon> GetSelectableWeapons()
    {
        List<Weapon> selectableWeapons = availableWeapons.FindAll(w => w.level < maxWeaponLevel);

        // �����_����3�I���������B�������A���킪3�����̏ꍇ�͑S�ĕԂ�
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

    // �I������UI��ɕ\������
    private void DisplayWeaponChoices(List<Weapon> weaponChoices)
    {
        // �����̃{�^�����N���A
        foreach (Transform child in selectionPanel)
        {
            Destroy(child.gameObject);
        }

        // �e����ɑ΂��đI���{�^���𐶐�
        foreach (Weapon weapon in weaponChoices)
        {
            GameObject buttonObj = Instantiate(selectionButtonPrefab, selectionPanel);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();

            buttonText.text = weapon.weaponName + " (���x�� " + weapon.level + ")";

            // �{�^�������������ɂ��̕�������x���A�b�v
            button.onClick.AddListener(() => UpgradeWeapon(weapon));
        }
    }

    // ����̃��x����1�グ��
    private void UpgradeWeapon(Weapon weapon)
    {
        if (weapon.level < maxWeaponLevel)
        {
            weapon.level++;
            Debug.Log(weapon.weaponName + " �����x�� " + weapon.level + " �ɏオ��܂����I");
        }

        // UI���N���A����i�I���I���j
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
