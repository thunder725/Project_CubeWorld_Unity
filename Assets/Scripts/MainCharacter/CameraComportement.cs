using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraComportement : MonoBehaviour
{

    public GameObject OwnCharacter;
    public float _Lag, _LagRotation, _DistanceLag;
    Vector3 Direction;
    Quaternion DirectionQuat;

    private void FixedUpdate()
    {
        Direction = OwnCharacter.GetComponent<CharacterController>().ForwardVelocity.normalized;
        ;
        print(Direction);

        transform.parent.transform.forward  = Vector3.Lerp(transform.parent.transform.forward, Direction, _LagRotation * Time.deltaTime);

        transform.parent.transform.position = Vector3.Lerp(transform.parent.transform.position, OwnCharacter.transform.position, Time.deltaTime * _Lag);

        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
