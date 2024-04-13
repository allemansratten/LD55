using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public TMP_Text currencyText;
    private int currency = 100;

    void Start()
    {
        // currencyText.text = '$' + currency.ToString();
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        // currencyText.text = '$' + currency.ToString();
    }

    public int GetCurrency()
    {
        return currency;
    }
}
