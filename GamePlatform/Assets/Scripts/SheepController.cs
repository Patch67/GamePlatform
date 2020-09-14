using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class SheepController : MonoBehaviour
{
    public float testRadius;
    //public GameObject sheep;  // Why?
    private int mask;
    //public GameObject[] neighbours; // Array to hold the sheep's neighbours
    public List<GameObject> neighbours = new List<GameObject>();

    private Vector2 flockVector;  // Average Direction of the neighbours
    private Vector2 optimalVector; // Direction to move to the centre of the flock
    private Vector2 populationVector; // Direction that the rest of the herd is moving (Lower weight, only check the nearest coule of flocks)

    private float flockWeight; // How much weight should I give to the flock's direction
    private float optimalWeight; // How much weight should I give to the optimal's direction
    private float populationWeight; // How much weight should I give to the population's direction

    public float fear; // How scared am i?

    private  Vector2 myVector; // The direction that I will move in
    public int noOfSheep;   // Number of sheep in local flock

    bool m_Started; // CHECK FOR GIZMOS, ONLY IN PLAY MODE
    private Rigidbody2D m_Rigidbody2D;
    private CircleCollider2D circleCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        m_Started = true;// We are in play mode, used for gizmos
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();

        mask = LayerMask.GetMask("Sheep"); // The layer used for all sheep

        //noOfSheep = 7;
        //fear = 0; // No fear on start

        //Give the sheep an initial random direction to move in
        myVector = Vector2.one;
    }

    void Update()
    {
        //TODO: How big should my personal space be?
        circleCollider2D.radius = (1 - fear) ; // The higher the feat the closer they will want to be
    }


    // FixedUpdate is called once per frame it is linked to physics engine activity
    void FixedUpdate()
    {
        // Who are my neighbours?
        GetNeighbours();  // Find my neighbours

        // What are my neighbours doing?
        flockVector = GetFlockDirection();  // What direction are they moving in
        optimalVector = GetOptimalVector(); // What direction to put me in the middle of the flock, don't want to be caught on the edge
        populationVector = GetPopulationVector();  // Which direction is the herd moving in general

        //TODO: How scared should I be?

        // What are my weights
        flockWeight = 1 * fear;
        optimalWeight = 1 * fear;
        populationWeight = 1 * fear;

        // Work out my best move
        myVector = flockVector * flockWeight + optimalVector * optimalWeight + populationVector * populationWeight;


        //TODO: Need to speed limit the myVector to a maximum magnitude


        // Make my move to the new position
        m_Rigidbody2D.AddForce(myVector*Time.deltaTime, ForceMode2D.Impulse);
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
        neighbours.Clear(); //Get rid of the old list
        //Make a new list
        foreach(Collider2D collider in hitColliders)
        {
            neighbours.Add(collider.gameObject);//Add each to the list
        }
        //If we have too many neighbours make the test circle 10% smaller
        if (hitColliders.Length > noOfSheep)
        {
            testRadius *= 0.9f;
        }
        // If we have too many neighbours make the test circle 10% larger
        else if(hitColliders.Length<noOfSheep)
        {
            testRadius *= 1.1f;
        }
    }

    /// <summary>
    /// Get the average direction of my flock
    /// </summary>
    /// <returns>Vector2 direction of flock</returns>
    public Vector2 GetFlockDirection()
    {
        Vector2 dir = Vector2.zero;
        if (neighbours.Count > 0)
        {
            foreach (GameObject go in neighbours)
            {
                //Important: This is how to get access to a subclass proerties
                SheepController sheep = go.GetComponent<SheepController>();
                //UnityEngine.Debug.Log("Sheep is " + sheep);
                //if (sheep) UnityEngine.Debug.Log("Sheep " + sheep.myVector);
                //else UnityEngine.Debug.Log("Not a sheep");
                dir += sheep.myVector;
            }
            //UnityEngine.Debug.Log("Direction = " + dir + " neighbours is " + neighbours.Count);
            dir /= neighbours.Count;
        }
        return dir;
    }

    /// <summary>
    /// Get the direction to move me in to the centre of the flock
    /// </summary>
    /// <returns>The vector pointing me to the centre of the flock</returns>
    public Vector2 GetOptimalVector()
    {
        //Find min and max coordinates
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = -float.MaxValue;
        float maxY = -float.MaxValue;
        foreach(GameObject go in neighbours)
        {
            minX = Math.Min(minX, go.transform.position.x);
            minY = Math.Min(minY, go.transform.position.y);
            maxX = Math.Max(maxX, go.transform.position.x);
            maxY = Math.Max(maxY, go.transform.position.y);
        }

        //Find centre position
        Vector2 centre = new Vector2(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);

        //Find vector to centre position

        return centre-(Vector2)transform.position;
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
    
}
