using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
 *  This class defines a 'Manager' for requesting AStar paths from the pathfinder 
 * 
 */

public class PathRequestManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    AStarPathfinder pathfinding;

    bool isProcessingPath;


    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<AStarPathfinder>();
    }



    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStartPoint, currentPathRequest.pathEndPoint);
        }
    }

    public void CurrentPathFound(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }


    // Defines a path request
    struct PathRequest
    {
        public Vector3 pathStartPoint;
        public Vector3 pathEndPoint;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStartPoint = _start;
            pathEndPoint = _end;
            callback = _callback;
        }

    }
}
