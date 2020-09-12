using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public GameObject sheepPrefab;
    public GameObject[] flock;
    public int noOfSheep;


    // Start is called before the first frame update
    void Start()
    {
        noOfSheep = 100;
        flock = new GameObject[noOfSheep];
        for(int i=0;i<noOfSheep;i++)
        {
            flock[i] = Instantiate(sheepPrefab);
            flock[i].transform.position = new Vector2(Random.value * 9 - 4.5f, Random.value * 7 - 3.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject[] GetNeighbours(int number,Vector2 position)
    {
        Responce r0 = Find(position, 0);
        Responce r1 = Find(position, r0.distance);


    }

    /// <summary>
    /// Find one element that is the closest thing above the given distance
    /// </summary>

    public struct Responce
    {
        public bool found;
        public GameObject go;
        public float distance;

        public Responce(bool iFound, GameObject iGo, float iDistance)
        {
            found = iFound;
            go = iGo;
            distance = iDistance;
        }
    }
    public Responce Find(Vector2 position, float minDistance)
    {
        GameObject go=flock[0];
        bool found = false;
        float distance = float.MaxValue;
        float tempDist;
        int c = 0;
        while(c<flock.Length)
        {
            tempDist = Vector2.Distance(position, flock[c].transform.position);
            if (tempDist>minDistance && tempDist<distance)
            {
                go = flock[c];
                found = true;
                distance = tempDist;
            }
            c++;
        }
        return new Responce(found, go, distance);
    }
}
