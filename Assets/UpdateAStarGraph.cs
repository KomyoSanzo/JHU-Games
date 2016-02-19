using UnityEngine;
using System.Collections;
using Pathfinding;
/// <summary>
/// Refreshes the Astar graph every 1/updateRate seconds
/// </summary>
public class UpdateAStarGraph : MonoBehaviour {
    public float updateRate = 2f;


	// Use this for initialization
	void Start () {
        AstarPath.active.Scan();
        StartCoroutine(ScanPaths());

    }

    IEnumerator ScanPaths()
    {
        AstarPath.active.Scan();
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(ScanPaths());
    }
}
