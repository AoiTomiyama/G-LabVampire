using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGenerator : MonoBehaviour
{
    [SerializeField, Header("ŒoŒ±’l‚ÌPrefab")]
    private GameObject[] _expPrefabs;
    public static ExpGenerator Instance { get; private set; }
    public GameObject[] ExpPrefabs => _expPrefabs;

    private void Awake()
    {
        Instance = this;
    }
}
