using UnityEngine;

/// <summary>
/// Manages a Dog
/// </summary>
public class Dog : MonoBehaviour
{
    private GameController gameController;
    //private FlockController flockController;
    public enum State {LieDown, WalkOn, LeftFlank, RightFlank, Steady, LookBack, ThatllDo  };
    public State state;

    public Vector2 myVector;
    private Vector2 myScanAngle;

    private Rigidbody2D m_rigidbody2D;

    private bool m_started;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        //flockController = FindObjectOfType<FlockController>();
        myVector = new Vector2(0, 0.5f); //Set initial vector north, half speed
        myScanAngle = new Vector2(0, 1);  //Start scanning north
        m_started = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case State.LieDown:
                myVector = Vector2.zero;
                break;
            case State.WalkOn:
                if(m_rigidbody2D.velocity.sqrMagnitude<1)
                {
                    myVector = new Vector2(0, 2);
                }
                else
                { 
                    myVector = new Vector2(m_rigidbody2D.velocity.x, m_rigidbody2D.velocity.y);
                    myVector /= 2;
                }
                break;
            case State.LeftFlank:
                //myVector =new Vector2(m_rigidbody2D.velocity.y, m_rigidbody2D.velocity.x); // Quick trick to rotate by 270 degrees
                myVector = new Vector2(-2, 0);
                break;
            case State.RightFlank:
                //myVector = new Vector2(-m_rigidbody2D.velocity.y, m_rigidbody2D.velocity.x); // Quick trick to rotate by 270 degrees                
                myVector = new Vector2(2, 0);
                break;
            case State.Steady:
                myVector = new Vector2(0, -2);
                break;
            case State.LookBack:
                break;
            case State.ThatllDo:
                myVector = Vector2.zero;
                break;
        }
        m_rigidbody2D.AddForce(myVector * Time.deltaTime, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_started)
        {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            //Gizmos.DrawWireCube(transform.position, transform.localScale*10);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, myScanAngle*5);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, myVector);
        }
    }
}
