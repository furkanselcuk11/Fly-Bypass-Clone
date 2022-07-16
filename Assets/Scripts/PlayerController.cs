using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private CharacterSO characterType = null;    // Scriptable Objects eriþir 

    private Rigidbody rb;
    private Animator anim;
    [Space]
    [Header("Player Controller")]
    [SerializeField] private float _speed = 10f;    // Karaker hizi
    [SerializeField] private float _flySpeed = 100f;   // Karakter zıplama gucu 
    [SerializeField] private bool _isMove;   // Zıplama aktif mi
    [SerializeField] private bool _isGround;   // Zıplama aktif mi
    [SerializeField] private bool _isFly;   // Zıplama aktif mi
    [SerializeField] private float _flyTime;  // Kalkan süresi
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
        //flyTime=characterType.flyTime;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (!GameManager.gamemanagerInstance.isFinish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Ekrana tıklandığında            
                TapToStart();
            }
            if (Input.GetMouseButton(0) && GameManager.gamemanagerInstance.gameStart && _isGround)
            {
                // Ekrana basılı tutulduğunda
                _isMove = true; // Running aktif olur
                //anim.SetBool("Running", true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // Ekrana dokunma bırakıldığında
                _isMove = false; // Running pasif olur 
                //anim.SetBool("Running", false);
            }
        }
        CharacterAnim();
    }
    private void FixedUpdate()
    {
        if (GameManager.gamemanagerInstance.gameStart)
        {
            if (!GameManager.gamemanagerInstance.isFinish && _isMove)
            {
                Move();
                // eger gameStart , isRunning true ve isFinish false ise (oyun baslamıs) hareker et
            }
            else if (!GameManager.gamemanagerInstance.isFinish && !_isMove)
            {
                rb.velocity = Vector3.zero; // sabit kal
            }
            if (GameManager.gamemanagerInstance.isFinish && _isFly)
            {
                Fly();
                //eger gameStart true, _isFly true ve isFinish true ise (oyun baslamıs) uc
            }
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
    void WingsOpen()
    {
        Debug.Log("wingOpen");
        if (CollectedLetfWing.Count > 1)
        {
            for (int i = 0; i < CollectedLetfWing.Count; i++)
            {
                var firstItem = CollectedLetfWing.ElementAt(i);
                // Stack (Toplama) iþlemi sonrasý toplanan objelerin  sýralý þekilde gidiþini ve üstüne eklemesini ayarlar
                firstItem.position = new Vector3(Mathf.Lerp(firstItem.position.x, firstItem.position.x - (i * diffBetweenItems), 1f),
                     firstItem.position.y,
                    firstItem.position.z);

            }
        }
        if (CollectedRigthWing.Count > 1)
        {
            for (int i = 0; i < CollectedRigthWing.Count; i++)
            {
                var firstItem = CollectedRigthWing.ElementAt(i);
                // Stack (Toplama) iþlemi sonrasý toplanan objelerin  sýralý þekilde gidiþini ve üstüne eklemesini ayarlar
                firstItem.position = new Vector3(Mathf.Lerp(firstItem.position.x, firstItem.position.x + (i * diffBetweenItems), 1f),
                     firstItem.position.y,
                    firstItem.position.z);
            }
        }
    }
    public void WingsClose()
    {
        Debug.Log("wingClose");
        if (CollectedLetfWing.Count > 1)
        {
            for (int i = 0; i < CollectedLetfWing.Count; i++)
            {
                var firstItem = CollectedLetfWing.ElementAt(i);
                // Stack (Toplama) iþlemi sonrasý toplanan objelerin  sýralý þekilde gidiþini ve üstüne eklemesini ayarlar
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
                // Stack (Toplama) iþlemi sonrasý toplanan objelerin  sýralý þekilde gidiþini ve üstüne eklemesini ayarlar
                firstItem.position = new Vector3(Mathf.Lerp(firstItem.position.x, firstItem.position.x - (i * diffBetweenItems), 1f),
                     firstItem.position.y,
                    firstItem.position.z);
            }
        }
    }
    public void TapToStart()
    {
        // oyunu baslatmak icin ekrana tıklanır     
        GameManager.gamemanagerInstance.gameStart = true;   // gameStart aktif olur
        UIController.uicontrollerInstance.GamePlayActive(); // GamePlay alanındaki textler akif olur
    }
    void Move()
    {
        transform.Translate(0, 0, _speed * Time.fixedDeltaTime); // Karakter speed degeri hizinda ileri hareket eder
    }
    void Fly()
    {
        //flyEffect.Play(); // Ucarken Flying efekti calisir  
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 3f, transform.position.z), 1f*Time.fixedDeltaTime);
        transform.Translate(0, 0, _flySpeed * Time.fixedDeltaTime); // Karakter speed degeri hizinda ileri hareket eder

        _flyTime -= Time.deltaTime;        
        if (_flyTime <= 0)
        {
            rb.useGravity = true;            
        }
    }
    IEnumerator WingsOpenClose()
    {
        WingsOpen();
        wings.eulerAngles=new Vector3(0, 0, 0); // Kanatlarin acisi acildiginda 0 derece olur
        yield return new WaitForSeconds(_flyTime+0.8f);
        WingsClose();
        wings.eulerAngles = new Vector3(-90, 0, 0); // Kanatlarin acisi kapandiginda -90 derece olur
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // Finish cizgisine  girmis ise
            GameManager.gamemanagerInstance.isFinish = true;
            _isGround = false;
            _isFly = true;
            _isMove = false;
            rb.useGravity = false;
            StartCoroutine(nameof(WingsOpenClose)); // Kanatları ac ve kapa
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground");
            GameManager.gamemanagerInstance.isFinish = false;
            _isGround = true;
            _isFly = false;
            //_flyTime = 3f;
        }
    }
}
