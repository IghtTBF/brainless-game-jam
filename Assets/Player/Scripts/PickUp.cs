using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction interact;

    private bool isEquipped;
    private bool isEquippable;
    
    public Outline outline;
    public RaycastHit hit;
    private LayerMask layerMask;
    private GameObject lastHitObject;
    public float maxDist;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        interact = inputActions.Player.Interact;
        interact.performed += OnInteract;
        interact.Enable();    
    }

    void OnDisable()
    {
        interact.performed -= OnInteract;
        interact.Disable();
    }

    void Update()
    { 
        layerMask = LayerMask.GetMask("PickUps");

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist, layerMask))
        {
            outline.enabled = true;
            isEquippable = true;
        }
        else{isEquippable = false; outline.enabled = false;}

        Debug.DrawRay(transform.position, transform.forward * maxDist, Color.red);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (isEquippable)
        {
            hit.collider.gameObject.SetActive(false);
        }

        Debug.Log("'E' Pressed");
    }
}
