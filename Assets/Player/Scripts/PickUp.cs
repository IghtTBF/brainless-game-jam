using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction interact;

    private bool isEquipped;
    private bool isEquippable;
    
    public RaycastHit hit;
    private LayerMask layerMask;
    private GameObject lastHitObject;
    public float maxDist;
    public float fireflies;

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

    void Start()
    {
        fireflies = 0;
    }

    void Update()
    { 
        layerMask = LayerMask.GetMask("PickUps");

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist, layerMask))
        {
            isEquippable = true;
        }
        else{isEquippable = false;}

        Debug.DrawRay(transform.position, transform.forward * maxDist, Color.red);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (isEquippable)
        {
            StartCoroutine(FadeOutFirefly(hit.collider.gameObject));
            fireflies += 1;
        }
    }

    IEnumerator FadeOutFirefly(GameObject firefly)
    {
        ParticleSystem ps = firefly.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            var emission = ps.emission;
            emission.enabled = false;

            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
            Material mat = renderer.material;

            float fadeDuration = 0.5f;
            float elapsedTime = 0f;

            Color startColor = mat.color;
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fadeDuration;
                mat.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }

            mat.color = targetColor;
        }

        Destroy(firefly);
    }
}
