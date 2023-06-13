using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cockPrefab;
    [SerializeField] private float _forceAmount;

    [SerializeField] private float _randX, _randZ;
    private void Start()
    {
        SpawnCock();
    }
    public void SpawnCock()
    {
        //instantiate cock
        //ntar ganti ke set pos aja
        GameObject go = Instantiate(_cockPrefab, transform.position, transform.rotation);

        //addforce
        Vector3 vec = new Vector3(Random.Range(-1 * _randX, _randX), 0, Random.Range(-1 * _randZ, _randZ));
        go.GetComponent<Rigidbody>().AddForce((Vector3.forward +vec).normalized * _forceAmount);
    }

    private Vector3 SetDirection()
    {
        //generate random direction to shoot the new cock

        return new Vector3();
    }
    
}
