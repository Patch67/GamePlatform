using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject sheepPrefab;
    public GameObject[] flock;
    public int noOfSheep;
    public Dog dogPrefab, dog;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        //noOfSheep = 100;
        flock = new GameObject[noOfSheep];
        for (int i = 0; i < noOfSheep; i++)
        {
            flock[i] = Instantiate(sheepPrefab);  // Problem here: Instantiate only makes GameObjects but I want a SheepController
            flock[i].transform.position = new Vector2(UnityEngine.Random.value * 9 - 4.5f, UnityEngine.Random.value * 7 - 3.5f);
        }

        dog = Instantiate(dogPrefab);
        dog.transform.position = new Vector2(0, 0);
        dog.state = Dog.State.LeftFlank;

        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //End Game
                Application.Quit();
            }
            else if (Input.anyKeyDown)
            {
                // Up pause game
                paused = false;
                Time.timeScale = 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause
            paused = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dog.state = Dog.State.LeftFlank;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            dog.state = Dog.State.RightFlank;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            dog.state = Dog.State.LieDown;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            dog.state = Dog.State.WalkOn;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            dog.state = Dog.State.Steady;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            dog.state = Dog.State.ThatllDo;
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            dog.state = Dog.State.LookBack;
        }
    }
}
