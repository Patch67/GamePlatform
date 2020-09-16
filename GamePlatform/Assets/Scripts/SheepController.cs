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
    private Vector2 dogVector;  // Direction to te dog

    private float flockWeight; // How much weight should I give to the flock's direction
    private float optimalWeight; // How much weight should I give to the optimal's direction
    private float populationWeight; // How much weight should I give to the population's direction
    private float dogWeight; //How much should I be worried about the dog?
    public float fear; // How scared am i?

    private  Vector2 myVector; // The direction that I will move in
    public int noOfSheep;   // Number of sheep in local flock

    bool m_Started; // CHECK FOR GIZMOS, ONLY IN PLAY MODE
    private Rigidbody2D m_Rigidbody2D;
    private CircleCollider2D circleCollider2D;


    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        m_Started = true;// We are in play mode, used for gizmos
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        gameController = FindObjectOfType<GameController>();

        mask = LayerMask.GetMask("Sheep"); // The layer used for all sheep

        //noOfSheep = 7;
        //fear = 0; // No fear on start

        //Give the sheep an initial random direction to move in
        myVector = Vector2.one;
    }

    void Update()
    {
        //How big should my personal space be?
        circleCollider2D.radius = (1 - fear) ; // The higher the feat the closer they will want to be
    }


    // FixedUpdate is called once per frame it is linked to physics engine activity
    void FixedUpdate()
    {
        // Who are my neighbours?
        GetNeighbours();  // Find my neighbours
//        UnityEngine.Debug.Log("Differece " + myVector + "; " + m_Rigidbody2D.velocity);

        // What are my neighbours doing?
        flockVector = GetFlockDirection();  // What direction are they moving in
        optimalVector = GetOptimalVector(); // What direction to put me in the middle of the flock, don't want to be caught on the edge
        populationVector = GetPopulationVector();  // Which direction is the herd moving in general

        //TODO: How scared should I be?
        // Am I exposed to the dog?
        //Work out vector from me to the dog
        dogVector = GetDogVector();
        //UnityEngine.Debug.Log(dogVector);

        //Cast a ray from me to the dog
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position+(dogVector.normalized*testRadius), dogVector, 5);
        //See if it hit a sheep, meaning I am covered
        //UnityEngine.Debug.Log("Hit is " + hit.rigidbody.name);
        if (hit.collider != null)
        {
            //UnityEngine.Debug.Log("2nd hit is " + hit.collider.name + " distance is " + hit.distance);
            if (hit.collider.name == "dog(Clone)")
            {
                //UnityEngine.Debug.Log("We got the dog, at last.");
                fear *= 1.0001f; // Up the fear a little bit
                dogWeight = 1;
            }
            else
            {
                dogWeight = 0;
            }
        }
        // Can I get another sheep between me and the dog?

        // What are my weights
        flockWeight = 1 * fear;
        optimalWeight = 0 * fear;
        populationWeight = 1 * fear;
        //dogWeight = .2f * fear;

        // Work out my best move
        myVector = flockVector * flockWeight + optimalVector * optimalWeight + populationVector * populationWeight - dogVector*dogWeight;


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

    /// <summary>
    /// Gets a vector from sheep to dog.
    /// Tested 14/09/2020 PAB
    /// </summary>
    /// <returns>Vector2 pointing from the sheep to the dog.</returns>
    public Vector2 GetDogVector()
    {
        return gameController.dog.transform.position - transform.position;
    }
    void OnDrawGizmos()
    {/*
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        { 
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            //Gizmos.DrawWireCube(transform.position, transform.localScale*10);
            Gizmos.DrawWireSphere(transform.position, testRadius);
        }*/
    }
    
}
