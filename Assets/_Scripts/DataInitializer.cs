using TMPro;
using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    private GSParser _parser;
    
    private void Awake()
    {
        _parser = GetComponent<GSParser>();
        StartCoroutine(_parser.ParseGS());
    }
}