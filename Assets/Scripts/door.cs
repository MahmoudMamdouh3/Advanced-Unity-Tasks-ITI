using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class door : MonoBehaviour
{
    public TwoBoneIKConstraint handIK; 
    public Transform player;          
    public float interactDistance = 2.5f;
    
    [Header("Speed Settings")]
    public float handReachSpeed = 0.8f; // Slow, natural reach
    public float doorOpenSpeed = 0.4f;  // Smooth swing
    
    private bool isOpening = false;
    private bool isFinished = false;
    private float currentRotation = 0f;
    private const float maxRotation = 90f; 

    void Update()
    {
        if (player == null || handIK == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // 1. Trigger the action with E
        if (!isOpening && distance < interactDistance && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOpening = true;
        }

        if (isOpening)
        {
            if (!isFinished)
            {
                // 2. Reach for the door FIRST
                handIK.weight = Mathf.MoveTowards(handIK.weight, 1f, Time.deltaTime * handReachSpeed);

                // 3. WAIT: Only start opening once the hand is 95% of the way there
                if (handIK.weight > 0.95f)
                {
                    float rotationStep = doorOpenSpeed * Time.deltaTime * 50f;
                    currentRotation += rotationStep;
                    
                    if (currentRotation <= maxRotation)
                    {
                        transform.Rotate(Vector3.up, rotationStep);
                    }
                    else
                    {
                        isFinished = true; // Stop when open
                    }
                }
            }
            else
            {
                // 4. Lower hand after the door is fully open
                handIK.weight = Mathf.MoveTowards(handIK.weight, 0f, Time.deltaTime * handReachSpeed);
                if (handIK.weight <= 0) isOpening = false; 
            }
        }
    }
}