using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private ScoreSO scoreType = null;    // Scriptable Objects eriþir 

    public static GameManager gamemanagerInstance;
    public bool gameStart;  // Oyun basladimi
    public bool isFinish;   // Finish alanına girdimi
    private void Awake()
    {
        if (gamemanagerInstance == null)
        {
            gamemanagerInstance = this;
        }
    }
    void Start()
    {
        gameStart = false;  // oyun yuklendiginde gameStart false olarak baslar
        isFinish = false; // oyun yuklendiginde isFinish false olarak baslar
        StartTextReset();   // ana ekrandaki text yazıları gunceller
    }
    void StartTextReset()
    {
        // ana ekrandaki text yazıları gunceller
        //scoreType.currentCoin = scoreType.minCoin;
        //UIController.uicontrollerInstance.GamePlayGoldText.text = scoreType.minCoin.ToString();
    }
    void Update()
    {
        
    }
    public void AddCoin()
    {
        // Altın Ekler
        Debug.Log("Coin added");
        //AudioController.audioControllerInstance.Play("CoinSound");  // Coin ses acar
        //scoreType.totalCoin+=scoreType.currentCoin++;
        //UIController.uicontrollerInstance.GamePlayCoinText.text = scoreType.totalCoin.ToString();
        // Coin ekler ve text gunceller
    }
    public void Finish()
    {
        isFinish = true;  
        //UIController.uicontrollerInstance.WinPanelActive();    // LosePanel Açıl 
        //UIController.uicontrollerInstance.WinPanelText();        // WinPanel Açıl
    }
    public void RetyLevel()
    {
        // Yeniden başlat
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GamePausePlay(int value)
    {
        Time.timeScale = value;
        // Oyunu durdur
    }
    public void GameExit()
    {
        // Oyundan çık
        Application.Quit();
    }
}
