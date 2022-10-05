using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserScript : MonoBehaviour
{
    public LayerMask playerLayer;
    public float pushStrength;

    CapsuleCollider capsuleCollider;

    [SerializeField] float OverlapFrequencies;
    float currentTimer;

    Vector3 capsulePoint1, capsulePoint2;
    public float capsuleRadius;

    GameObject ParticlesGameObject;

    [SerializeField] float CullingDistance;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        GetCapsuleValues();

        ParticlesGameObject = transform.GetChild(0).gameObject;
    }

    void GetCapsuleValues()
    {

        capsuleRadius = capsuleCollider.radius;
        capsulePoint1 = capsuleCollider.bounds.center;
        capsulePoint2 = capsuleCollider.bounds.center;

        capsulePoint1.y -= (capsuleCollider.height / 2)*transform.localScale.y;
        capsulePoint2.y += (capsuleCollider.height / 2)*transform.localScale.y;

        capsuleCollider.enabled = false;
    }


    void Update()
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            ResetTimer();
            CheckForPlayer();
        }

        // If active, try to unload it
        if (ParticlesGameObject.activeInHierarchy)
        {
            if ((Camera.main.transform.position - transform.position).sqrMagnitude > MathPlus.FastSquare(CullingDistance))
            {
                ParticlesGameObject.SetActive(false);
            }

        }
        else
        {
            if ((Camera.main.transform.position - transform.position).sqrMagnitude < MathPlus.FastSquare(CullingDistance))
            {
                ParticlesGameObject.SetActive(true);
            }
        }
    }


    void ResetTimer()
    {
        if (OverlapFrequencies == 0) { OverlapFrequencies = 1; }
        currentTimer = 1 / OverlapFrequencies;
    }

    // Script won't have a collider per-se to save ressources
    // it'll have a OverlapCapsule

    void CheckForPlayer()
    {

        var overlapResults = Physics.OverlapCapsule(capsulePoint1, capsulePoint2, capsuleRadius, playerLayer);


        if (overlapResults.Length != 0)
        {
            // Debug.Log("Player Found");
            // reset Velocity & PUSH
            overlapResults[0].attachedRigidbody.velocity = (Vector3.up * pushStrength);
        }
        
    }
            
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(capsulePoint1, capsuleRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(capsulePoint2, capsuleRadius);

    }
}
