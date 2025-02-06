using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour 
{
    [SerializeField] private GameObject[] blockPrefabs; // Array of block prefabs (for each type)
    [SerializeField] private int initialPoolSize = 10; // Number of blocks to pre-instantiate for each type
    
    private readonly Dictionary<int, Queue<GameObject>> _blockPools = new();

    private void Awake()
    {
        // Initialize a pool for each block type
        for (int i = 0; i < blockPrefabs.Length; i++)
        {
            _blockPools[i] = new Queue<GameObject>();
            InitializePool(i, initialPoolSize);
        }
    }

    // Retrieve a block of the specified type
    public GameObject GetBlock(int blockType)
    {
        if (_blockPools[blockType].Count > 0)
        {
            GameObject block = _blockPools[blockType].Dequeue();
            block.SetActive(true);
            return block;
        }

        // If the pool is empty, create a new block
        GameObject newBlock = Instantiate(blockPrefabs[blockType]);
        return newBlock;
    }

    // Return a block to the pool
    public void ReturnBlock(GameObject block, int blockType)
    {
        block.SetActive(false);
        _blockPools[blockType].Enqueue(block);
    }
    
    
    // Pre-instantiate blocks for a specific type
    private void InitializePool(int blockType, int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject block = Instantiate(blockPrefabs[blockType]);
            block.SetActive(false);
            _blockPools[blockType].Enqueue(block);
        }
    }
}