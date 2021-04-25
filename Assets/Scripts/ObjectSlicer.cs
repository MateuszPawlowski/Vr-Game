using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class ObjectSlicer : MonoBehaviour
{
    // Variables
    [SerializeField] private float initialSlicedFruitVelocity = 100;
    [SerializeField] private Material slicedMaterial;
    [SerializeField] private Transform startOfBlade;
    [SerializeField] private Transform endOfBlade;
    [SerializeField] private LayerMask sliceLayer;
    [SerializeField] private VelocityEstimator velocityEstimator;

    private int delay = 2;

    [SerializeField] AudioClip sliceSound;
    [SerializeField] [Range(0, 1)] float sliceVolume = 0.25f;

    [SerializeField] AudioClip bombSound;
    [SerializeField] [Range(0, 1)] float bombVolume = 0.25f;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Blade full position
        Vector3 bladePosition = endOfBlade.position - startOfBlade.position;
        // Check if it has touched the blade
        bool hasHit = Physics.Raycast(startOfBlade.position, bladePosition, out hit, bladePosition.magnitude, sliceLayer);
        if(hasHit)
        {
            // Check if the layer is 9 i.e bomb
            if (hit.transform.gameObject.layer == 9)
            {
                // Start a Coroutine
                StartCoroutine(WaitAndLoad());
                // Call Slice method
                Slice(hit.transform.gameObject, hit.point, velocityEstimator.GetVelocityEstimate());
                // Play a sound
                AudioSource.PlayClipAtPoint(bombSound, Camera.main.transform.position, bombVolume);
            }
            else
                // Update the score
                DisplayScore.score++;
                // Call Slice method
                Slice(hit.transform.gameObject, hit.point, velocityEstimator.GetVelocityEstimate());
        }
    }

    IEnumerator WaitAndLoad()
    {
        // Wait x amount seconds and run the game over scene
        yield return new WaitForSeconds(delay);
        // Get the LevelLoader method MainMenu
        FindObjectOfType<LevelLoader>().MainMenu();
    }

    void Slice(GameObject target, Vector3 planePosition, Vector3 slicerVelocity)
    {
        // Blade full position
        Vector3 bladePosition = endOfBlade.position - startOfBlade.position;
        // Get the plane for the blades
        Vector3 planeNormal = Vector3.Cross(slicerVelocity, bladePosition);

        // Using the imported EZslice classes
        SlicedHull hull = target.Slice(planePosition, planeNormal, slicedMaterial);

        if(hull != null)
        {
            // Check if the score if greater or equal to 10
            if(DisplayScore.score >= 10)
            {
                // Start a Coroutine
                StartCoroutine(WaitForNextLevel());
            }
            // Play a sound
            AudioSource.PlayClipAtPoint(sliceSound, Camera.main.transform.position, sliceVolume);
            // Create 2 game objects of the fruit sliced
            GameObject upperHull = hull.CreateUpperHull(target, slicedMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, slicedMaterial);

            // Use the method to create an fruit seem that it was sliced in half
            CreateSlicedComponent(upperHull);
            CreateSlicedComponent(lowerHull);

            // Destroy game object
            Destroy(target);
        }
    }

    IEnumerator WaitForNextLevel()
    {
        // Wait x amount seconds and run the game over scene
        yield return new WaitForSeconds(1);
        // Get the LevelLoader method LoadNextLevel
        FindObjectOfType<LevelLoader>().LoadNextLevel();
    }

    void CreateSlicedComponent(GameObject slicedHull)
    {
        // Add rigidbody to the sliced fruit
        Rigidbody rb = slicedHull.AddComponent<Rigidbody>();
        // Add a mess collider
        MeshCollider collider = slicedHull.AddComponent<MeshCollider>();
        collider.convex = true;

        // Make the sliced fruit explode to two different ways
        rb.AddExplosionForce(initialSlicedFruitVelocity, slicedHull.transform.position, 1);

        // Destroy the fruit after 4 seconds
        Destroy(slicedHull, 4);
    }
}
