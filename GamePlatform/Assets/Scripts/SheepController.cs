using UnityEngine;
using System;
using System.Diagnostics;


public class SheepController : MonoBehaviour
{
    public int testRadius;
    //public GameObject sheep;  // Why?
    private int mask;
    public GameObject[] neighbours; // Array to hold the sheep's neighbours

    private Vector2 flockVector;  // Average Direction of the neighbours
    private Vector2 optimalVector; // Direction to move to the centre of the flock
    private Vector2 populationVector; // Direction that the rest of the herd is moving (Lower weight, only check the nearest coule of flocks)

    private float flockWeight; // How much weight should I give to the flock's direction
    private float optimalWeight; // How much weight should I give to the optimal's direction
    private float populationWeight; // How much weight should I give to the population's direction

    public float fear; // How scared am i?

    private Vector2 myVector; // The direction that I will move in
    private int noOfSheep;   // Number of sheep in local flock

    bool m_Started; // CHECK FOR GIZMOS, ONLY IN PLAY MODE
    // Start is called before the first frame update
    void Start()
    {
        m_Started = true;// We are in play mode, used for gizmos
        mask = LayerMask.GetMask("Sheep"); // The layer used for all sheep
       
        noOfSheep = 7;
        neighbours = new GameObject[7]; // Set up a blank array for 7 sheep
        fear = 0; // No fear on start
        //Give the sheep an initial random direction to move in

    }

    // Update is called once per frame
    void FixedUpdate()
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
        transform.position += myVector3 * Time.deltaTime; // Add the new vector to the original vector
    }
    /// <summary>
    /// Uses a bounding box to get all of the neighbours.
    /// Note there may not be 7 neighbours, there may be more or less
    /// </summary>
    public void GetNeighbours()
    {
        //UnityEngine.Debug.Log("Looking for neighbours");
        //UnityEngine.Debug.Log("Local Scale is " + transform.localScale);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, testRadius,mask);
        int noOfColliders = hitColliders.Length;
        if(noOfColliders>0)
        { 
            UnityEngine.Debug.Log("No of colliders is " + noOfColliders);
            int maxCount = Math.Min(noOfSheep, noOfColliders);
            for (int i = 0; i < maxCount; i++)
            {
                neighbours[i] = hitColliders[i].gameObject;
            }
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
        Vector2 average;
        average = Vector2.one;

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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        { 
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            //Gizmos.DrawWireCube(transform.position, transform.localScale*10);
            Gizmos.DrawWireSphere(transform.position, testRadius);
        }
    }
    public static void Show(SheepController sheep)
    {
        UnityEngine.Debug.Log(sheep);
    }
}
