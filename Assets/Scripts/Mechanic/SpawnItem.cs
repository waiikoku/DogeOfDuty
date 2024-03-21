using Dod;
using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : Singleton<SpawnItem>
{
    [SerializeField] private Dropitem[] itemList;

    [SerializeField] private float radius = 1f;
    private Vector3 offset = new Vector3(0, 45, 0);
    [SerializeField] private float delay = 1f;

    public int limitItem = 10;
    private int currentSpawnItem;
    protected override void Awake()
    {
        base.Awake();
        InvokeRepeating(nameof(Spawn), 0, delay);
    }
    public void Spawn()
    {
        if (currentSpawnItem >= limitItem) return;
        /*
        Vector3 position = UnityEngine.Random.insideUnitCircle * radius;
        position.z = position.y;
        position.y = 0;
        int index = Random.Range(0, itemList.Length);
        Instantiate(itemList[index], position, Quaternion.Euler(offset));
        */
        var pos = GetRandomPositionInGrid();
        int index = Random.Range(0, itemList.Length);
        var drop = Instantiate(itemList[index], pos, Quaternion.Euler(offset));
        drop.connectToSpawn = true;
        currentSpawnItem++;
    }

    public BoxCollider boxCollider;
    public float constantY = 0.0f; // Set your desired constant Y value

    // Call this function to get a random position within the bounds of the BoxCollider
    public Vector3 GetRandomPositionInBox()
    {
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider not assigned!");
            return Vector3.zero;
        }

        // Get the extents of the BoxCollider
        Vector3 extents = boxCollider.bounds.extents;

        // Generate a random position within the bounds of the BoxCollider
        Vector3 randomPosition = new Vector3(
            Random.Range(-extents.x, extents.x),
            constantY, // Set a constant Y value
            Random.Range(-extents.z, extents.z)
        );

        // Return the random position within the bounds
        return boxCollider.transform.position + randomPosition;
    }

    public void RemoveBox()
    {
        currentSpawnItem--;
    }

    public int gridRows = 5;
    public int gridColumns = 5;
    public float padding = 2.0f; // Set your desired padding value

    // Call this function to get a single random position within the bounds of the BoxCollider and in the grid
    public Vector3 GetRandomPositionInGrid()
    {
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider not assigned!");
            return Vector3.zero;
        }

        // Get the extents of the BoxCollider
        Vector3 extents = boxCollider.bounds.extents;

        // Calculate the spacing between positions
        float spacingX = (extents.x * 2 - padding * (gridColumns - 1)) / (gridColumns - 1);
        float spacingZ = (extents.z * 2 - padding * (gridRows - 1)) / (gridRows - 1);

        // Generate a random position within the grid
        float x = Random.Range(-extents.x, extents.x);
        float z = Random.Range(-extents.z, extents.z);

        // Snap the position to the grid
        int gridX = Mathf.FloorToInt((x + extents.x) / spacingX);
        int gridZ = Mathf.FloorToInt((z + extents.z) / spacingZ);

        x = -extents.x + gridX * (spacingX + padding);
        z = -extents.z + gridZ * (spacingZ + padding);

        // Return the random position within the bounds and in the grid
        return boxCollider.transform.position + new Vector3(x, constantY, z);
    }
}
