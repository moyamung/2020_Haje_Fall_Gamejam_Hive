using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honeycomb : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject honeycombCellPrefab;
    public GameObject honeycombAddPrefab;

    GameObject[,] honeycomb = new GameObject[7, 7];
    Vector3 xVector = new Vector3(1f, 0f, 0f);
    Vector3 yVector = new Vector3(-0.5f, Mathf.Sqrt(3) / 2, 0f);
    float cellSize = 1.1f;

    void Start()
    {
        AddCell(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    struct Coords
    {
        public int x { get; set; }
        public int y { get; set; }
        public Coords(int _x,int _y)
        {
            x = _x;
            y = _y;

        }
        public static Coords operator +(Coords a, Coords b)
            => new Coords(a.x + b.x, a.y + b.y);
    }

    public void AddCell(int x, int y)
    {
        if (honeycomb[x + 3, y + 3] != null)
        {
            Destroy(honeycomb[x + 3, y + 3]);
        }
        GameObject cell = Instantiate(honeycombCellPrefab, SetCellPosition(x, y), Quaternion.identity, this.transform);
        honeycomb[x + 3, y + 3] = cell;
        AddButtonMaking();
    }

    void AddButtonMaking()
    {
        List<Coords> list = Circumference();
        foreach (Coords coord in list)
        {
            if (honeycomb[coord.x + 3, coord.y + 3] == null)
            {
                GameObject AddButton = Instantiate(honeycombAddPrefab, SetCellPosition(coord.x, coord.y), Quaternion.identity, this.transform);
                AddButton.GetComponent<HoneycombAddButton>().set(coord.x, coord.y);
                honeycomb[coord.x + 3, coord.y + 3] = AddButton;
            }
        }
    }

    List<Coords> Circumference()
    {
        Queue<Coords> q = new Queue<Coords>();
        List<Coords> list = new List<Coords>();
        bool[,] visit = new bool[7, 7];

        Coords[] dist = { new Coords(1, 0), new Coords(1, 1), new Coords(0, 1), new Coords(-1, 0), new Coords(-1, -1), new Coords(0, -1) };

        q.Enqueue(new Coords(0,0));
        while (q.Count > 0)
        { 
            Coords coord = q.Dequeue();
            for (int i = 0; i < 6; i++)
            {
                Coords searchCoord = coord + dist[i];
                if (searchCoord.x < -3 || searchCoord.x > 3 || searchCoord.y < -3 || searchCoord.y > 3 || searchCoord.x - searchCoord.y > 3 || searchCoord.x - searchCoord.y < -3) continue;
                if (visit[searchCoord.x + 3, searchCoord.y + 3] == true) continue;
                visit[searchCoord.x + 3, searchCoord.y + 3] = true;
                if (honeycomb[searchCoord.x + 3, searchCoord.y + 3] == null)
                {
                    list.Add(searchCoord);
                }
                else if (honeycomb[searchCoord.x + 3, searchCoord.y + 3].GetComponent<HoneycombCell>() != null) 
                {
                    q.Enqueue(searchCoord);
                }
            }
        }
        //Debug.Log(list.Count);
        return list;
    }

    Vector3 SetCellPosition(int x,int y)
    {
        return x * cellSize * xVector + y * cellSize * yVector + this.transform.position;
    }
}
