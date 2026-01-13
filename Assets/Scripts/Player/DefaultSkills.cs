using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultSkills : MonoBehaviour
{   
    private PlayerInput fComp_playerInput;
    private Rigidbody fComp_rigidbody;

    void Awake()
    {
        fComp_playerInput = GetComponent<PlayerInput>();
        fComp_rigidbody = GetComponent<Rigidbody>();
    }

    public void Propel(InputAction.CallbackContext context)
    {   
        fComp_rigidbody.useGravity = false;
        fComp_rigidbody.AddForce(UnityEngine.Vector3.up * 3f, ForceMode.VelocityChange);
        if(context.canceled)
        {
            fComp_rigidbody.useGravity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}               
