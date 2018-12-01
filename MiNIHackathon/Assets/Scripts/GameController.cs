using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class GameController : MonoBehaviour {
    public GameObject Cursor;

    public Spawnable spawnObjectPrefab;

    public GameObject starPrefab;
    public GameObject spawnOrnamentPrefab1;
    public GameObject spawnOrnamentPrefab2;
    public GameObject spawnOrnamentPrefab3;

    public int InitOrnamentsNumber = 5;
    private float startTime;

    private readonly float timeToAdd = 5;

    private bool startSpawning = false;
    private bool IsGravity = false;

    private readonly int numberOfOrnamentsPerLevel = 6;

    private int levelIndex = 0;
    private int index = 0;

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
                float y = 0.5f;
                float z = Random.Range(treePos.z -1, treePos.z + 1);
                GameObject orn = null;
                switch (Random.Range(1, 4))
                {
                    case 1: orn = Instantiate(spawnOrnamentPrefab1, new Vector3(x, y, z), Quaternion.identity); break;
                    case 2: orn = Instantiate(spawnOrnamentPrefab2, new Vector3(x, y, z), Quaternion.identity); break;
                    case 3: orn = Instantiate(spawnOrnamentPrefab3, new Vector3(x, y, z), Quaternion.identity); break;
                }
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
            float y = 0.5f;
            float z = Random.Range(treePos.z - 1, treePos.z + 1);
            GameObject orn = null;
            switch(Random.Range(1, 4))
            {
                case 1: orn = Instantiate(spawnOrnamentPrefab1, new Vector3(x, y, z), Quaternion.identity); break;
                case 2: orn = Instantiate(spawnOrnamentPrefab2, new Vector3(x, y, z), Quaternion.identity); break;
                case 3: orn = Instantiate(spawnOrnamentPrefab3, new Vector3(x, y, z), Quaternion.identity); break;
            }
            Debug.Log(orn);
        }
        startSpawning = true;
    }

    public void AddOrnament()
    {
        Debug.Log("here");
        Debug.Log(GameObject.Find("lvl1").transform.childCount);
        switch (levelIndex)
        {
            case 0: GameObject.Find("lvl1").transform.GetChild(index++).transform.GetChild(0).gameObject.SetActive(true); break;
            case 1: GameObject.Find("lvl2").transform.GetChild(index++).transform.GetChild(0).gameObject.SetActive(true); break;
            case 2: GameObject.Find("lvl3").transform.GetChild(index++).transform.GetChild(0).gameObject.SetActive(true); break;
        }
        if (index == 6)
        {
            index = 0;
            levelIndex++;
        }
        if (levelIndex == 3)
            EndGame();
    }

    private void EndGame()
    {
        GameObject star = GameObject.Find("star").gameObject;
        Vector3 starPos = star.transform.position;
        star = Instantiate(starPrefab, starPos, Quaternion.identity);
    }
}
