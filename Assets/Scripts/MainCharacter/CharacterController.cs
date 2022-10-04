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
            MovementValue.x = MainController.Gamepad.Movement.ReadValue<Vector2>().x;
            MovementValue.y = MainController.Gamepad.Movement.ReadValue<Vector2>().y;
            Vector3 _Velocity = rb.velocity;


            _Velocity.z = Speed * MovementValue.y;
            _Velocity.x = Speed * MovementValue.x;

            rb.velocity = _Velocity;
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
            

        }
        else
        {
            _Mult =0;
        }
    }
}
