using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject sheepPrefab;
    public GameObject[] flock;
    public int noOfSheep;


    // Start is called before the first frame update
    void Start()
    {
        noOfSheep = 100;
        flock = new GameObject[noOfSheep];
        for (int i = 0; i < noOfSheep; i++)
        {
            flock[i] = Instantiate(sheepPrefab);
            flock[i].transform.position = new Vector2(UnityEngine.Random.value * 9 - 4.5f, UnityEngine.Random.value * 7 - 3.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
