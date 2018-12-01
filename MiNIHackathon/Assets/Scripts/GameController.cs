using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class GameController : MonoBehaviour {
    public GameObject Cursor;

    public Spawnable spawnObjectPrefab;

    public GameObject spawnOrnamentPrefab;

    public int InitOrnamentsNumber = 5;
    private float startTime;

    private readonly float timeToAdd = 100;

    private bool startSpawning = false;
    private bool IsGravity = false;

    // Use this for initialization
    void Start () {
        if (!Cursor)
        {
            Debug.LogError("!Cursor");
        }
        if (!spawnObjectPrefab)
        {
            Debug.LogError("!spawnObjectPrefab");
        }

        //GravityOff();
    }
	
	// Update is called once per frame
	void Update () {
		if(startSpawning)
        {
            if (Time.time - startTime > timeToAdd)
            {
                startTime = Time.time;
                Vector3 treePos = GameObject.FindGameObjectWithTag("Tree").transform.position;
                float x = Random.Range(treePos.x - 1, treePos.x + 1);
                float y = 2f;
                float z = Random.Range(treePos.z -1, treePos.z + 1);
                var orn = Instantiate(spawnOrnamentPrefab, new Vector3(x, y, z), Quaternion.identity);
                Debug.Log(orn);
            }
        }
	}

    public void StartGame()
    {
        Debug.Log("GameController::StartGame");

        SpawnObject();
    }

    public void SpawnObject()
    {
        Spawn(spawnObjectPrefab, new Vector3(0, 0, 2), Quaternion.identity);
    }

    public void Spawn(
    Spawnable spawnObjectPrefab,
    Vector3 position,
    Quaternion rotation)
    {
        Debug.Log("Spawner::Spawn");
        var spawnedObject = Instantiate(spawnObjectPrefab, position, rotation, null);

        SpawnInit(spawnedObject);
    }

    private void SpawnInit(Spawnable spawnedObject)
    {
        spawnedObject.Init(Cursor);
        var rigidbody = spawnedObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.isKinematic = !IsGravity;
        }
    }

    public void ToggleGravity()
    {
        if (IsGravity)
        {
            GravityOff();
        }
        else
        {
            GravityOn();
        }
    }

    public void GravityOn()
    {
        IsGravity = true;

        Debug.Log("Spawner::GravityOn");

        var foundObjects = FindObjectsOfType<Rigidbody>();
        foreach(var foundObject in foundObjects)
        {
            foundObject.isKinematic = false;
        }
    }

    public void GravityOff()
    {
        IsGravity = false;

        Debug.Log("Spawner::GravityOff");

        var foundObjects = FindObjectsOfType<Rigidbody>();
        foreach (var foundObject in foundObjects)
        {
            foundObject.isKinematic = true;
        }
    }

    public void SpawnOrnaments()
    {
        Random r = new Random();
        for (int i = 0; i < InitOrnamentsNumber; i++)
        {
            Vector3 treePos = GameObject.FindGameObjectWithTag("Tree").transform.position;
            float x = Random.Range(treePos.x - 1, treePos.x + 1);
            float y = 2f;
            float z = Random.Range(treePos.z - 1, treePos.z + 1);
            var orn = Instantiate(spawnOrnamentPrefab, new Vector3(x,y,z), Quaternion.identity);
            Debug.Log(orn);
        }
        startSpawning = true;
    }
}
