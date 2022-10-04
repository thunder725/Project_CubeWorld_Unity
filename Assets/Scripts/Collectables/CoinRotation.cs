using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{

    [SerializeField] float rotationSpeed;


    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0,  0);
    }
}
