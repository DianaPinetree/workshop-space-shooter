using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinCounter : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private int playerID;
    private TextMeshProUGUI counter;
    private MatchManager manager;
    
    private void Awake() {
        manager = FindObjectOfType<MatchManager>();
        counter = GetComponent<TextMeshProUGUI>();
        counter.text = 0.ToString();
    }
    private void OnEnable() {
        MatchManager.matchReset += UpdateCounter;
    }

    private void OnDisable() {
        MatchManager.matchReset -= UpdateCounter;
    }

    public void UpdateCounter()
    {
        int number = playerID == 0 ? manager.Player1Wins : manager.Player2Wins;
        counter.text = number.ToString();
    }
}
