using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    private Animator anim;
    private Rigidbody rb;

    [SerializeField] private bool _isMove;   // Hareket aktif mi
    [SerializeField] private bool _isGround;   // Zemine temas etti mi
    [SerializeField] private bool _isFly;   // Ucma aktif mi
    public float flyTime;  // Ucma süresi
    [SerializeField] private float radius=2f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        _isFly = false;
        _isGround = true;
        anim = GetComponent<Animator>();
    }    
    void Update()
    {
        EnemyAnim();
    }
    void FindClosetWing()
    {
        float distanceToClosetWing = Mathf.Infinity;
        Wing closetWing = null;
        Wing[] wings = GameObject.FindObjectsOfType<Wing>();
        foreach (Wing currentWing in wings)
        {
            float distanceToWing = (currentWing.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToWing < distanceToClosetWing)
            {
                distanceToClosetWing = distanceToWing;
                closetWing = currentWing;
            }
        }
        Debug.DrawLine(this.transform.position, closetWing.transform.position);
        agent.SetDestination(new Vector3(closetWing.transform.position.x,this.transform.position.y,this.transform.position.z));
    }
    private void FixedUpdate()
    {
        if (GameManager.gamemanagerInstance.gameStart & !GameManager.gamemanagerInstance.isFinish & _isGround)
        {
            this.transform.Translate(0, 0, agent.speed * Time.fixedDeltaTime); // Karakter speed deðeri hýzýdna ileri hareket eder
            _isMove = true; // Running aktif olur
            FindClosetWing();
        }
        else
        {
            _isMove = false; // Running pasifr
        }
    }
    void EnemyAnim()
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // Finish cizgisine  girmis ise
            _isGround = false;
            //flyTime = (float)GameManager.gamemanagerInstance.wingsValue;   // Karakterin uçma süresi toplandığı kanat sayısına eşit olacak
            _isFly = true;
            _isMove = false;
            rb.useGravity = false;
            //StartCoroutine(nameof(WingsOpenClose)); // Kanatları ac ve kapa
            //StartCoroutine(nameof(WingsSubcartCoroutine));
        }
        if (other.CompareTag("Collect"))
        {
            // Eger yerde toplancak kanata temas edilmis ise
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
            _isFly = false;     
        }
    }
}
