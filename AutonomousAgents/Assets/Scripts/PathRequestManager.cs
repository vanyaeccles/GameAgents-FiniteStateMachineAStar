using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
 *  This class defines a 'Manager' for requesting AStar paths from the pathfinder 
 */

public class PathRequestManager : MonoBehaviour
{
    // The queue of path requests, 
    //c# queues are basically a list but can only add to the end or remove from the beginning (First-in,First-out)
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>(); 
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    AStarPathfinder pathfinding;

    bool isProcessingPath;

    // Initialises
    void Awake()
    {
        instance = this; //Singleton
        pathfinding = GetComponent<AStarPathfinder>();
    }

    // 
    // - uses Action delegate - this is simply an encapsulated method that returns no value, or a void encapsulated method
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, bool isSoundPath, Action<Vector3[], bool, bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, isSoundPath, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.ProcessNextRequest();
    }

    // Moves on to the next request in the queue
    void ProcessNextRequest()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue(); // gets the first one out to process
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStartPoint, currentPathRequest.pathEndPoint, currentPathRequest.isSoundPath);
        }
    }

    // 
    public void CurrentPathFound(Vector3[] path, bool success, bool isSoundPath)
    {
        currentPathRequest.callback(path, success, isSoundPath); // uses the Action delegate to return the path vector and the bool to the 'Follow path' method of the agent
        isProcessingPath = false;
        ProcessNextRequest();
    }


    // Defines a path request
    struct PathRequest
    {
        public Vector3 pathStartPoint;
        public Vector3 pathEndPoint;
        public bool isSoundPath;
        public Action<Vector3[], bool, bool> callback; // These are the parameters that are returned to the agent that requested the path (eg a vector of the path and whether a path was found)

        public PathRequest(Vector3 _start, Vector3 _end, bool _isSoundPath, Action<Vector3[], bool, bool> _callback)
        {
            pathStartPoint = _start;
            pathEndPoint = _end;
            isSoundPath = _isSoundPath;
            callback = _callback;
        }

    }
}
