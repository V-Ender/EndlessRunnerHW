using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collect : MonoBehaviour
{
    [SerializeField] AudioSource collectableFX;

    void OnTriggerEnter(Collider other)
    {
        collectableFX.Play();
        MasterInfo.gemCount += 1;
        this.gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
