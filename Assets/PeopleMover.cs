﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMover : MonoBehaviour
{
    public Waypoint waypointHead;

    public CharacterController character;

    public int running_average_size = 5;
    public float arrivalThreshhold = 1f;

    private float[] reported_velocities;
    private int rotating_index = 0;

    private Waypoint currentWaypoint;
    private Waypoint nextWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        reported_velocities = new float[running_average_size];

        currentWaypoint = waypointHead;
        nextWaypoint = waypointHead.waypointChildren[0];
        character.transform.position = currentWaypoint.transform.position;
        character.transform.LookAt(nextWaypoint.transform);
    }

    public void ReportVelocity(float new_target_velocity)
    {
        reported_velocities[rotating_index] = new_target_velocity;
        rotating_index = (rotating_index + 1) % running_average_size;
    }

    // Update is called once per frame
    void Update()
    {
        //update velocity
        //current_velocity = target_velocity;
        float sum = 0;
        foreach (float v in reported_velocities)
            sum += v;
        float current_velocity = sum / running_average_size;

        //update current/next waypoints
        if (Vector3.Distance(character.transform.position, nextWaypoint.transform.position) < arrivalThreshhold)
        {
            currentWaypoint = nextWaypoint;
            nextWaypoint = currentWaypoint.waypointChildren[Random.Range(0, currentWaypoint.waypointChildren.Length)];
        }

        //move
        if (!float.IsNaN(current_velocity))
        {
            //Debug.Log(current_velocity * Vector3.Normalize(nextWaypoint.transform.position - character.transform.position));
            character.SimpleMove(current_velocity * Vector3.Normalize(nextWaypoint.transform.position - character.transform.position));
        }
    }
}
