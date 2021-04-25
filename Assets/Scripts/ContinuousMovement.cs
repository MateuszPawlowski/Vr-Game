using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private XRNode inputSource;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float gravity = -9.82f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float additionalHeight = 0.2f;

    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;
    private float fallingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Get components to access them
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input of the device
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        // Call the capsuleFollow method
        capsuleFollow();

        // Get the head movementof the vr headset
        Quaternion headMovement = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        // When we get the head movment we multiply it by the input of the user on the joystick
        Vector3 direction = headMovement * new Vector3(inputAxis.x, 0, inputAxis.y);

        // Move the character the proper direction and speed
        character.Move(direction * Time.deltaTime * speed);

        // Check for gravity
        bool grounded = ifGrounded();
        // If the person is touching the floor dont fall down
        // Else calculate the speed he is going to fall down
        if (grounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        // Make the player fall to the ground
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    private void capsuleFollow()
    {
        // Make the capsule around the player follow him
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        // Get the center of the capsule
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        // Get the center of capsule for headset
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    private bool ifGrounded()
    {
        // Checking if the person is grounded
        // Start a ray in the center
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLenght = character.center.y + 0.01f;
        // Calculate usings physics to see if the player touches the ground
        bool touchesGround = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLenght, groundLayer);
        // Returning if the player has hit the ground
        return touchesGround;
    }
}
