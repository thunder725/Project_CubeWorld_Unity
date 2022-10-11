using UnityEngine;

public class Oazo_Script : MonoBehaviour
{
    [SerializeField] float flightSpeed;

    Vector3 FlightDirection = Vector3.forward;
    [SerializeField] float CullingDistance;


    // ======================== [DEFAULT UNITY METHODS] ========================

    void Update()
    {
        transform.forward = Camera.main.transform.forward;

        transform.position += FlightDirection * flightSpeed * Time.deltaTime;


        // If the bird is too far away
        if ((transform.position - Camera.main.transform.position).sqrMagnitude > (CullingDistance * CullingDistance))
        {
            Destroy(gameObject);
        }
    }

    // ======================== [SCRIPT METHODS] ========================



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(transform.position, CullingDistance);
    }


}
