using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint[] waypointChildren; //waypoints just need to keep track of their
    //children, which are the possible waypoints the player can go to next
    //afer reaching this one
}