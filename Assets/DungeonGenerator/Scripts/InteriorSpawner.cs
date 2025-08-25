using System.Collections.Generic;
using UnityEngine;

public class InteriorSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("List of prefabs to spawn as interior items")]
    public List<GameObject> interiorPrefabs = new List<GameObject>();
    
    [Tooltip("Number of interior items to spawn")]
    public int numberOfItemsToSpawn = 5;
    
    [Tooltip("The area where items can be spawned (if empty, uses this object's bounds)")]
    public Collider2D spawnArea;
    
    [Header("Advanced Settings")]
    [Tooltip("Minimum distance between spawned items")]
    public float minDistanceBetweenItems = 1.0f;
    
    [Tooltip("Should items be spawned with random rotation?")]
    public bool randomRotation = true;
    
    [Tooltip("Maximum attempts to place an item before giving up")]
    public int maxPlacementAttempts = 30;

    void Start()
    {
        SpawnInteriorItems();
    }

    public void SpawnInteriorItems()
    {
        // Validate inputs
        if (interiorPrefabs == null || interiorPrefabs.Count == 0)
        {
            Debug.LogWarning("No interior prefabs assigned to the spawner.");
            return;
        }
        
        if (numberOfItemsToSpawn <= 0)
        {
            Debug.LogWarning("Number of items to spawn must be greater than zero.");
            return;
        }
        
        // Use this object's collider if no spawn area is specified
        Collider2D area = spawnArea != null ? spawnArea : GetComponent<Collider2D>();
        
        if (area == null)
        {
            Debug.LogError("No spawn area defined and no collider found on this GameObject.");
            return;
        }

        // Spawn the items
        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            SpawnItem(area);
        }
    }

    private void SpawnItem(Collider2D area)
    {
        // Select a random prefab from the list
        GameObject prefabToSpawn = interiorPrefabs[Random.Range(0, interiorPrefabs.Count)];
        
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("A prefab in the interiorPrefabs list is null.");
            return;
        }

        Vector2 spawnPosition;
        int attempts = 0;
        bool positionFound = false;

        // Try to find a valid position
        do
        {
            spawnPosition = GetRandomPointInCollider(area);
            attempts++;
            Debug.Log(spawnPosition);
            
            // Check if the position is far enough from other objects
            if (minDistanceBetweenItems <= 0 || IsPositionValid(spawnPosition))
            {
                positionFound = true;
            }
        }
        while (!positionFound && attempts < maxPlacementAttempts);

        // If we found a valid position, spawn the item
        if (positionFound)
        {
            Quaternion rotation = randomRotation ? 
                Quaternion.Euler(0, 0, Random.Range(0, 360)) : 
                prefabToSpawn.transform.rotation;
                
            Instantiate(prefabToSpawn, spawnPosition, rotation, transform);
        }
        else
        {
            Debug.LogWarning("Failed to find a valid position for an interior item after " + 
                            maxPlacementAttempts + " attempts.");
        }
    }

    private Vector2 GetRandomPointInCollider(Collider2D area)
    {
        Bounds bounds = area.bounds;
        
        Vector2 point = new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
        
        return point;
    }

    private bool IsPositionValid(Vector2 position)
    {
        // Check if there are any objects too close to this position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, minDistanceBetweenItems);
        
        foreach (Collider2D col in colliders)
        {
            // Ignore the spawn area collider and triggers
            if (col != spawnArea && !col.isTrigger && col.transform != transform)
            {
                return false;
            }
        }
        
        return true;
    }

    // Editor button to spawn items manually
    #if UNITY_EDITOR
    [ContextMenu("Spawn Items Now")]
    private void SpawnItemsEditor()
    {
        SpawnInteriorItems();
    }
    
    [ContextMenu("Remove All Spawned Items")]
    private void RemoveAllSpawnedItems()
    {
        // Remove all children that aren't the spawn area
        List<GameObject> toDestroy = new List<GameObject>();
        
        foreach (Transform child in transform)
        {
            if (child.gameObject != spawnArea?.gameObject)
            {
                toDestroy.Add(child.gameObject);
            }
        }
        
        foreach (GameObject obj in toDestroy)
        {
            if (Application.isPlaying)
            {
                Destroy(obj);
            }
            else
            {
                DestroyImmediate(obj);
            }
        }
    }
    #endif
}
