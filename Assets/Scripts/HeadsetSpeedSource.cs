using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accord.Math;

//this is the fourier transform-based approach
public class HeadsetSpeedSource : MonoBehaviour
{
    public PeopleMover peopleMover;
                                    //all speed sources must report speed to
                                    //PeopleMover script
    public GameObject head;
                                    //the y position of this object will
                                    //be used as input for the fft
    public const float patience_constant = 0f;
                                    //how long to wait for an established
                                    //frequency before reporting speed change
    public const int buffer_size = 128;
                                    //for the discrete fft, this must be
                                    //a power of 2
    public float speed_multiplier = 3f;
                                    //constant multiplier to tune speed

    private Queue<System.Numerics.Complex> fft_me = new Queue<System.Numerics.Complex>();
                                    //contains the samples from headset height
    private Queue<float> samples_times = new Queue<float>();
                                    //contains corresponding times for each sample
    private float index_time = 0f; //most recent freq first time of appearance
    private int last_index; //most recent freq
    private int active_index; //reported freq

    // Start is called before the first frame update
    void Start()
    {
        //fill buffer with steady state
        for (int i = 0; i < buffer_size; i++)
        {
            fft_me.Enqueue(GetHeadsetY());
            samples_times.Enqueue(Time.time);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //update buffer and corresponing times
        fft_me.Enqueue(GetHeadsetY());
        fft_me.Dequeue();
        samples_times.Enqueue(Time.time);
        float popped_time = samples_times.Dequeue();

        //perform the fft
        System.Numerics.Complex[] fft_me_array = fft_me.ToArray();
        Accord.Math.FourierTransform.FFT(fft_me_array, FourierTransform.Direction.Forward);
        //fft_me now contains frequency data, convert it to floats
        float[] floats = new float[fft_me_array.Length];
        for (int i = 0; i < floats.Length; i++)
        {
            floats[i] = (float)fft_me_array[i].Magnitude;
        }
        floats[0] = 0;
        //find the dominant frequency
        float best_value = Mathf.Max(floats);
        int best_index = floats.IndexOf<float>(best_value);//index of dominant frequency
        Debug.Log(best_index);
        if (best_index <= 1) //clamp very slow frequencies
            best_index = 0;

        if (best_index != last_index)
        { //update time of index, if it stays the same this time
          //will get further in the past
            last_index = best_index;
            index_time = Time.time;
        }
        if (Time.time - index_time > patience_constant)
        {  //once we have established an index for a number of seconds,
           //switch to reporting that one.
           //with patience constant set to 0, we will always report
           //the most recet index
            active_index = best_index;
        }

        float buffer_length = Time.time - popped_time;
        float cycles_per_second = active_index / buffer_length; //note use of active_index

        peopleMover.ReportVelocity(speed_multiplier * cycles_per_second * cycles_per_second);
    }

    private float GetHeadsetY()
    {
        Debug.Log(head.transform.position.y);
        return head.transform.position.y;
    }
}
