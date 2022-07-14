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
    [SerializeField] private bool _isRunning;   // Zıplama aktif mi
    [SerializeField] private bool _isFly;   // Zıplama aktif mi
    [SerializeField] private float _flyTime;  // Kalkan süresi
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _isFly = false;
        //flyTime=characterType.flyTime;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Ekrana tıklandığında            
            TapToStart();
        }
        if (Input.GetMouseButton(0) && GameManager.gamemanagerInstance.gameStart)
        {
            // Ekrana basılı tutulduğunda
            _isRunning = true; // Running aktif olur
            anim.SetBool("Running", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Ekrana dokunma bırakıldığında
            _isRunning = false; // Running pasif olur 
            anim.SetBool("Running", false);
        }         
    }
    private void FixedUpdate()
    {
        if (GameManager.gamemanagerInstance.gameStart && !GameManager.gamemanagerInstance.isFinish && _isRunning)
        {
            Move();
            // eger gameStart , isRunning true ve isFinish false ise (oyun baslamıs) hareker et
        }
        else
        {
            rb.velocity = Vector3.zero; // sabit kal
        }
        if (GameManager.gamemanagerInstance.gameStart && _isFly && GameManager.gamemanagerInstance.isFinish)
        {
            Fly();
            //eger gameStart true, _isFly true ve isFinish true ise (oyun baslamıs) uc
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
        rb.AddForce(transform.up * _flySpeed * Time.fixedDeltaTime, ForceMode.Impulse);    // Yukari yonde zıplma gucu uygular
        rb.AddForce(transform.forward * _speed * Time.fixedDeltaTime, ForceMode.Impulse);    // Yukari yonde zıplma gucu uygular
        //flyEffect.Play(); // Ekrana basmayınca jetpack efekti calisir  
    }
}
