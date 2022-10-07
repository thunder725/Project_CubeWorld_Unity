using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SC_ControllerMainCharacter : MonoBehaviour
{

    InputController _Controller;
    Rigidbody rb;
    float _SpeedMovement;
    public float _RotationLag;
    CharacterAbilities SCabilities;

    //===========================================================

    private void Awake()
    {
        
        _Controller = new InputController();
        rb = GetComponent<Rigidbody>();

        /*
                 overlapResults = new Collider[32];
        */

    }

    private void OnEnable()
    {
        _Controller.Enable();

        /*
        GeneralEventManager.PauseGameplay += FreezeInPlace;
        GeneralEventManager.ResumeGameplay += Unfreeze;
        _Controller.Gamepad.Abilities.performed += EventAbilities;
        */
    }

    private void OnDisable()
    {
        _Controller.Disable();

        /*
        GeneralEventManager.PauseGameplay -= FreezeInPlace;
        GeneralEventManager.ResumeGameplay -= Unfreeze;

        MainController.Gamepad.Abilities.performed -= EventAbilities;
        */
    }


    private void FixedUpdate()
    {

        Vector2 MovementValueStick = _Controller.Gamepad.Movement.ReadValue<Vector2>();

        if (MovementValueStick != Vector2.zero)
        {

            Quaternion StickDirection = Quaternion.LookRotation(new Vector3(MovementValueStick.x, 0, MovementValueStick.y));  // * transform.rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, StickDirection, Time.deltaTime * _RotationLag);

            //rb.velocity = _Velocity.x * SocleCharacterController.transform.right + _Velocity.z * SocleCharacterController.transform.forward + _Velocity.y * Vector3.up;

            /*

            if (_Mult <= 3)
            {
                _Mult += Time.deltaTime * _DragMult;
            }

            if (InitialSpeed * 3 <= Speed)
            {
                Speed = InitialSpeed * 2;
            }
            else
            {
                Speed = InitialSpeed * _Mult;
            }

            ForwardVelocity = rb.velocity;
            */
        }
        else
        {
           // _Mult = 0;
        }
            
    }


    #region LeonardCode

    /*

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



    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
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



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + groundDetectionStartingPoint, groundDetectionSize);
    }

    */

    #endregion

}
