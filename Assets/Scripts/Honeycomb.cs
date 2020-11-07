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
    float cellSize = 100f;

    void Start()
    {
        AddCell(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        List<(int, int)> list = Circumference();
        foreach ((int x, int y) coord in list)
        {
            if (honeycomb[coord.x + 3, coord.y + 3] == null)
            {
                GameObject AddButton = Instantiate(honeycombAddPrefab, SetCellPosition(coord.x, coord.y), Quaternion.identity, this.transform);
                AddButton.GetComponent<HoneycombAddButton>().set(coord.x, coord.y);
                honeycomb[coord.x + 3, coord.y + 3] = AddButton;
            }
        }
    }

    List<(int, int)> Circumference()
    {
        Queue<(int x, int y)> q = new Queue<(int, int)>();
        List<(int x, int y)> list = new List<(int, int)>();
        bool[,] visit = new bool[7, 7];

        (int x, int y)[] dist = { (1, 0), (1, 1), (0, 1), (-1, 0), (-1, -1), (0, -1) };

        q.Enqueue((0, 0));
        while (q.Count > 0)
        { 
            (int, int) coord = q.Dequeue();
            for (int i = 0; i < 6; i++)
            {
                (int x, int y) searchCoord = MyAdd(coord, dist[i]);
                if (searchCoord.x < -3 || searchCoord.x > 3 || searchCoord.y < -3 || searchCoord.y > 3 || searchCoord.x - searchCoord.y > 3 || searchCoord.x - searchCoord.y < -3) continue;
                if (visit[searchCoord.x + 3, searchCoord.y + 3] == true) continue;
                visit[searchCoord.x + 3, searchCoord.y + 3] = true;
                if (honeycomb[searchCoord.x + 3, searchCoord.y + 3] == null)
                {
                    list.Add((searchCoord.x, searchCoord.y));
                }
                else if (honeycomb[searchCoord.x + 3, searchCoord.y + 3].GetComponent<HoneycombCell>() != null) 
                {
                    q.Enqueue((searchCoord.x, searchCoord.y));
                }
            }
        }
        Debug.Log(list.Count);
        return list;
    }
    
    (int, int) MyAdd((int x, int y) a, (int x, int y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    Vector3 SetCellPosition(int x,int y)
    {
        return x * cellSize * xVector + y * cellSize * yVector + this.transform.position;
    }
}
