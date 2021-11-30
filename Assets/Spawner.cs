using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public static Spawner Instance{
        get {
            if(_instance == null) {
                _instance = new Spawner();
            }
            return _instance;
        }
    }
    private static Spawner _instance;


    public GameObject objectToSpawn;
    public GameObject parent;
    public int numberToSpawn;
    public int limit;
    public float rate;

    float spawnTimer;


    void Awake()
    {
        _instance = this;
        spawnTimer = rate;
    }

    void Update()
    {
        if (parent.transform.childCount < limit) {
            spawnTimer -= Time.deltaTime;
        }
    }

    public void SpawnObject() {
        if (spawnTimer <= 0f) {
            for (int i = 0; i < numberToSpawn; i++) {
                Instantiate(objectToSpawn, new Vector3(this.transform.position.x + GetModifier(), this.transform.position.y + GetModifier()), Quaternion.identity, parent.transform);               
            }
            spawnTimer = rate;
        }
    }
    
    float GetModifier() {
        float modifier = Random.Range(0f, 1f);
        if (Random.Range(0,2) > 1)
        return -modifier;
        else
        return modifier;
    }
}