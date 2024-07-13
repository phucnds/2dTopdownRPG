using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private int currentCoin = 100;

    const string COIN_TEXT = "CoinText";

    private void Start()
    {
        if (txtCoin == null)
        {
            txtCoin = GameObject.Find(COIN_TEXT).GetComponent<TextMeshProUGUI>();
        }
        txtCoin.text = currentCoin.ToString();
    }

    public void UpdateCurrentCoin()
    {

        currentCoin++;
        txtCoin.text = currentCoin.ToString();
    }
}
