using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class SheepController : MonoBehaviour
{
    public GameObject sheep;

    private GameObject[] neighbours; // Array to hold the sheep's neighbours

    private Vector2 flockVector;  // Average Direction of the neighbours
    private Vector2 optimalVector; // Direction to move to the centre of the flock
    private Vector2 populationVector; // Direction that the rest of the herd is moving (Lower weight, only check the nearest coule of flocks)

    private float flockWeight; // How much weight should I give to the flock's direction
    private float optimalWeight; // How much weight should I give to the optimal's direction
    private float populationWeight; // How much weight should I give to the population's direction

    private float fear; // How scared am i?

    private Vector2 myVector; // The direction that I will move in
    private int noOfSheep;   // Number of sheep in local flock


    // Start is called before the first frame update
    void Start()
    {
        noOfSheep = 7;
        neighbours = new GameObject[7]; // Set up a blank array for 7 sheep
        fear = 0; // No fear on start
    }

    // Update is called once per frame
    void Update()
    {
        // Who are my neighbours?
        GetNeighbours();  // Find my neighbours

        // What are my neighbours doing?
        flockVector = GetFlockDirection();  // What direction are they moving in
        optimalVector = GetOptimalVector(); // What direction to put me in the middle of the flock, don't want to be caught on the edge
        populationVector = GetPopulationVector();  // Which direction is the herd moving in general

        //How scared should I be?

        // What are my weights
        flockWeight = 1 * fear;
        optimalWeight = 2 * fear;
        populationWeight = 2 * fear;
        //TODO: Just be careful here, I don't want to myVector to become too big

        // Work out my best move
        myVector = flockVector * flockWeight + optimalVector * optimalWeight + populationVector * populationWeight;

        // Make my move to the new position
        Vector3 myVector3 = new Vector3(myVector.x, myVector.y, 0); // Required to stop error on next line
        transform.position += myVector3; // Add the new vector to the original vector
    }

    public void GetNeighbours()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Sheep"));
        int noOfColliders = hitColliders.Length;
        int maxCount = Math.Min(noOfSheep, noOfColliders);
        for (int i = 0; i < maxCount; i++)
        {
            neighbours[i] = hitColliders[i].gameObject;
        }
    }
    /// <summary>
    /// Get the average direction of my flock
    /// </summary>
    /// <returns>Vector2 direction of flock</returns>
    public Vector2 GetFlockDirection()
    {
        return Vector2.zero;
    }
    /// <summary>
    /// Get the direction to move me in to the centre of the flock
    /// </summary>
    /// <returns>The vector pointing me to the centre of the flock</returns>
    public Vector2 GetOptimalVector()
    {
        return Vector2.zero;
    }
    /// <summary>
    /// Gets the direction to move in to move me towards neighbouing flock
    /// </summary>
    /// <returns>The direction to the neighbouring flock</returns>
    public Vector2 GetPopulationVector()
    {
        return Vector2.zero;
    }
}
