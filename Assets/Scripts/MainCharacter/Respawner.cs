using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    Transform playerTransform;
    Transform playerRespawnPoint;

    private void Awake()
    {
        playerTransform = FindObjectOfType<CharacterAbilities>().transform;
        playerRespawnPoint = transform.GetChild(0);
    }


    void Update()
    {
        if (playerTransform.position.y < transform.position.y)
        {

            playerTransform.position = playerRespawnPoint.position;


        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 positions = (Vector3.right * 300) + (Vector3.forward * 300);

        Gizmos.DrawWireCube(Vector3.up * transform.position.y, positions);

        positions.y = transform.position.y;
        positions.x = 0;
        for (int i = -150; i < 149; i+= 5)
        {
            positions.z = i;
            Gizmos.DrawLine(positions + (Vector3.right * 150), positions + (Vector3.left * 150));
        }


        positions.z = 0;
        for (int i = -150; i < 149; i += 5)
        {
            positions.x = i;
            Gizmos.DrawLine(positions + (Vector3.forward * 150), positions + (Vector3.back * 150));
        }

    }
}
