using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeySpeedSource : MonoBehaviour
{
    public bool mouseTest = false;
                                    //if checked in the unity editor,
                                    //use the mouse Y instead of headset Y
    public PeopleMover peopleMover;
                                    //like other speed sources do,
                                    //report the speed to this once calculated
    public GameObject head;
                                    //this is the object which tracks the headset,
                                    //we use its Y position
    public float buffer_size = 1f;
                                    //how many seconds of samples to account for
    public float speed_multiplier = 3f;
                                    //constant multiplier to tune speed
    public float height_requirement = 0.1f;
                                    //only report spikes the fall outside of this
                                    //distance from each other

    private Queue<float> local_minmax_me = new Queue<float>();//sample buffer
    private Queue<float> samples_times = new Queue<float>();  //corresponding times


    // Update is called once per frame
    void Update()
    {
        //update buffer and times
        if (mouseTest)
            local_minmax_me.Enqueue(Input.mousePosition.y);
        else
            local_minmax_me.Enqueue(GetHeadsetY());
        samples_times.Enqueue(Time.time);

        float oldest_time = samples_times.Peek();
        float buffer_length = Time.time - oldest_time;

        if (buffer_length > buffer_size)
        {
            local_minmax_me.Dequeue();
            samples_times.Dequeue();
        }
        
        //count spikes
        //spikes are whenver an arrangement of 3 samples occurs where the local min
        //or max is the middle of the 3 samples
        List<float> minmaxes = new List<float>();
        int local_minmaxes = 0;
        float twoprev = local_minmax_me.Peek();
        float prev = local_minmax_me.Peek();
        int ignore = 2;
        foreach (float sample in local_minmax_me)
        {
            if (ignore > 0)
            {
                ignore--;
                continue;
            }
            if (sample == prev)
            {
                continue;
            }

            if ((twoprev < prev && prev > sample) || (twoprev > prev && prev < sample))
            {
                local_minmaxes++;
                minmaxes.Add(sample);
            }
            twoprev = prev;
            prev = sample;
        }
        //disregard spikes seperated by less than the height requirements
        for (int i = 1; i < minmaxes.Count; i++)
        {
            if (Mathf.Abs(minmaxes[i] - minmaxes[i - 1]) < height_requirement)
                local_minmaxes--;
        }

        //if there are few spikes, clamp to zero
        if (local_minmaxes <= 2)
            local_minmaxes = 0;

        float minmax_per_second = local_minmaxes / buffer_size;
        //the speed is simple a function of how many spikes per second we have
        peopleMover.ReportVelocity(speed_multiplier * minmax_per_second);
    }

    private float GetHeadsetY()
    {
        return head.transform.position.y;
    }
}
