using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //[SerializeField] private CharacterSO characterType = null;    // Scriptable Objects eriþir 
    //[SerializeField] private ScoreSO scoreType = null;    // Scriptable Objects eriþir 

    public static UIController uicontrollerInstance;

    [Space]
    [Header("Panel Controller")]
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject GamePlayPanel;
    [SerializeField] private GameObject LosePanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject PausePanel;

    [Space]
    [Header("StartPanelText Controller")]
    public TextMeshProUGUI TotalCoinText;
    [Header("GamePlayPanelText Controller")]
    public TextMeshProUGUI GamePlayCoinText;
    public TextMeshPro CollectedWingsText;
    [Header("WinPanelText Controller")]
    public TextMeshProUGUI WinCoinText;
    private void Awake()
    {
        if (uicontrollerInstance == null)
        {
            uicontrollerInstance = this;
        }
    }
    void Start()
    {
        StartUI();
        StartPanelText();
    }
    void Update()
    {
        
    }
    public void StartUI()
    {
        StartPanel.SetActive(true);
        GamePlayPanel.SetActive(false);
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
        PausePanel.SetActive(false);
    }
    public void GamePlayActive()
    {
        StartPanel.SetActive(false);
        GamePlayPanel.SetActive(true);
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
        PausePanel.SetActive(false);
    }
    public void LosePanelActive()
    {
        StartPanel.SetActive(false);
        GamePlayPanel.SetActive(false);
        LosePanel.SetActive(true);
        WinPanel.SetActive(false);
        PausePanel.SetActive(false);
    }
    public void WinPanelActive()
    {
        StartPanel.SetActive(false);
        GamePlayPanel.SetActive(false);
        LosePanel.SetActive(false);
        WinPanel.SetActive(true);
        PausePanel.SetActive(false);
    }
    public void StartPanelText()
    {
        //TotalCoinText.text = scoreType.totalCoin.ToString();
    }
    public void GamePlayPanelText()
    {
        //TotalCoinText.text = scoreType.totalCoin.ToString();
    }
    public void WinPanelText()
    {
        //WinCoinText.text = scoreType.currentCoin.ToString();
    }
}
