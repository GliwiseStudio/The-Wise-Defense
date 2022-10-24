using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] waypoints; // an array to store each waypoint on the way (a waypoint is basically a coordinate)

    private void Awake()
    {
        waypoints = new Transform[transform.childCount]; // initialice the array to the number of child objects of the WaypointsHolder

        for (int i = 0; i < waypoints.Length; i++) // loop to add every waypoint to the array
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
}
