using System;
using System.Numerics;
using JetBrains.Annotations;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{   
    [Header("Movement Components")]
    private PlayerInput mComp_playerInput;
    private InputAction mComp_jumpAction;
    private Rigidbody mComp_rigidbody;
    [SerializeField] private LayerMask mComp_interactableObjectLayer;
    [SerializeField] private LayerMask mComp_groundLayer;
    [SerializeField] private CinemachineCamera mComp_cinemachineCamera;
    [SerializeField] private GameObject mComp_head;
     
    [Header("Movement Variables")]
    [SerializeField] private float mov_moveSpeed = 2f;   
    [SerializeField] private float mov_jumpForce = 2f;
    private UnityEngine.Vector2 mov_movementInput;   

    [Header("Player HP")]
    private float maxHP = 100;
    private float minHP = 0;
    private float currentHP;
    public float CurrentHP { get {return currentHP;} }
    [SerializeField] private Slider slider;

    private void Awake()
    {   
        mComp_playerInput = GetComponent<PlayerInput>();
        mComp_jumpAction = mComp_playerInput.actions.FindAction("Jump");
        mComp_rigidbody = GetComponent<Rigidbody>();


        slider.maxValue = maxHP;
        slider.minValue = minHP;

        currentHP = maxHP;

        mComp_rigidbody.maxLinearVelocity = 5f;
    
    } 

    public void ApplyDamage(float dmgTaken)
    {
        currentHP = currentHP - dmgTaken;
    }

    private bool isGrounded()
    {   
        return Physics.SphereCast(mComp_rigidbody.position, 0.2f, new UnityEngine.Vector3(0f, -1f, 0f),  out RaycastHit hitInfo, 0.8f, mComp_groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        mov_movementInput = context.ReadValue<UnityEngine.Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {   
        if(Physics.Raycast(mComp_cinemachineCamera.transform.position, mComp_cinemachineCamera.transform.forward, out RaycastHit hitInfo, 3f, mComp_interactableObjectLayer))
        {   
            if(hitInfo.collider.GetComponent<IInteractable>() != null)
            {   
                hitInfo.collider.GetComponent<IInteractable>().Interaction();
                Debug.Log("i am touching the " + hitInfo.collider.name);
            }

            if(hitInfo.collider.GetComponent<IInteractable>() == null)
            {
                Debug.Log(hitInfo.collider.name + " is not interactable");
            }

        }
        Debug.DrawRay(mComp_cinemachineCamera.transform.position, mComp_cinemachineCamera.transform.forward * 10f, UnityEngine.Color.red, 0.1f);
    }

    private void Walk(float speed)
    {   
        UnityEngine.Vector3 camForward = mComp_cinemachineCamera.transform.forward;
        UnityEngine.Vector3 camRight = mComp_cinemachineCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        UnityEngine.Vector3 move = camForward * mov_movementInput.y + camRight * mov_movementInput.x;
        mComp_rigidbody.AddForce(move.normalized * speed, ForceMode.Impulse);

    }

    private void Jump(float jumpForce)
    {
        mComp_rigidbody.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    private void Look()
    {   
        UnityEngine.Vector3 cameraEuler = mComp_cinemachineCamera.transform.rotation.eulerAngles;
        float eulerAngle = cameraEuler.x > 180 ? cameraEuler.x - 360 : cameraEuler.x;

        transform.rotation = UnityEngine.Quaternion.Euler(eulerAngle * 0.3f, cameraEuler.y , 0f);   

    } 

    private void Update()
    {   
        bool grounded = isGrounded();

        if (grounded && mComp_jumpAction.triggered)
        {
            Jump(mov_jumpForce);
        }

        slider.value = currentHP;
        Look();
        Debug.Log(mComp_rigidbody.linearVelocity.x + " x");
        Debug.Log(mComp_rigidbody.linearVelocity.y + " y");
        Debug.Log(mComp_rigidbody.linearVelocity.z + " z");


    }

    private void FixedUpdate()
    {
        Walk(mov_moveSpeed);
    }

}
