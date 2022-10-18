using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PeguScript : MonoBehaviour
{
    public enum PeguIAState { Idle, Moving, RunningAway, Sticking, Paused};

    PeguIAState currentState;

    [SerializeField] float distanceToDetectPlayer;
    [SerializeField] float gravityValue;


    [SerializeField] SpriteRenderer sr;
    [SerializeField] float regularMovementSpeed, runningMovementSpeed;
    [SerializeField] LayerMask playerLayer;

    Color currentColor;

    float currentStateTimer;

    [SerializeField] Vector2 idleTimerTimes, movementTimerTimes;
    public Vector3 DirectionToMoveTowards;

    [SerializeField] float TimeBetweenCheckForPlayer, timeToRunFor;

    Collider[] overlapResults;

    Transform detectedPlayer;

    Animator animator;

    [SerializeField] float CullingDistance;
    bool shouldBeActive;

    [SerializeField] float stickingTimer;

    Rigidbody rb;
    PeguIAState previousState;


    Transform secondaryEventCamera;
    float previousTimer;

    BillboardScript billboardScript;
    AudioSource _audioSource;

    float timerBeforeWoo;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        billboardScript = GetComponent<BillboardScript>();
        billboardScript.isBillboardActive = true;

        RandomlyChangeColor();
        StartIdling();

        // Do the first check in a random time between 0 & 2 Timer
        // So that every P�gu has a random check time
        // And not do the calls of the 80000 P�gu at the same frame
        Invoke("CheckForPlayer", Random.Range(0, 2*TimeBetweenCheckForPlayer));

        overlapResults = new Collider[3];
        ReinitializeTimerBeforeWoo();
        timerBeforeWoo -= 18f;
    }

    private void Update()
    {
        if (currentState == PeguIAState.Paused)
        {
            // Don't rotate if should stick
            if (previousState != PeguIAState.Sticking) { transform.forward = secondaryEventCamera.forward; }
            
            return;
        }
        if (shouldBeActive) // P�gu should have AI
        {
            

            // P�gu is too far away
            if ((Camera.main.transform.position - transform.position).sqrMagnitude > MathPlus.FastSquare(CullingDistance))
            {
                // Disable AI
                shouldBeActive = false;

                // Disable Animation
                animator.enabled = false;

            }
            else
            {   // Do AI

                timerBeforeWoo -= Time.deltaTime;

                if (timerBeforeWoo <= 0)
                {
                    if (currentState == PeguIAState.RunningAway)
                    {
                        _audioSource.pitch = Random.Range(1.5f, 2f);
                    }
                    else
                    {
                        _audioSource.pitch = Random.Range(0.9f, 1.25f);
                    }
                    
                    _audioSource.Play();
                    ReinitializeTimerBeforeWoo();
                }

                // Reduce timer
                currentStateTimer -= Time.deltaTime;

                if (currentStateTimer <= 0)
                {
                    switch (currentState)
                    {
                        case PeguIAState.Idle:
                            StartMoving();
                            break;

                        case PeguIAState.Moving:
                            StartIdling();
                            break;

                        case PeguIAState.RunningAway:
                            MaybeStopRunning();
                            break;

                        case PeguIAState.Sticking:
                            Destroy(gameObject);
                            break;

                    }
                }

                // Walk or Run away

                if (currentState == PeguIAState.Moving)
                {
                    rb.velocity = DirectionToMoveTowards * regularMovementSpeed;
                    // rb.velocity += Vector3.down * gravityValue;

                    // rb.velocity = Vector3.zero;



                    FlipXOrNot();

                }
                else if (currentState == PeguIAState.RunningAway)
                {

                    rb.velocity = DirectionToMoveTowards * runningMovementSpeed;
                    timerBeforeWoo -= Time.deltaTime;
                    // rb.velocity += Vector3.down * gravityValue;

                    // rb.velocity = Vector3.zero;
                    

                    FlipXOrNot();

                }

            }
        }
        else // P�gu shouldn't have AI
        {
            // P�gu is close enough
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

        // Just analyse the x component: > 0 => moves to the P�gu's Right, and the other way around

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
         IA of the P�gu:
            
            Roll a timer for a time before moving (medium-long)
            When it's done, they roll a random direction to move to and a time to move for
            Repeat the cycle
    */

    void StartIdling()
    {
        if (currentState == PeguIAState.Sticking) { return; }
        animator.Play("Pegu_IdleAnim");
        animator.speed = 1;

        // Change state to idle
        currentState = PeguIAState.Idle;

        // Start Timer
        currentStateTimer = Random.Range(idleTimerTimes.x, idleTimerTimes.y);

        rb.velocity = Vector3.zero;
    }

    void StartMoving()
    {
        if (currentState == PeguIAState.Sticking) { return; }
        animator.Play("Pegu_MovementAnim");
        animator.speed = 1;

        // Change state to idle
        currentState = PeguIAState.Moving ;


        // Create a random 2d vector with only X and Z in parameters
        DirectionToMoveTowards = Random.insideUnitSphere;
        DirectionToMoveTowards.y = 0;
        DirectionToMoveTowards.Normalize();




        // Start Timer
        currentStateTimer = Random.Range(movementTimerTimes.x, movementTimerTimes.y);
    }


    void CheckForPlayer()
    {
        if (currentState == PeguIAState.Sticking || currentState == PeguIAState.Paused) { Invoke("CheckForPlayer", TimeBetweenCheckForPlayer); return; }
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

        currentState = PeguIAState.RunningAway;

        // Move straight away;
        DirectionToMoveTowards = transform.position - detectedPlayer.position;
        DirectionToMoveTowards.y = 0;
        DirectionToMoveTowards.Normalize();

        currentStateTimer = timeToRunFor;

        _audioSource.pitch = Random.Range(1.5f, 2f);
        _audioSource.Play();
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

        billboardScript.isBillboardActive = false;
        // Debug.Log("Sticky Time");

        // Align to rotation
        // transform.rotation = _player.transform.rotation;

        rb.velocity = Vector3.zero;

        // Change State to "dead"
        currentState = PeguIAState.Sticking;

        // Remove Animation
        animator.Play("Pegu_NoAnimation");
        

        // Disable the Collider
        GetComponent<BoxCollider>().enabled = false;


        // Remove the RigidBody of the Pégu
        Destroy(rb);

        transform.SetParent(_player.transform);


        // get closer to the cube
        transform.localPosition /= 1.4f;

        // Disable Animator a bit later
        animator.enabled = false;

        currentStateTimer = stickingTimer;
    }

    void PausePegu()
    {
        animator.Play("Pegu_NoAnimation");
        previousState = currentState;
        currentState = PeguIAState.Paused;
        billboardScript.isBillboardActive = false;

        secondaryEventCamera = FindObjectOfType<AllCoinsChallenge>().transform.GetChild(0).GetChild(0);
        previousTimer = currentStateTimer;
        currentStateTimer = 10000;
    }

    void UnpausePegu()
    {
        currentState = previousState;
        currentStateTimer = previousTimer;

        switch (currentState)
        {
            case PeguIAState.Idle:
                animator.Play("Pegu_IdleAnim");
                billboardScript.isBillboardActive = true;
                break;

            case PeguIAState.Moving:
                billboardScript.isBillboardActive = true;
                animator.Play("Pegu_MovementAnim");
                break;

            case PeguIAState.RunningAway:
                billboardScript.isBillboardActive = true;
                animator.Play("Pegu_MovementAnim");
                break;

            case PeguIAState.Sticking:
                animator.Play("Pegu_NoAnimation");
                break;
        }

    }

    private void OnEnable()
    {
        GeneralEventManager.PauseGameplay += PausePegu;
        GeneralEventManager.ResumeGameplay += UnpausePegu;
        
    }

    private void OnDisable()
    {
        GeneralEventManager.PauseGameplay -= PausePegu;
        GeneralEventManager.ResumeGameplay -= UnpausePegu;
    }

    void ReinitializeTimerBeforeWoo()
    {
        timerBeforeWoo = Random.Range(25f, 125f);
    }

}
