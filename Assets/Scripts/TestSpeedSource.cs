using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedSource : MonoBehaviour
{
    public PeopleMover peopleMover;

    // Just call report velocity with a random number to test
    void Update()
    {
        peopleMover.ReportVelocity(Random.Range(0, 10));
    }
}
