using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeedSource : MonoBehaviour
{
    public PeopleMover peopleMover;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        peopleMover.ReportVelocity(Random.Range(0, 10));
    }
}
