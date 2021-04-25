using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverIfFruitTouchGround : MonoBehaviour
{
    // Variables
    [SerializeField] private int maxNumErrors = 3;
    private int numOfErrors = 0;

    // If something collides method
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the layer is 8 i.e the fruit
        if(collision.gameObject.layer == 8)
        {
            // Add to the variable
            numOfErrors++;
            // Check if the variables are equal
            if(numOfErrors == maxNumErrors)
            {
                // Get the LevelLoader class and go to main menu
                FindObjectOfType<LevelLoader>().MainMenu();
            }

            // Destroy the fruit
            Destroy(collision.gameObject);
        }
    }
}
