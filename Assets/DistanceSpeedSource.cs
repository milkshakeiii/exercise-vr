using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSpeedSource : MonoBehaviour
{
    public bool mouseTest = false;
    public PeopleMover peopleMover;
    public GameObject head;
    public float buffer_size = 1f;
    public float speed_multiplier = 3f;
    public float threshhold = 1f;

    private Queue<float> running_sum = new Queue<float>();
    private Queue<float> samples_times = new Queue<float>();
    private float index_time = 0f;
    private int last_index;
    private int active_index;
    private float last_sum = 0f;
    private float last_sample = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < buffer_size; i++)
        //{
            //local_minmax_me.Enqueue(Input.mousePosition.y);
            //local_minmax_me.Enqueue(GetHeadsetY());
            //samples_times.Enqueue(Time.time);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        float sample;
        if (mouseTest)
            sample = Input.mousePosition.y;
        else
            sample = GetHeadsetY();

        float difference;
        if (last_sample == 0f)
            difference = 0f;
        else
            difference = Mathf.Abs(last_sample - sample);

        float this_sum = last_sum + difference;
        running_sum.Enqueue(this_sum);
        samples_times.Enqueue(Time.time);
        last_sample = sample;
        last_sum = this_sum;

        float oldest_time = samples_times.Peek();
        float buffer_length = Time.time - oldest_time;
        
        if (buffer_length > buffer_size)
        {
            running_sum.Dequeue();
            samples_times.Dequeue();
        }

        float oldest_sum = running_sum.Peek();
        float total_distance = this_sum - oldest_sum;
        float distance_per_second = total_distance / buffer_size;

        if (distance_per_second < threshhold)
            distance_per_second = 0f;

        peopleMover.ReportVelocity(speed_multiplier * Mathf.Pow(distance_per_second, 1.5f));
    }

    private float GetHeadsetY()
    {
        return head.transform.position.y;
    }
}
