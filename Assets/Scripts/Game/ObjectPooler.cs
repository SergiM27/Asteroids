using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects() //For each pool in the pools list, instantiate all the objects it contains, based on its size. This is done at the beggining of the Game scene.
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                AddNewObject(pool, objectPool);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private void AddNewObject(Pool pool, Queue<GameObject> objectPool)
    {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    public GameObject GetPooledObject(string tag, Vector3 position, Quaternion rotation) //Function used by other scripts to get what object is available to be used that time.
    {

        if (!poolDictionary.ContainsKey(tag))//If the tag doesnt exist in the list of gameobjects, return null.
        {
            print("Spawn didn't work");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue(); //Take the object out of the list

        //Set the object's rotation and position to the ones passed by parameter, and set active true.
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objectToSpawn); //Put the object again to the list, but at last position.
        return objectToSpawn;


    }

}
