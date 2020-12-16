using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;

public class ChestContent
{
    private IntRange _moneyAmount = new IntRange(0, 100);
    private System.Random _random = new System.Random();

    public ChestContent()
    {
        _random = new System.Random();
    }

    public ChestContent(IntRange moneyAmount)
    {
        _moneyAmount = moneyAmount;
        _random = new System.Random();
    }

    public int GenerateReward()
    {
        int amount = _moneyAmount.RandomInRange(_random);
        return amount;
    }
}
