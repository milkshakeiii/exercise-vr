using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeySpeedSource : MonoBehaviour
{
    public PeopleMover peopleMover;
    public GameObject head;
    public float buffer_size = 1f;
    public float speed_multiplier = 3f;

    private Queue<float> local_minmax_me = new Queue<float>();
    private Queue<float> samples_times = new Queue<float>();
    private float index_time = 0f;
    private int last_index;
    private int active_index;

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
        local_minmax_me.Enqueue(Input.mousePosition.y);
        //local_minmax_me.Enqueue(GetHeadsetY());
        samples_times.Enqueue(Time.time);

        float oldest_time = samples_times.Peek();
        float buffer_length = Time.time - oldest_time;
        //Debug.Log(local_minmax_me.Count);
        //Debug.Log(buffer_length);

        if (buffer_length > buffer_size)
        {
            local_minmax_me.Dequeue();
            samples_times.Dequeue();
        }
        

        List<float> minmaxes = new List<float>();
        int local_minmaxes = 0;
        float twoprev = 0f;
        float prev = 0f;
        foreach (float sample in local_minmax_me)
        {
            if ((twoprev < prev && prev > sample) || (twoprev > prev && prev < sample))
            {
                Debug.Log("beep");
                local_minmaxes++;
                minmaxes.Add(sample);
            }
            twoprev = prev;
            prev = sample;
        }
        //Debug.Log(local_minmaxes);

        float minmax_per_second = local_minmaxes / buffer_length;

        //peopleMover.ReportVelocity(5);
        peopleMover.ReportVelocity(speed_multiplier * minmax_per_second * minmax_per_second);
    }

    private float GetHeadsetY()
    {
        Debug.Log(head.transform.position.y);
        return head.transform.position.y;
    }
}
