using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SkySphere : MonoBehaviour
{

    public GameObject Pawn;

    private void Update()
    {
        transform.position = new Vector3(Pawn.transform.position.x,Pawn.transform.position.y - 80,Pawn.transform.position.z);
    }






}
