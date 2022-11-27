using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : MonoBehaviour
{
    private RecyclableObject _prefab;
    private HashSet<RecyclableObject> _instantiatedObjects;
    private Queue<RecyclableObject> _recycledObjects;

    private Transform _objectPoolInstanceContainer;

    public ObjectPool(RecyclableObject prefab, Transform objectPoolInstanceContainer)
    {
        _prefab = prefab;
        _objectPoolInstanceContainer = objectPoolInstanceContainer;
    }

    public void Init(int numberOfInstances)
    {
        _instantiatedObjects = new HashSet<RecyclableObject>();
        _recycledObjects = new Queue<RecyclableObject>();

        for (int i = 0; i < numberOfInstances; i++)
        {
            RecyclableObject instantiatedObject = InstantiateNewObject();
            instantiatedObject.gameObject.SetActive(false);
            instantiatedObject.transform.SetParent(_objectPoolInstanceContainer);
            _recycledObjects.Enqueue(instantiatedObject);
        }
    }

    private RecyclableObject InstantiateNewObject()
    {
        RecyclableObject instantiatedObject = GameObject.Instantiate(_prefab);
        instantiatedObject.Configure(this);
        return instantiatedObject;
    }

    public T Spawn<T>(Vector3 spawnPosition)
    {
        RecyclableObject objectToSpawn = GetRecycledObject();
        _instantiatedObjects.Add(objectToSpawn);
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = spawnPosition;
        objectToSpawn.Init();
        return objectToSpawn.GetComponent<T>();
    }

    private RecyclableObject GetRecycledObject()
    {
        if (_recycledObjects.Count > 0)
        {
            RecyclableObject recycledObject = _recycledObjects.Dequeue();
            recycledObject.transform.SetParent(null);
            return recycledObject;
        }

        Debug.LogWarning("The object pool is empty and you have instantiated a new instance. Consider increasing the initial number of instances");
        RecyclableObject insantiatedObject = InstantiateNewObject();
        return insantiatedObject;
    }

    public void RecycleGameObject(RecyclableObject objectToRecycle)
    {
        bool wasInstantiated = _instantiatedObjects.Remove(objectToRecycle);
        Assert.IsTrue(wasInstantiated, "[ObjectPool at RecycleObject]: The object does not belong to this object pool");

        objectToRecycle.gameObject.SetActive(false);
        objectToRecycle.Release();
        objectToRecycle.transform.SetParent(_objectPoolInstanceContainer);
        _recycledObjects.Enqueue(objectToRecycle);
    }
}
