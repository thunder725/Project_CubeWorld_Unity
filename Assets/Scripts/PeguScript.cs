using UnityEngine;

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


    private void Start()
    {
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
            transform.LookAt(Camera.main.transform.position, Vector3.up);

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
                    }
                }

                // Walk or Run away

                if (currentState == PéguIAState.Moving)
                {
                    transform.position += DirectionToMoveTowards * regularMovementSpeed * Time.deltaTime;
                }
                else if (currentState == PéguIAState.RunningAway)
                {
                    transform.position += DirectionToMoveTowards * runningMovementSpeed * Time.deltaTime;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Stick to player
            StickToPlayer(other.gameObject);
        }
    }

    void StickToPlayer(GameObject _player)
    {
        // Align to rotation
        transform.rotation = _player.transform.rotation;

        // 
        currentState = PéguIAState.Sticking;

        // Disable the Collider
        GetComponent<SphereCollider>().enabled = false;
    }

}
