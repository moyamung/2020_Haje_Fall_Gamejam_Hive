using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WorkerBeePrefab;
    public Transform WorkerBeeParent;

    List<GameObject> Workers = new List<GameObject>();

    public static GameManager gameManager;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBee()
    {
        float theta = Random.Range(0, 2 * Mathf.PI);
        Vector3 vec = new Vector3(5 * Mathf.Cos(theta), 5 * Mathf.Sin(theta), 0f);
        GameObject WorkerBee = Instantiate(WorkerBeePrefab, vec, Quaternion.identity, WorkerBeeParent);
        Workers.Add(WorkerBee);
    }
}
