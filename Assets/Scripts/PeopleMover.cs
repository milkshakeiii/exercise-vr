using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMover : MonoBehaviour
{
    public Waypoint waypointHead;
                                    //this points to the head of the waypoint tree
                                    //which is also the starting position of the player
    public CharacterController character;
                                    //this points to the script on the player object
    public UnityEngine.UI.Text speedometer;
                                    //upate this text with speed
    public float arrivalThreshhold = 1f;
                                    //consider a waypoint reached when within
                                    //this distance of it
    public float text_update_freq = 0.5f;
                                    //update the speed text every this many seconds

    private float last_update = 0f;
    private Waypoint currentWaypoint;
    private Waypoint nextWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = waypointHead;
        nextWaypoint = waypointHead.waypointChildren[0];
        character.transform.position = currentWaypoint.transform.position;
        character.transform.LookAt(nextWaypoint.transform);
    }

    //speed sources call this every frame
    public void ReportVelocity(float velocity)
    {
        Vector3 move_amount = velocity * Vector3.Normalize(nextWaypoint.transform.position - character.transform.position);
        character.SimpleMove(move_amount); //move the player
        if (Time.time - last_update > text_update_freq)
        {
            last_update = Time.time;
            speedometer.text = (velocity * 2.23694).ToString("F1") + " mph";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //update current/next waypoints
        if (Vector3.Distance(character.transform.position, nextWaypoint.transform.position) < arrivalThreshhold)
        {
            currentWaypoint = nextWaypoint;
            nextWaypoint = currentWaypoint.waypointChildren[Random.Range(0, currentWaypoint.waypointChildren.Length)];
            character.transform.LookAt(nextWaypoint.transform);
        }
    }
}