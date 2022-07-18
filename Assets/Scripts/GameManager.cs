using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreSO scoreType = null;    // Scriptable Objects eriþir 

    public static GameManager gamemanagerInstance;

    [SerializeField] GameObject player;
    public bool gameStart;  // Oyun basladimi
    public bool isFinish;   // Finish alanına girdimi
    public int collectedWingsCount = 0;
    public int wingsValue = 0;
    [SerializeField] private GameObject wingPrefab;
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
        scoreType.finishCollectedWings = 0;
        StartTextReset();   // ana ekrandaki text yazıları gunceller
        CharacterWings();
    }
    void StartTextReset()
    {
        // ana ekrandaki text yazıları gunceller
        UIController.uicontrollerInstance.GamePlayCoinText.text = scoreType.totalCoin.ToString();
        UIController.uicontrollerInstance.CollectedWingsText.text = collectedWingsCount.ToString();
    }
    void Update()
    {
        
    }
    public void AddCoin(int value)
    {
        // Altın Ekler
        Debug.Log("Coin added");
        int coinValue = scoreType.finishCollectedWings * value; // Gelen value degerini ile oyun bitiminde toplanan kanat sayılarını carp toplamı altın olarak ekle
        AudioController.audioControllerInstance.Play("CoinSound");  // Coin ses acar
        scoreType.totalCoin+=coinValue;
        UIController.uicontrollerInstance.WinCoinText.text = coinValue.ToString(); // Coin ekler ve text gunceller
        StartCoroutine(nameof(WinPanelOpen));
    }
    IEnumerator WinPanelOpen()
    {
        yield return new WaitForSeconds(2);
        UIController.uicontrollerInstance.WinPanelActive();
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
        collectedWingsCount+=3;  // Toplanan kanat sayısını arttır
        UIController.uicontrollerInstance.CollectedWingsText.text = collectedWingsCount.ToString(); // Toplanan kanat sayılarını karakter üzerinde göster
        StartCoroutine(WingsIsActive(obj)); // Toplanan objeyi pasif hale getir
        WingsValueCalculate();  // Kaç kanadın aktif olacağı sayı dönderir
        player.GetComponent<PlayerController>().CollectedLetfWing.ElementAt(wingsValue).gameObject.SetActive(true); // wingsValue degerine sahip kanat aktif hale gelir
        player.GetComponent<PlayerController>().CollectedRigthWing.ElementAt(wingsValue).gameObject.SetActive(true);   // wingsValue degerine sahip kanat aktif hale gelir
        AudioController.audioControllerInstance.Play("WingsSound");
    }
    public void WingsSubtract()
    {
        // Fonksiyon her çalıştığında bir adet kanat pasif hale gelir
        collectedWingsCount -= 3;  // Toplanan kanat sayısını arttır
        if (collectedWingsCount <= 0)
        {
            collectedWingsCount = 0;
        }
        UIController.uicontrollerInstance.CollectedWingsText.text = collectedWingsCount.ToString(); // Toplanan kanat sayılarını karakter üzerinde göster
        WingsValueCalculate(); // Kaç kanadın aktif olacağı sayı dönderir
        FailWings(
            player.GetComponent<PlayerController>().CollectedLetfWing.ElementAt(wingsValue + 1).gameObject.transform, 
            player.GetComponent<PlayerController>().CollectedRigthWing.ElementAt(wingsValue + 1).gameObject.transform); // Dusen kanatların pozisyonları verilir
        player.GetComponent<PlayerController>().CollectedLetfWing.ElementAt(wingsValue+1).gameObject.SetActive(false);  // wingsValue degerine sahip kanat pasif hale gelir
        player.GetComponent<PlayerController>().CollectedRigthWing.ElementAt(wingsValue+1).gameObject.SetActive(false); // wingsValue degerine sahip kanat pasif hale gelir
    }
    IEnumerator WingsIsActive(GameObject obj)
    {
        obj.SetActive(false);   // Toplanan kanadı  pasif hale getir
        yield return new WaitForSeconds(2);
        obj.SetActive(true);    // Toplanan kanadı 2 saniye sonra aktif hale getir
    }
    void FailWings(Transform Left,Transform Right)
    {
        // Her kanat ciktiginda kanat yere duser
        GameObject LeftWing = Instantiate(wingPrefab, Left.position, Quaternion.identity);  // Sol kanadın dusecegi posizyonda kanat yarat
        GameObject RightWing = Instantiate(wingPrefab, Right.position, Quaternion.identity);// Sag kanadın dusecegi posizyonda kanat yarat
        LeftWing.AddComponent<Rigidbody>().useGravity = true;
        RightWing.AddComponent<Rigidbody>().useGravity = true;
        Destroy(LeftWing, 1);
        Destroy(RightWing, 1);
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
