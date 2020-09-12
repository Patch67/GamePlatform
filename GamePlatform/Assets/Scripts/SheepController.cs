using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public GameObject sheep;

    private Sheep[] neighbours; // Array to hold the sheep's neighbours

    private Vector2 flockVector;  // Average Direction of the neighbours
    private Vector2 optimalVector; // Direction to move to the centre of the flock
    private Vector2 populationVector; // Direction that the rest of the herd is moving (Lower weight, only check the nearest coule of flocks)

    private float flockWeight; // How much weight should I give to the flock's direction
    private float optimalWeight; // How much weight should I give to the optimal's direction
    private float populationWeight; // How much weight should I give to the population's direction

    private float fear; // How scared am i?

    private Vector2 myVector; // The direction that I will move in


    // Start is called before the first frame update
    void Start()
    {
        neighbours = new Sheep[7]; // Set up a blank array for 7 sheep
        fear = 0; // No fear on start
    }

    // Update is called once per frame
    void Update()
    {
        // Who are my neighbours?
        neighbours = GetNeighbours();  // Find my neighbours

        // What are my neighbours doing?
        flockVector = GetFlockDirection();  // What direction are they moving in
        optimalVector = GetOptimalvector(); // What direction to put me in the middle of the flock, don't want to be caught on the edge
        populationVector = getPopulationVector();  // Which direction is the herd moving in general

        //How scared should I be?

        // What are my weights
        flockWeight = 1 * fear;
        optimalWeight = 2 * fear;
        populationWeight = 2 * fear;
        //TODO: Just be careful here, I don't want to myVector to become too big

        // Work out my best move
        myVector = transform.position + flockVector * flockWeight + optimalVector * optimalWeight + populationVector * populationWeight;

        //Make my move

    }


}
