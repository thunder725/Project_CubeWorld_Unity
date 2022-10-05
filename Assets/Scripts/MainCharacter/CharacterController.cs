using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    Rigidbody rb;
    InputController MainController;
    Vector2 MovementValue;
    public float Speed, _Mult, _DragMult;
    private float InitialSpeed;
    public Vector3 ForwardVelocity;
    public GameObject SupportCamera;

    //start =====================================
    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
        MainController = new InputController();


    }

    private void Start()
    {
        InitialSpeed = Speed;
    }

    private void OnEnable()
    {
        MainController.Enable();
    }

    private void OnDisable()
    {
        MainController.Disable();
    }

    // Per tick =============================================

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(MainController.Gamepad.Movement.ReadValue<Vector2>() != new Vector2(0,0))
        {
            MovementValue.x = MainController.Gamepad.Movement.ReadValue<Vector2>().x ;
            MovementValue.y = MainController.Gamepad.Movement.ReadValue<Vector2>().y ;
            Vector3 _Velocity = rb.velocity;


            _Velocity.z = Speed * MovementValue.y;
            _Velocity.x = Speed * MovementValue.x;

            //rb.velocity = _Velocity;
            Debug.Log(SupportCamera.transform.forward);
            //rb.velocity = new Vector3(MovementValue.x * SupportCamera.transform.right.x,
            //    0,
            //    _Velocity.z * SupportCamera.transform.forward.z);
            
            rb.velocity = _Velocity.x * SupportCamera.transform.right + _Velocity.z * SupportCamera.transform.forward;
            //rb.AddForce(SupportCamera.transform.right.z*MovementValue.x, 0, SupportCamera.transform.forward.z*MovementValue.y, ForceMode.Impulse);

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
    }
}
