using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    // Variables
    [SerializeField] private bool showController = false;
    [SerializeField] private InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField] private List<GameObject> controllerPrefabs;
    [SerializeField] private GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // Call method
        TryInitialize();
    }

    void TryInitialize()
    {
        // Get all the devices
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        // Check if there is a device
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            // Find the exact controller depending on what VR headset you are using
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            // Check if it finds the prefab from the prefabs in the game
            if (prefab)
            {
                // Spawn the proper controller
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                // Give a warning
                Debug.LogWarning("Did not find corresponding controller model");
            }

            // Spawn a hand model
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            // Animate the hand
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        // Check if the user used the trigger button
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            // Use the animation for trigger
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            // Put it back to normal state if not using the trigger anymore
            handAnimator.SetFloat("Trigger", 0);
        }

        // Check if the user used the grip button
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            // Use the animation for grip
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            // Put it back to normal state if not using the grip anymore
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there is a device
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            // Check if the contoller is showing
            if (showController)
            {
                // Turn off the hand and set the controller to be seen
                spawnedHandModel.SetActive(false);
                if (spawnedController)
                    spawnedController.SetActive(true);
            }
            else
            {
                // Turn on the hand and set the controller to not be seen
                spawnedHandModel.SetActive(true);
                if (spawnedController)
                    spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
    }
}
