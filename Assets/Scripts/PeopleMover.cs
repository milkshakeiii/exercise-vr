using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMover : MonoBehaviour
{
    public Waypoint waypointHead;

    public CharacterController character;

    public UnityEngine.UI.Text speedometer;

    public float arrivalThreshhold = 1f;

    public float text_update_freq = 0.5f;

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

    public void ReportVelocity(float velocity)
    {
        Vector3 move_amount = velocity * Vector3.Normalize(nextWaypoint.transform.position - character.transform.position);
        character.SimpleMove(move_amount);
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