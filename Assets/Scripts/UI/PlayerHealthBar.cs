using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private int playerID;
    private Image bar;

    private void Awake() {
        bar = GetComponent<Image>();
    }

    private void OnEnable() 
    {
        MatchManager.matchReset += ResetBar;
        Ship.hpChange += UpdateHealthBar;
    }

    private void OnDisable() 
    {
        MatchManager.matchReset -= ResetBar;
        Ship.hpChange -= UpdateHealthBar;
    }

    public void ResetBar()
    {
        bar.fillAmount = 1;
    }

    public void UpdateHealthBar()
    {
        bar.fillAmount = (float)MatchManager.Ships[playerID].CurrentHP / (float)Ship.MAX_HP;
    }
}
