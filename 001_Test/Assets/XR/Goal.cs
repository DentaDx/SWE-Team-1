using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
    private void Awake()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cock"))
        {
            FindObjectOfType<GameManager>().AddScore();
        }
    }

}
