using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{   
    private PlayerInput iComp_playerInput;
    private bool isOpen = false;
    private GameObject inventoryChild;
     
    private void Awake()
    {
        iComp_playerInput = GetComponent<PlayerInput>();
        inventoryChild = transform.GetChild(0).gameObject;
    }
    
    public void OpenInventory(InputAction.CallbackContext context)
    {   
        if (!context.performed)
        return;

        isOpen = !isOpen;
        Debug.Log(isOpen);
        inventoryChild.SetActive(isOpen);
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
