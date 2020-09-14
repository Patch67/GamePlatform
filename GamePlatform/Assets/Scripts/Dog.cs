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

    private Rigidbody2D m_rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        //flockController = FindObjectOfType<FlockController>();
        myVector = new Vector2(0, 0.5f); //Set initial vector north, half speed
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.LieDown:
                myVector = Vector2.zero;
                break;
            case State.WalkOn:
                /*speed = 0.5f;
                //Move in direction of angle
                Quaternion rotation = Quaternion.Euler(0, 0, direction); // Rotate around Z; suitable for 2D X,Y coordinates
                Matrix4x4 m = Matrix4x4.identity;
                m.SetTRS(transform.position, rotation, new Vector3(1, 1, 0));// Set up matrix
                transform.position = transform.position + m.MultiplyVector(new Vector3(speed * Time.deltaTime, 0, 0));*/

                //TODO: Put a speed limit on myVector
                m_rigidbody2D.AddForce(myVector * Time.deltaTime, ForceMode2D.Impulse);
                break;
            case State.LeftFlank:
                /*speed = 0.1f;
                transform.position = Vector3.Slerp(transform.position, flockController.flock[0].transform.position, speed*Time.deltaTime);*/

                //Find the sheep on the left of my field of view

                break;
            case State.RightFlank:
                /*speed = 0.1f;
                transform.position = Vector3.Slerp(transform.position, flockController.flock[0].transform.position, speed * Time.deltaTime);*/
                break;
            case State.Steady:
                myVector /= 2;
                break;
            case State.LookBack:
                //TODO: 
                break;
            case State.ThatllDo:
                //speed = 0;
                myVector = Vector2.zero;
                break;
        }
    }
}
