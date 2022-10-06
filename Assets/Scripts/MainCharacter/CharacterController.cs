using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    Rigidbody rb;
    InputController MainController;
    CharacterAbilities SCabilities;
    public Vector2 MovementValue, CameraValue;
    float Timer;
    public float Speed, _Mult, _DragMult;
    private float InitialSpeed;
    public Vector3 ForwardVelocity;
    public GameObject SupportCamera;
    public bool IsCamera;

    bool isFrozen;
    Vector3 savedVelocity, savedAngularVelocity;

    [SerializeField] AllCoinsChallenge coinsChallenge;

    [Header("Ground Detection")]

    [SerializeField] LayerMask groundLayers;
    [SerializeField] Vector3 groundDetectionStartingPoint;
    [SerializeField] Vector3 groundDetectionSize;
    [SerializeField] bool isGrounded;

    [SerializeField] Collider[] overlapResults;


    [Header("Jump Values")]

    [SerializeField] float jumpImpulsionStrength;


    [Header("Dash Values")]
    [SerializeField] float DashImpulsionStrength;
    [SerializeField] float DashDuration;
    bool isDashing;
    float currentDashDuration;

    [Header("Particle Systems")]
    [SerializeField] GameObject CoinPickupParticles;

    //start =====================================
    private void Awake()
    {
        SCabilities = GetComponent<CharacterAbilities>();
        rb = GetComponent<Rigidbody>();
        MainController = new InputController();

        

        overlapResults = new Collider[32];

        
    }

    private void Start()
    {
        InitialSpeed = Speed;

    }

    private void OnEnable()
    {
        MainController.Enable();

        GeneralEventManager.PauseGameplay += FreezeInPlace;
        GeneralEventManager.ResumeGameplay += Unfreeze;

        MainController.Gamepad.Abilities.performed += EventAbilities;
    }

    private void OnDisable()
    {
        MainController.Disable();

        GeneralEventManager.PauseGameplay -= FreezeInPlace;
        GeneralEventManager.ResumeGameplay -= Unfreeze;

        MainController.Gamepad.Abilities.performed -= EventAbilities;
    }

    // Per tick =============================================

    private void FixedUpdate()
    {
        #region Camera

        CameraValue = MainController.Gamepad.CameraDirection.ReadValue<Vector2>();

        if(CameraValue != Vector2.zero)
        {
            IsCamera = false;
        }
        else
        {
            Timer += Time.deltaTime;
            if (Timer >= 1.5f)
            {
                IsCamera = true;
            }
        }

        #endregion

        #region Movement
        
        MovementValue = MainController.Gamepad.Movement.ReadValue<Vector2>();

        if(MovementValue != Vector2.zero)
        {
            Vector3 _Velocity = rb.velocity;


            _Velocity.z = Speed * MovementValue.y;
            _Velocity.x = Speed * MovementValue.x;
            _Velocity.y = rb.velocity.y;

            
            rb.velocity = _Velocity.x * SupportCamera.transform.right + _Velocity.z * SupportCamera.transform.forward + _Velocity.y * Vector3.up;


            if (_Mult <= 3)
            {
                _Mult += Time.deltaTime * _DragMult;
            }
            
            if(InitialSpeed*3 <= Speed)
            {
                Speed = InitialSpeed*2;
            }
            else
            {
                Speed = InitialSpeed * _Mult;
            }

            ForwardVelocity = rb.velocity;

        }
        else
        {
            _Mult =0;
        }



        if (isDashing)
        {
            currentDashDuration -= Time.deltaTime;
            if (currentDashDuration <= 0)
            {
                isDashing = false;
            }
        }

        #endregion

        #region isGrounded

        // IMPORTANT
        /*
            The "overlapResults" doesn't empty itself if the OverlapBox doesn't hit anything
            BUT, the method will return 0 
            So I need to check for the returned value instead of the length of the array
        */
        if (Physics.OverlapBoxNonAlloc(transform.position + groundDetectionStartingPoint, groundDetectionSize / 2, overlapResults, Quaternion.identity, groundLayers, QueryTriggerInteraction.Ignore) > 0)
        {
            // Touching ground
            if (!isGrounded)
            {
                LandingOnGround();
            }
        }
        else
        {
            // Not touching ground
            if (isGrounded)
            {
                LeavingGround();
            }
        }

        

        #endregion

        if (isFrozen)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

    }

    void LandingOnGround()
    {
        isGrounded = true;
    }

    void LeavingGround()
    {
        isGrounded = false;
    }

    // Activation

    public void EventAbilities(InputAction.CallbackContext press)
    {
        if (isGrounded)
        {
            // Face under is red
            if (SCabilities.GetDownFaceColor() == 1)
            {
                PlayerDash();
            }
            else // Face under is Blue
            {
                PlayerJump();
            }
        }
        
        
    }


    void PlayerDash()
    {
        Vector3 _directionToDash = MovementValue;
        _directionToDash[2] = _directionToDash[1];
        _directionToDash[1] = 0;

        _directionToDash.Normalize();

        if (_directionToDash == Vector3.zero)
        {
            _directionToDash = SupportCamera.transform.forward;
        }

        rb.AddForce(_directionToDash * DashImpulsionStrength * 1000f);

        isDashing = true;
        currentDashDuration = DashDuration;

    }

    void PlayerJump()
    {
        rb.AddForce(Vector3.up * jumpImpulsionStrength * 1000f);
    }



    private void OnTriggerEnter(Collider other) {
        switch(other.gameObject.tag)
        {
            case "Collectable":
                // Picking up a Coin
                PickingUpCoin(other.gameObject);
                break;

            case "Big_Collectable":
                // Picking up a Big Star
                PickingUpStar(other.gameObject);
                break;

            default:
                Debug.LogWarning("Entering in contact with an unknown Trigger: The object has undefined tag " + other.gameObject.tag);
                break;
        }
    }

    void PickingUpCoin(GameObject _coinGO)
    {
        coinsChallenge.CoinCollected();

        Instantiate(CoinPickupParticles, _coinGO.transform.position, Quaternion.identity);

        Destroy(_coinGO);
    }

    void PickingUpStar(GameObject _starGO)
    {
        Destroy(_starGO);
    }



    void FreezeInPlace()
    {
        isFrozen = true;
        rb.useGravity = false;

        savedVelocity = rb.velocity;
        savedAngularVelocity = rb.angularVelocity;
    }

    void Unfreeze()
    {
        isFrozen = false;
        rb.useGravity = true;

        rb.velocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;
    }



    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + groundDetectionStartingPoint, groundDetectionSize);
    }

}
