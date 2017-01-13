using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimplePool
{
    GameObject prefab;
    LinkedList<GameObject> spawnQueue;

    public SimplePool(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public GameObject GetObject()
    {
        if (spawnQueue.Count > 0)
        {
            var o = spawnQueue.First.Value;
            spawnQueue.RemoveFirst();
            return o;
        }
        return Object.Instantiate(prefab);
    }

    public void ReturnObject(GameObject o)
    {
        spawnQueue.AddLast(o);
    }
}
