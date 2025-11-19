using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;

    void OnTriggerEnter(Collider other)
    {
        thePlayer.GetComponent<PlayerMovement>().Die();
    }
}
