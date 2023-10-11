using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using UnityEngine;

public class PoolEmptyException : Exception
{
    public PoolEmptyException() : base("The object pool is empty.") { }
}

public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField] int capacity;
    [SerializeField] GameObject objToPool;
    ConcurrentBag<GameObject> bag;

    void Awake()
    {
        bag = new ConcurrentBag<GameObject>();
        for (int i = 0; i < capacity; i++)
        {
            var obj = Instantiate(objToPool, transform);
            obj.layer = objToPool.layer;
            obj.SetActive(false);
            bag.Add(obj);
        }
    }
    protected abstract IEnumerator ReturnCoroutine(GameObject obj);

    public GameObject SpawnObject(Transform transform, out Coroutine p_returnCoroutine)
    {
        if (bag.TryTake(out GameObject obj))
        {
            obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
            obj.SetActive(true);
            p_returnCoroutine = StartCoroutine(ReturnCoroutine(obj));
            return obj;
        }
        throw new PoolEmptyException();
    }
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        bag.Add(obj);
    }
}