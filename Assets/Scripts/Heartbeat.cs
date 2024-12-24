using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Heartbeat : MonoBehaviour
{
    public float minHeartbeatTime = 0.1f;
    public float minHeartbeatDist = 3.0f;
    public float maxHeartbeatTime = 1.0f;
    public float maxHeartbeatDist = 10.0f;

    private GameObject playerObj;
    private float timeSinceLastBeat = 0.0f;
    private float nextBeatTime = 0.0f;
    // private float playerDist = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastBeat += Time.deltaTime;

        if (timeSinceLastBeat >= nextBeatTime)
        {
            SendMessage("OnHeartbeat");
            timeSinceLastBeat = 0.0f;
            nextBeatTime = GetNextHeartbeatTime();
            Debug.Log($"NextHeartbeatTime: {nextBeatTime}");
        }
    }

    private float GetNextHeartbeatTime()
    {
        float dist = (playerObj.transform.position - transform.position).magnitude;
        if (dist <= minHeartbeatDist)
        {
            return minHeartbeatTime;
        }

        if (dist >= maxHeartbeatDist) 
        {
            return maxHeartbeatTime;
        }

        return minHeartbeatTime + (maxHeartbeatTime - minHeartbeatTime) * (dist - minHeartbeatDist) / (maxHeartbeatDist - minHeartbeatDist);
    }
}
