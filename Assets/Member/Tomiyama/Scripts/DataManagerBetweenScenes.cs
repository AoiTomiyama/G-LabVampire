using System.Collections.Generic;

public class DataManagerBetweenScenes : SingletonMonoBehaviour<DataManagerBetweenScenes>
{
    private Dictionary<string, int> _weaponsData = new();
    private Dictionary<string, int> _itemsData = new();
    private int _playerLevelOnEnd;
    public Dictionary<string, int> WeaponsData { get => _weaponsData; set => _weaponsData = value; }
    public Dictionary<string, int> ItemsData { get => _itemsData; set => _itemsData = value; }
    public int PlayerLevelOnEnd { get => _playerLevelOnEnd; set => _playerLevelOnEnd = value; }
}
