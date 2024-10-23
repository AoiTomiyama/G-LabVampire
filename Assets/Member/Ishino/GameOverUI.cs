using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Unity.VisualScripting;

public class GameOverUI : MonoBehaviour
{
    // Unity��Text�R���|�[�l���g��Inspector�Őݒ肷��
    public Text playerLevelText;  // �v���C���[���x����\������e�L�X�g
    public Text weaponsDataText;  // ����f�[�^��\������e�L�X�g
    public Text itemsDataText;    // �A�C�e���f�[�^��\������e�L�X�g

    // �Q�[���I�[�o�[���ɌĂяo����郁�\�b�h
    public void DisplayGameOverInfo()
    {
        // �C���X�^���X����f�[�^���擾
        var dataManager = DataManagerBetweenScenes.Instance;

        // �v���C���[���x���̕\��
        playerLevelText.text = $"{dataManager.PlayerLevelOnEnd}";
        Debug.Log(playerLevelText.text);

        // ����f�[�^�̕\��
        StringBuilder weaponsBuilder = new StringBuilder();
        foreach (var weapon in dataManager.WeaponsData)
        {
            weaponsBuilder.AppendLine($"{weapon.Key} Lv:{weapon.Value}");
        }
        weaponsDataText.text = weaponsBuilder.ToString();

        // �A�C�e���f�[�^�̕\��
        StringBuilder itemsBuilder = new StringBuilder();
        foreach (var item in dataManager.ItemsData)
        {
            itemsBuilder.AppendLine($"{item.Key} Lv:{item.Value}");
        }
        itemsDataText.text = itemsBuilder.ToString();
    }
}
