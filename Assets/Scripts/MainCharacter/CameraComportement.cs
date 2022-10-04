using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComportement : MonoBehaviour
{

    public GameObject OwnCharacter;
    public float _Lag, _LagRotation;
    Vector3 Direction;
    Quaternion DirectionQuat;

    private void FixedUpdate()
    {
        Direction = OwnCharacter.GetComponent<Rigidbody>().velocity;
        DirectionQuat = Quaternion.Euler(Direction);


        transform.localPosition = Vector3.Lerp(transform.localPosition, OwnCharacter.transform.position, Time.deltaTime * _Lag);

        
        transform.localRotation = Quaternion.Lerp(transform.localRotation, DirectionQuat, Time.deltaTime * _LagRotation);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
