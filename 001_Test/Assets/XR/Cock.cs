using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cock : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("touch ground");
            //spawn new cock
            //ntar delete
            FindObjectOfType<CockSpawner>().SpawnCock();


            //ntar jangan di destroy, set pos aja
            Destroy(gameObject);
        }
    }
}
