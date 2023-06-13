using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] private float _hitForce;
    [SerializeField] private Transform _playerTf;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Cock"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce((collision.gameObject.transform.position - _playerTf.transform.position) * _hitForce);
        }
    }
}
