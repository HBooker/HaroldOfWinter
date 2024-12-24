using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject treeShardPrefab;
    public GameObject snowShardPrefab;
    public GameObject giftsShardPrefab;
    public GameObject lightsShardPrefab;

    public int gridSizeX = 6;
    public int gridSizeZ = 6;
    //public int numTreeShards = 1;
    // public int numSnowShards = 19;
    public int numGiftsShards = 9;
    public int numLightsShards = 3;
    //public int numIndoorShards = 8;

    // Start is called before the first frame update
    void Start()
    {
        int gridSize = gridSizeX * gridSizeZ;
        int[] shards = new int[gridSize];

        for (int i = 0; i < gridSize; ++i)
        {
            // C# probably inits the array to 0s but I don't have time to test that
            shards[i] = 0;
        }

        // add tree shard
        shards[15] = 1;

        // add light shards
        for (int i = 0; i < numLightsShards; ++i)
        {
            int nextIndex = -1;
            do
            {
                nextIndex = Random.Range(0, gridSize);
            }
            while (shards[nextIndex] > 0);
            shards[nextIndex] = 2;
        }

        // add gifts shards
        for (int i = 0; i < numGiftsShards; ++i)
        {
            int nextIndex = -1;
            do
            {
                nextIndex = Random.Range(0, gridSize);
            }
            while (shards[nextIndex] > 0);
            shards[nextIndex] = 3;
        }

        // fill in with snow shards
        for (int i = 0; i < gridSize; ++i)
        {
            if (shards[i] == 0)
            {
                shards[i] = 4;
            }
        }

        for (int i = 0; i < shards.Length; ++i)
        {
            int x = i % gridSizeX;
            int z = i / gridSizeZ;

            switch (shards[i])
            {
                case 1:
                    Instantiate(
                        treeShardPrefab,
                        new Vector3(x * 10.0f, 0.0f, z * 10.0f),
                        Quaternion.identity);
                    break;
                case 2:
                    Instantiate(
                        lightsShardPrefab,
                        new Vector3(x * 10.0f, 0.0f, z * 10.0f),
                        Quaternion.identity);
                    break;
                case 3:
                    Instantiate(
                        giftsShardPrefab,
                        new Vector3(x * 10.0f, 0.0f, z * 10.0f),
                        Quaternion.identity);
                    break;
                case 4:
                    Instantiate(
                        snowShardPrefab,
                        new Vector3(x * 10.0f, 0.0f, z * 10.0f),
                        Quaternion.identity);
                    break;
                default:
                    Debug.LogError("Invalid shard type during map gen");
                    break;
            }
        }

        // just put the player on the tree shard
        //Instantiate(playerPrefab, new Vector3(gridSizeX*5.0f, 0.03f, gridSizeZ*5.0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
