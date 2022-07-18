using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ScoreSO scoreType = null;    // Scriptable Objects eriþir 

    private Rigidbody rb;
    private Animator anim;
    [Space]
    [Header("Player Controller")]
    [SerializeField] private float _speed = 10f;    // Karaker hizi
    [SerializeField] private float _horizontalspeed = 5f; // Player yön hareket hizi
    [SerializeField] private float _flySpeed = 100f;   // Karakter ucma hizi
    [SerializeField] private float _defaultSwipe = 1.8f;    // Player default kaydirma mesafesi
    [SerializeField] private bool _isMove;   // Hareket aktif mi
    [SerializeField] private bool _isGround;   // Zemine temas etti mi
    [SerializeField] private bool _isFly;   // Ucma aktif mi
    public float flyTime;  // Ucma süresi
    [SerializeField] private ParticleSystem _flyEffect;
    [SerializeField] private ParticleSystem _finishEffect;
    [Space]
    [Header("Collected Controller")]
    [SerializeField] Transform wings;
    public List<Transform> CollectedLetfWing = new List<Transform>();   // Toplanan objelerin listesi
    public List<Transform> CollectedRigthWing = new List<Transform>();   // Toplanan objelerin listesi
    [SerializeField] private float diffBetweenItems;    // Toplana objelerin yana kayma h?z? ve objeler aras? mesafesi
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _isFly = false;
        _isGround = true;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        CharacterAnim();
    }
    private void FixedUpdate()
    {
        if (GameManager.gamemanagerInstance.gameStart & !GameManager.gamemanagerInstance.isFinish & _isGround)
        {
            // Eðer StartGame ,_isGround true ve isFinish false ise hareket et
            transform.Translate(0, 0, _speed * Time.fixedDeltaTime); // Karakter speed deðeri hýzýdna ileri hareket eder
            _isMove = true; // Running aktif olur
            MoveInput();    // Player hareket kontrolü çalýþtýr
        }
        else
        {
            // Eðer StartGame False ise  hareket etmez
            _isMove = false; // Running pasif olur 
        }
        if(GameManager.gamemanagerInstance.isFinish && _isFly)
        {           
            Fly();
        }
    }
    void CharacterAnim()
    {
        if (_isMove)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
        if (_isFly)
        {
            anim.SetBool("Flying", true);
        }
        else
        {
            anim.SetBool("Flying", false);
        }
    }
    void MoveInput()
    {
        #region Mobile Controller 4 Direction

        float moveX = transform.position.x; // Player objesinin x pozisyonun de?erini al?r      
        float moveZ = transform.position.z; // Player objesinin z pozisyonun de?erini al?r           

        if (Input.GetKey(KeyCode.LeftArrow) || MobileInput.instance.swipeLeft)
        {   // Eðer klavyede sol ok tuþuna basýldýysa yada "MobileInput" scriptinin swipeLeft deðeri True ise  Sola hareket gider
            moveX = Mathf.Clamp(moveX - 1 * _horizontalspeed * Time.fixedDeltaTime, -_defaultSwipe, _defaultSwipe);    // Pozisyon sýnýrlandýrýlmasý koyulacaksa
            // Player objesinin x (sol) pozisyonundaki gideceði min-max sýnýrý belirler
        }
        else if (Input.GetKey(KeyCode.RightArrow) || MobileInput.instance.swipeRight)
        {   // Eðer klavyede sað ok tuþuna basýldýysa yada "MobileInput" scriptinin swipeRight deðeri True ise Saða hareket gider   
            moveX = Mathf.Clamp(moveX + 1 * _horizontalspeed * Time.fixedDeltaTime, -_defaultSwipe, _defaultSwipe);    // Pozisyon sýnýrlandýrýlmasý koyulacaksa
            // Player objesinin x (sað) pozisyonundaki gideceði min-max sýnýrý belirler
        }
        else
        {
            rb.velocity = Vector3.zero; // E?er hareket edilmediyse Player objesi sabit kals?n
        }

        transform.position = new Vector3(moveX, transform.position.y, moveZ);
        // Player objesinin pozisyonu moveX deðerine göre x ekseninde, moveZ deðerine göre z ekseninde hareket eder ve y ekseninde sabit kalýr  

        #endregion
    }
    void Fly()
    {
        //_flyEffect.Play(); // Ucarken Flying efekti calisir  
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 3f, transform.position.z), 1f * Time.fixedDeltaTime);
        transform.Translate(0, 0, _flySpeed * Time.fixedDeltaTime); // Karakter speed degeri hizinda ileri hareket eder

        flyTime -= Time.deltaTime; // Karakterin uçma süresi toplandığı kanat sayısına eşit olup zaman göre azalacaktır
        if (flyTime <= 0)
        {
            // 0 olduğu zaman yer çekimi aktif olup yere düşecektir
            rb.useGravity = true;
        }
    }
    void WingsOpen()
    {
        // Kanat Açma
        if (CollectedLetfWing.Count > 1)
        {
            for (int i = 0; i < CollectedLetfWing.Count; i++)
            {
                // Sol kanat
                var firstItem = CollectedLetfWing.ElementAt(i);
                // Stack (Toplama) islemi sonrasi toplanan objelerin  sirali sekilde gidisini ve üstüne eklemesini ayarlar
                firstItem.position = new Vector3(Mathf.Lerp(firstItem.position.x, firstItem.position.x - (i * diffBetweenItems), 1f),
                     firstItem.position.y,
                    firstItem.position.z);

            }
        }
        if (CollectedRigthWing.Count > 1)
        {
            for (int i = 0; i < CollectedRigthWing.Count; i++)
            {
                // Sağ kanat
                var firstItem = CollectedRigthWing.ElementAt(i);
                // Stack (Toplama) islemi sonrasi toplanan objelerin  sirali sekilde gidisini ve üstüne eklemesini ayarlar
                firstItem.position = new Vector3(Mathf.Lerp(firstItem.position.x, firstItem.position.x + (i * diffBetweenItems), 1f),
                     firstItem.position.y,
                    firstItem.position.z);
            }
        }
    }
    public void WingsClose()
    {
        // Kanat kapatma
        if (CollectedLetfWing.Count > 1)
        {
            for (int i = 0; i < CollectedLetfWing.Count; i++)
            {
                var firstItem = CollectedLetfWing.ElementAt(i);
                // Cikarma islemi sonrasi toplanan objelerin  sirali sekilde gidisini ve en ustten cikarmasini ayarlar
                firstItem.position = new Vector3(Mathf.Lerp(firstItem.position.x, firstItem.position.x + (i * diffBetweenItems), 1f),
                     firstItem.position.y,
                    firstItem.position.z);

            }
        }
        if (CollectedRigthWing.Count > 1)
        {
            for (int i = 0; i < CollectedRigthWing.Count; i++)
            {
                var firstItem = CollectedRigthWing.ElementAt(i);
                // Cikarma islemi sonrasi toplanan objelerin  sirali sekilde gidisini ve en ustten cikarmasini ayarlar
                firstItem.position = new Vector3(Mathf.Lerp(firstItem.position.x, firstItem.position.x - (i * diffBetweenItems), 1f),
                     firstItem.position.y,
                    firstItem.position.z);
            }
        }
    }   
    
    IEnumerator WingsOpenClose()
    {
        WingsOpen();    // Kanat ac
        wings.eulerAngles=new Vector3(0, 0, 0); // Kanatlarin acisini acildiginda 0 dereceye ayarlar
        yield return new WaitForSeconds(flyTime + 0.8f);
        WingsClose();   // Kanat kapat (flyTime suresi bittiginde)
        wings.eulerAngles = new Vector3(-90, 0, 0); // Kanatlarin acisini kapandiginda -90 dereceye ayarlar
        StopCoroutine(nameof(WingsSubcartCoroutine));   // Kanat çıkarmayı durdur
    }
    IEnumerator WingsSubcartCoroutine()
    {
        while (true)
        {            
            // her 1 sniyede bir kanat çıkar
            GameManager.gamemanagerInstance.WingsSubtract();
            yield return new WaitForSeconds(0.3f);
        }        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // Finish cizgisine  girmis ise
            GameManager.gamemanagerInstance.isFinish = true;
            _isGround = false;
            flyTime = (float)GameManager.gamemanagerInstance.wingsValue;   // Karakterin uçma süresi toplandığı kanat sayısına eşit olacak
            _isFly = true;
            _isMove = false;
            rb.useGravity = false;
            _flyEffect.Play(); // Ucarken Flying efekti calisir
            _finishEffect.Play(); // Ucarken Finish efekti calisir
            scoreType.finishCollectedWings = GameManager.gamemanagerInstance.collectedWingsCount;
            StartCoroutine(nameof(WingsOpenClose)); // Kanatları ac ve kapa
            StartCoroutine(nameof(WingsSubcartCoroutine));
            AudioController.audioControllerInstance.Play("FinishSound");
        }
        if (other.CompareTag("Collect"))
        {
            // Eger yerde toplancak kanata temas edilmis ise
            GameManager.gamemanagerInstance.WingsAdd(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.gamemanagerInstance.isFinish = false;
            _isGround = true;
            _isFly = false;
            _flyEffect.Stop(); // Ucarken Flying efekti durdur       
        }
        if (collision.gameObject.CompareTag("Award"))
        {
            // Finish alaninda uctuktan hangi odul rengine ulastigini ayarlar
            GameManager.gamemanagerInstance.isFinish = false;
            _isFly = false;
            _flyEffect.Stop(); // Ucarken Flying efekti durdur 
            int AwardName = int.Parse(collision.gameObject.name);
            switch (AwardName)
            {
                case 1:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 2:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 3:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 4:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 5:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 6:
                    GameManager.gamemanagerInstance.AddCoin(AwardName); ;
                    break;
                case 7:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 8:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 9:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
                case 10:
                    GameManager.gamemanagerInstance.AddCoin(AwardName);
                    break;
            }            
        }
    }
}
