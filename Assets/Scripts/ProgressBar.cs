using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject _playerPos;
    [SerializeField] private GameObject _finishPoint;
    Image progresBar;
    float maxDistance;
    void Start()
    {
        progresBar = GetComponent<Image>();
        maxDistance = _finishPoint.transform.position.z;
        progresBar.fillAmount = _playerPos.transform.position.z / maxDistance;
    }

    
    void Update()
    {
        if (progresBar.fillAmount < 1)
        {
            progresBar.fillAmount = _playerPos.transform.position.z / maxDistance;
        }        
    }
}
