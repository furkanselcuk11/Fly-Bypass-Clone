using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scoret Type", menuName = "ScoreSO")]
public class ScoreSO : ScriptableObject
{
    [SerializeField] private int _totalCoin;
    [SerializeField] private int _finishCollectedWings;
    [SerializeField] private int _gameLevel = 0;
    public int totalCoin
    {
        get { return _totalCoin; }
        set { _totalCoin = value; }
    }
    public int finishCollectedWings
    {
        get { return _finishCollectedWings; }
        set { _finishCollectedWings = value; }
    }
    public int gameLevel
    {
        get { return _gameLevel; }
        set { _gameLevel = value; }
    }
}

