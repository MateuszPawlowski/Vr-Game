using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    // Variables
    [SerializeField] private TextMeshPro textMeshPro;
    public static int score;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize score to 0
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the score and change it to toString
        textMeshPro.text = score.ToString();
    }
}
