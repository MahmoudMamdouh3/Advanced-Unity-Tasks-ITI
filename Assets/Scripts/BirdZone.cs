using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BirdZone : MonoBehaviour
{
    public MultiAimConstraint headAim; // Drag HeadLook_Rig here
    private bool playerInside = false;

    void Update()
    {
        // Smoothly look at bird when inside, look forward when outside
        headAim.weight = Mathf.Lerp(headAim.weight, playerInside ? 1f : 0f, Time.deltaTime * 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }
}