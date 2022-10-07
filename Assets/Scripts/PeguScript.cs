using UnityEngine;
using UnityEngine.Rendering;

public class PeguScript : MonoBehaviour
{
    public enum PéguIAState { Idle, Moving, RunningAway, Sticking};

    PéguIAState currentState;


    [SerializeField] float distanceToDetectPlayer;


    [SerializeField] SpriteRenderer sr;
    [SerializeField] float regularMovementSpeed, runningMovementSpeed;
    [SerializeField] LayerMask playerLayer;

    Color currentColor;

    float currentStateTimer;

    [SerializeField] Vector2 idleTimerTimes, movementTimerTimes;
    Vector3 DirectionToMoveTowards;

    [SerializeField] float TimeBetweenCheckForPlayer, timeToRunFor;

    Collider[] overlapResults;

    Transform detectedPlayer;

    Animator animator;

    [SerializeField] float CullingDistance;
    bool shouldBeActive;

    [SerializeField] float stickingTimer;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        RandomlyChangeColor();
        StartIdling();

        // Do the first check in a random time between 0 & 2 Timer
        // So that every Pégu has a random check time
        // And not do the calls of the 80000 Pégu at the same frame
        Invoke("CheckForPlayer", Random.Range(0, 2*TimeBetweenCheckForPlayer));

        overlapResults = new Collider[3];
    }

    private void Update()
    {
        if (shouldBeActive) // Pégu should have AI
        {

            // Do a Billboard effect if not sticking to the cube
            if (currentState != PéguIAState.Sticking)
            {
                transform.forward = Camera.main.transform.forward;
            }
            

            // Pégu is too far away
            if ((Camera.main.transform.position - transform.position).sqrMagnitude > MathPlus.FastSquare(CullingDistance))
            {
                // Disable AI
                shouldBeActive = false;

                // Disable Animation
                animator.enabled = false;

            }
            else
            {   // Do AI

                // Reduce timer
                currentStateTimer -= Time.deltaTime;

                if (currentStateTimer <= 0)
                {
                    switch (currentState)
                    {
                        case PéguIAState.Idle:
                            StartMoving();
                            break;

                        case PéguIAState.Moving:
                            StartIdling();
                            break;

                        case PéguIAState.RunningAway:
                            MaybeStopRunning();
                            break;

                        case PéguIAState.Sticking:
                            Destroy(gameObject);
                            break;

                    }
                }

                // Walk or Run away

                if (currentState == PéguIAState.Moving)
                {
                    rb.velocity = DirectionToMoveTowards * regularMovementSpeed;
                    
                    // transform.position += DirectionToMoveTowards * regularMovementSpeed * Time.deltaTime;


                    FlipXOrNot();

                }
                else if (currentState == PéguIAState.RunningAway)
                {
                    // transform.position += DirectionToMoveTowards * runningMovementSpeed * Time.deltaTime;

                    rb.velocity = DirectionToMoveTowards * runningMovementSpeed;

                    FlipXOrNot();

                }

            }
        }
        else // Pégu shouldn't have AI
        {
            // Pégu is close enough
            if ((Camera.main.transform.position - transform.position).sqrMagnitude < MathPlus.FastSquare(CullingDistance))
            {
                // Enable AI
                shouldBeActive = true;

                // Re-Enable Animation
                animator.enabled = true;
            }
        }
        
    }


    void FlipXOrNot()
    {
        // Calculate the Quaternion to rotate a vector by the difference between transform.forward & World.forward
        var rotationToWorld = Quaternion.FromToRotation(transform.forward, Vector3.forward);

        // Rotate the direction with the Quaternion to "transfer" it into the coordinate system of the Transform
        var rotatedDirection = rotationToWorld * DirectionToMoveTowards;

        // Just analyse the x component: > 0 => moves to the Pégu's Right, and the other way around

        if (rotatedDirection.x >= 0)
        {
            // Unflip when going to the right

            sr.flipX = false;
        }
        else
        {
            // Flip when going to the left
            sr.flipX = true;
        }
    }


    /*
         IA of the Pégu:
            
            Roll a timer for a time before moving (medium-long)
            When it's done, they roll a random direction to move to and a time to move for
            Repeat the cycle
    */

    void StartIdling()
    {
        animator.Play("Pegu_IdleAnim");
        animator.speed = 1;

        // Change state to idle
        currentState = PéguIAState.Idle;

        // Start Timer
        currentStateTimer = Random.Range(idleTimerTimes.x, idleTimerTimes.y);

        rb.velocity = Vector3.zero;
    }

    void StartMoving()
    {
        animator.Play("Pegu_MovementAnim");
        animator.speed = 1;

        // Change state to idle
        currentState = PéguIAState.Moving ;


        // Create a random 2d vector with only X and Z in parameters
        DirectionToMoveTowards = Random.insideUnitSphere;
        DirectionToMoveTowards.y = 0;
        DirectionToMoveTowards.Normalize();




        // Start Timer
        currentStateTimer = Random.Range(movementTimerTimes.x, movementTimerTimes.y);
    }


    void CheckForPlayer()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, distanceToDetectPlayer, overlapResults, playerLayer, QueryTriggerInteraction.Ignore) != 0)
        {
            // Detected Player!

            PlayerDetected(overlapResults[0].transform);
        }
        else
        {
            // Didn't detect a Player!
            Invoke("CheckForPlayer", TimeBetweenCheckForPlayer);
        }
    }


    void PlayerDetected(Transform playerTransform)
    {
        animator.Play("Pegu_MovementAnim");
        animator.speed = 2f;
        detectedPlayer = playerTransform;

        currentState = PéguIAState.RunningAway;

        // Move straight away;
        DirectionToMoveTowards = transform.position - detectedPlayer.position;
        DirectionToMoveTowards.y = 0;
        DirectionToMoveTowards.Normalize();

        currentStateTimer = timeToRunFor;
    }

    void MaybeStopRunning()
    {
        if ((transform.position - detectedPlayer.position).magnitude > distanceToDetectPlayer)
        {
            // Player is too far away

            // Start idling again

            StartIdling();
            Invoke("CheckForPlayer", TimeBetweenCheckForPlayer);
        }
        else
        {
            // Player is still close


            // Change direction to run towards
            DirectionToMoveTowards = transform.position - detectedPlayer.position;
            DirectionToMoveTowards.y = 0;
            DirectionToMoveTowards.Normalize();

            currentStateTimer = timeToRunFor;
        }
    }


    /// <summary> Changes the sprite's color to anything highly saturated </summary>
    void RandomlyChangeColor()
    {
        currentColor = Color.HSVToRGB(Random.Range(0, 1f), Random.Range(0.75f, 1f), Random.Range(0.75f, 1f));
        sr.color = currentColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanceToDetectPlayer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Stick to player
            StickToPlayer(collision.gameObject);
        }
    }



    void StickToPlayer(GameObject _player)
    {
        // Align to rotation
        // transform.rotation = _player.transform.rotation;

        rb.velocity = Vector3.zero;

        // Change State to "dead"
        currentState = PéguIAState.Sticking;

        // Remove Animation
        animator.Play("New_State");
        animator.enabled = false;

        // Disable the Collider
        GetComponent<SphereCollider>().enabled = false;

        transform.SetParent(_player.transform);


        // get closer to the cube
        transform.localPosition /= 1.7f;



        currentStateTimer = stickingTimer;
    }

}
