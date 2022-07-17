using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private ScoreSO scoreType = null;    // Scriptable Objects eriþir 

    public static GameManager gamemanagerInstance;

    [SerializeField] GameObject player;
    public bool gameStart;  // Oyun basladimi
    public bool isFinish;   // Finish alanına girdimi
    public int collectedWingsCount = 0;
    public int wingsValue = 0;
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
        CharacterWings();
    }
    void StartTextReset()
    {
        // ana ekrandaki text yazıları gunceller
        //scoreType.currentCoin = scoreType.minCoin;
        //UIController.uicontrollerInstance.GamePlayGoldText.text = scoreType.minCoin.ToString();
        UIController.uicontrollerInstance.CollectedWingsText.text = collectedWingsCount.ToString();
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
    void WingsValueCalculate()
    {
        // Hangi kanatların aktif olacağı hesaplanır
        for (int i = 0; i < collectedWingsCount; i++)
        {
            if (i < 10)
            {
                wingsValue = 0;
            }
            else if (i < 20)
            {
                wingsValue = 1;
            }
            else if (i < 30)
            {
                wingsValue = 2;
            }
            else if (i < 40)
            {
                wingsValue = 3;
            }
            else if (i < 50)
            {
                wingsValue = 4;
            }
            else if (i < 60)
            {
                wingsValue = 5;
            }
            else if (i < 70)
            {
                wingsValue = 6;
            }
            else if (i < 80)
            {
                wingsValue = 7;
            }
            else if (i < 90)
            {
                wingsValue = 8;
            }
            else if (i >= 90)
            {
                wingsValue = 9;
            }
        }
    }
    public void WingsAdd(GameObject obj)
    {  
        // Fonksiyon her çalıştığında bir adet kanat aktif hale gelir
        collectedWingsCount+=10;  // Toplanan kanat sayısını arttır
        UIController.uicontrollerInstance.CollectedWingsText.text = collectedWingsCount.ToString(); // Toplanan kanat sayılarını karakter üzerinde göster
        StartCoroutine(WingsIsActive(obj)); // Toplanan objeyi pasif hale getir
        WingsValueCalculate();  // Kaç kanadın aktif olacağı sayı dönderir
        player.GetComponent<PlayerController>().CollectedLetfWing.ElementAt(wingsValue).gameObject.SetActive(true);
        player.GetComponent<PlayerController>().CollectedRigthWing.ElementAt(wingsValue).gameObject.SetActive(true);
    }
    public void WingsSubtract()
    {
        // Fonksiyon her çalıştığında bir adet kanat pasif hale gelir
        collectedWingsCount -= 10;  // Toplanan kanat sayısını arttır
        if (collectedWingsCount <= 0)
        {
            collectedWingsCount = 0;
        }
        UIController.uicontrollerInstance.CollectedWingsText.text = collectedWingsCount.ToString(); // Toplanan kanat sayılarını karakter üzerinde göster
        WingsValueCalculate(); // Kaç kanadın aktif olacağı sayı dönderir
        player.GetComponent<PlayerController>().CollectedLetfWing.ElementAt(wingsValue+1).gameObject.SetActive(false);
        player.GetComponent<PlayerController>().CollectedRigthWing.ElementAt(wingsValue+1).gameObject.SetActive(false);
    }
    IEnumerator WingsIsActive(GameObject obj)
    {
        obj.SetActive(false);   // Toplanan kanadı  pasif hale getir
        yield return new WaitForSeconds(2);
        obj.SetActive(true);    // Toplanan kanadı 2 saniye sonra aktif hale getir
    }
    public void CharacterWings()
    {
        // Baslangicda tüm kanatlari pasif yap
        for (int i = 0; i < player.GetComponent<PlayerController>().CollectedLetfWing.Count; i++)
        {
            player.GetComponent<PlayerController>().CollectedLetfWing.ElementAt(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < player.GetComponent<PlayerController>().CollectedRigthWing.Count; i++)
        {
            player.GetComponent<PlayerController>().CollectedRigthWing.ElementAt(i).gameObject.SetActive(false);
        }
    }
    public void Finish()
    {
        isFinish = true;  
        UIController.uicontrollerInstance.WinPanelActive();    // LosePanel Açıl 
        UIController.uicontrollerInstance.WinPanelText();        // WinPanel Açıl
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
