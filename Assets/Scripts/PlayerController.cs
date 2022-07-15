using System.Collections;
using System.Collections.Generic;
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
                anim.SetBool("Running", true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // Ekrana dokunma bırakıldığında
                _isMove = false; // Running pasif olur 
                anim.SetBool("Running", false);
            }
        }
                
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
            anim.SetBool("Flying", true);
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
            _flyTime = 3f;
            anim.SetBool("Flying", false);
        }
    }
}
