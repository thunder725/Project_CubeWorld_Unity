using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SC_ControllerMainCharacter : MonoBehaviour
{

    InputController _Controller;
    Rigidbody rb;
    
    public float _RotationLag, _CameraSpeedY, _CameraSpeedX, _SpeedMovement, _Mult, _DragMult, LagSpeedVelocity, _LagMultCalcule, _MultCamera, _DragMultCamera, _LagMovementCamera, LagRotationOutRange90D;
    float InitialSpeed;
    CharacterAbilities SCabilities;
    public GameObject SocleCameraRotationX, SocleCameraRotationY;
    Vector2 MovementValue;
    Quaternion _RotationCameraControllerResult, _RotationCameraControllerResultY;

    SoundManager soundManager;

    //===========================================================

    private void Awake()
    {
        SCabilities = GetComponent<CharacterAbilities>();
        _Controller = new InputController();
        rb = GetComponent<Rigidbody>();

        
        overlapResults = new Collider[32];
        soundManager = GetComponent<SoundManager>();   
        

    }

    private void Start()
    {
        InitialSpeed = _SpeedMovement;
    }

    private void OnEnable()
    {
        _Controller.Enable();

        
        GeneralEventManager.PauseGameplay += FreezeInPlace;
        GeneralEventManager.ResumeGameplay += Unfreeze;
        _Controller.Gamepad.Abilities.performed += EventAbilities;
        
    }

    private void OnDisable()
    {
        _Controller.Disable();

        
        GeneralEventManager.PauseGameplay -= FreezeInPlace;
        GeneralEventManager.ResumeGameplay -= Unfreeze;

        _Controller.Gamepad.Abilities.performed -= EventAbilities;
        
    }



    private void FixedUpdate()
    {

        Vector3 SocleRotation = new Vector3(SocleCameraRotationY.transform.rotation.eulerAngles.x, SocleCameraRotationX.transform.rotation.eulerAngles.y, SocleCameraRotationY.transform.rotation.eulerAngles.z) ;
        SocleCameraRotationY.transform.rotation = Quaternion.Euler(SocleRotation);

        print(isGrounded);

        if (_MultCamera <= 3)
        {
            _MultCamera += Time.deltaTime * _DragMultCamera;
        }

        Vector2 MovementValueStickCamera = _Controller.Gamepad.CameraDirection.ReadValue<Vector2>() * _MultCamera;

        if (MovementValueStickCamera != Vector2.zero)
        {
            if (_MultCamera <= 3)
            {
                _MultCamera += Time.deltaTime * _DragMult;
            }

            Vector3 RotationActualX = SocleCameraRotationX.transform.rotation.eulerAngles;
            _RotationCameraControllerResult = Quaternion.Euler(new Vector3(0, RotationActualX.y + MovementValueStickCamera.x * _CameraSpeedX, 0));


            Vector3 RotationActualY = SocleCameraRotationY.transform.rotation.eulerAngles;
            _RotationCameraControllerResultY = Quaternion.Euler(new Vector3(RotationActualY.x + -MovementValueStickCamera.y * _CameraSpeedY, RotationActualY.y, 0));


            
      
 
            if(SocleCameraRotationY.transform.rotation.eulerAngles.x >= 275f || SocleCameraRotationY.transform.rotation.eulerAngles.x <= 85)
            {
                SocleCameraRotationY.transform.rotation = Quaternion.Lerp(SocleCameraRotationY.transform.rotation, _RotationCameraControllerResultY, Time.deltaTime * _RotationLag);
            }
            else
            {
                if(SocleCameraRotationY.transform.rotation.eulerAngles.x <= 90)
                {
                    Vector3 CameraInf = new Vector3(80, SocleCameraRotationY.transform.rotation.eulerAngles.y, SocleCameraRotationY.transform.rotation.eulerAngles.z);
                    SocleCameraRotationY.transform.rotation = Quaternion.Lerp(SocleCameraRotationY.transform.rotation, Quaternion.Euler(CameraInf), Time.deltaTime * LagRotationOutRange90D);
                    print("oui");
                }
                else
                {
                    Vector3 CameraInf = new Vector3(276, SocleCameraRotationY.transform.rotation.eulerAngles.y, SocleCameraRotationY.transform.rotation.eulerAngles.z);
                    SocleCameraRotationY.transform.rotation = Quaternion.Lerp(SocleCameraRotationY.transform.rotation, Quaternion.Euler(CameraInf), Time.deltaTime * LagRotationOutRange90D);
                }
            }


            SocleCameraRotationX.transform.rotation = Quaternion.Lerp(SocleCameraRotationX.transform.rotation, _RotationCameraControllerResult, Time.deltaTime * _RotationLag);

        }
        else
        {
            _MultCamera = 0;
        }

        MovementValue = _Controller.Gamepad.Movement.ReadValue<Vector2>();

        SocleCameraRotationX.transform.position = Vector3.Lerp(SocleCameraRotationX.transform.position, transform.position, Time.deltaTime * _LagMovementCamera);
        if(isGrounded)
        {
            if (MovementValue != Vector2.zero)
            {
                Vector3 _Velocity = rb.velocity;


                _Velocity.z = _SpeedMovement * MovementValue.y;
                _Velocity.x = _SpeedMovement * MovementValue.x;
                _Velocity.y = rb.velocity.y;

                Vector3 _VelocityTarget = _Velocity.x * SocleCameraRotationX.transform.right + _Velocity.z * SocleCameraRotationX.transform.forward + _Velocity.y * Vector3.up;
                rb.velocity = Vector3.Lerp(rb.velocity, _VelocityTarget, Time.deltaTime * LagSpeedVelocity);
                // print(rb.velocity);


                if (_Mult <= 3)
                {
                    _Mult += Time.deltaTime * _DragMult;
                }

                if (InitialSpeed * 3 <= _SpeedMovement)
                {
                    _SpeedMovement = Mathf.Lerp(_SpeedMovement, InitialSpeed * 3, Time.deltaTime * _LagMultCalcule);
                }
                else
                {
                    _SpeedMovement = Mathf.Lerp(_SpeedMovement, InitialSpeed * _Mult, Time.deltaTime * _LagMultCalcule);
                }
            }
            else
            {
                _Mult = Mathf.Lerp(_Mult, 0, Time.deltaTime * _DragMult);
                _SpeedMovement = Mathf.Lerp(_SpeedMovement, 0, Time.deltaTime * 1);
            }
        }
        else
        {
            if (MovementValue != Vector2.zero)
            {
                Vector3 _Velocity = rb.velocity;


                _Velocity.z = _SpeedMovement  * MovementValue.y;
                _Velocity.x = _SpeedMovement  * MovementValue.x;
                _Velocity.y = rb.velocity.y;

                Vector3 _VelocityTarget = _Velocity.x * SocleCameraRotationX.transform.right + _Velocity.z * SocleCameraRotationX.transform.forward + _Velocity.y * Vector3.up;
                rb.velocity = Vector3.Lerp(rb.velocity, _VelocityTarget, Time.deltaTime * LagSpeedVelocity / 5);
                // print(rb.velocity);


                if (_Mult <= 3)
                {
                    _Mult += Time.deltaTime * _DragMult;
                }

                if (InitialSpeed * 3 <= _SpeedMovement)
                {
                    _SpeedMovement = Mathf.Lerp(_SpeedMovement, InitialSpeed * 3, Time.deltaTime * _LagMultCalcule / 5);
                }
                else
                {
                    _SpeedMovement = Mathf.Lerp(_SpeedMovement, InitialSpeed * _Mult, Time.deltaTime * _LagMultCalcule / 5);
                }
            }
            else
            {
                _Mult = Mathf.Lerp(_Mult, 0, Time.deltaTime * _DragMult);
                _SpeedMovement = Mathf.Lerp(_SpeedMovement, 0, Time.deltaTime * 1);
            }
        }
        

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


    #region LeonardCode



    bool isFrozen;
    Vector3 savedVelocity, savedAngularVelocity, _directionToDash;

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
        _directionToDash = SocleCameraRotationX.transform.forward;
        /*
        _directionToDash = MovementValue;
        _directionToDash[2] = _directionToDash[1];
        _directionToDash[1] = 0;

        _directionToDash.Normalize();
        
        if (_directionToDash == Vector3.zero)
        {
            _directionToDash = SocleCameraRotationX.transform.forward;
        }
        */
        rb.AddForce(_directionToDash * DashImpulsionStrength * 1000f);

        isDashing = true;
        currentDashDuration = DashDuration;

        soundManager.PlaySoundWithName(SoundManager.ValidSounds.Dash);

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
                soundManager.PlaySoundWithName(SoundManager.ValidSounds.CoinPickup);
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

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Tree":

                //Debug.Log(rb.velocity.sqrMagnitude);
                if (rb.velocity.sqrMagnitude > 4f)
                {
                        soundManager.PlaySoundWithName(SoundManager.ValidSounds.HitTree);
                }
                
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

    

    #endregion

}
