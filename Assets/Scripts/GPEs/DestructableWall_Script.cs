using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall_Script : MonoBehaviour
{
    [SerializeField] GameObject destructionParticles;

    public const float speedDestructionThreshold = 6f;


    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.rigidbody.velocity.sqrMagnitude);


        if (collision.gameObject.tag == "Player" && collision.rigidbody.velocity.sqrMagnitude > speedDestructionThreshold)
        {
            // get the direction of the movement of the Player colliding
            Vector3 impactInertia = collision.rigidbody.velocity;

            // make it vertical non-dependant
            // impactInertia[1] = 0;
            impactInertia = impactInertia.normalized;

            // Spawn particle
            GameObject _particle = Instantiate(destructionParticles, collision.GetContact(0).point, Quaternion.identity);
            _particle.transform.forward = impactInertia;


            Destroy(gameObject);
        }
    }
}
