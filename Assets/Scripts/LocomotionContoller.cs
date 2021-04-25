using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionContoller : MonoBehaviour
{
    // Variables
    [SerializeField] private XRController leftTpRay;
    [SerializeField] private XRController rightTpRay;
    [SerializeField] private InputHelpers.Button activateButton;
    [SerializeField] private float activationThreshold = 0.1f;
    [SerializeField] private XRRayInteractor leftInteractor;
    [SerializeField] private XRRayInteractor rightInteractor;

    // Getters and setters
    public bool allowTpLeft { get; set; } = true;
    public bool allowTpRight { get; set; } = true;

    // Update is called once per frame
    void Update()
    {
        // Initialize variables
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;

        // Check if using right ray
        if (rightTpRay)
        {
            // Create a hover ray for the user to see where he teleports
            bool isrightInteractorHover = rightInteractor.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);

            // Allow the player to teleport
            rightTpRay.gameObject.SetActive(allowTpRight && CheckIfActivated(rightTpRay) && !isrightInteractorHover);
        }

        // Check if using left ray
        if (leftTpRay)
        {
            // Create a hover ray for the user to see where he teleports
            bool isLeftInteractorHover = leftInteractor.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            
            // Allow the player to teleport
            leftTpRay.gameObject.SetActive(allowTpLeft && CheckIfActivated(leftTpRay) && !isLeftInteractorHover);
        }
    }

    private bool CheckIfActivated(XRController controller)
    {
        // Check if the button is activated
        InputHelpers.IsPressed(controller.inputDevice, activateButton, out bool isActivated, activationThreshold);
        // Return if it is active
        return isActivated;
    }
}
