using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accord.Math;

public class MousePosSpeedSource : MonoBehaviour
{
    public PeopleMover peopleMover;
    public const float patience_constant = 0f;
    public const int buffer_size = 128;

    private Queue<System.Numerics.Complex> fft_me = new Queue<System.Numerics.Complex>();
    private Queue<float> samples_times = new Queue<float>();
    private float index_time = 0f;
    private int last_index;
    private int active_index;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buffer_size; i++)
        {
            fft_me.Enqueue(Input.mousePosition.y);
            samples_times.Enqueue(Time.time);
        }
    }

    // Update is called once per frame
    void Update()
    {
        fft_me.Enqueue(Input.mousePosition.y);
        fft_me.Dequeue();
        samples_times.Enqueue(Time.time);
        float popped_time = samples_times.Dequeue();

        System.Numerics.Complex[] fft_me_array = fft_me.ToArray();
        Accord.Math.FourierTransform.FFT(fft_me_array, FourierTransform.Direction.Forward);
        float[] floats = new float[fft_me_array.Length];
        for (int i = 0; i < floats.Length; i++)
        {
            floats[i] = (float)fft_me_array[i].Magnitude;
        }
        floats[0] = 0;
        float best_value = Mathf.Max(floats);
        int best_index = floats.IndexOf<float>(best_value);
        //Debug.Log(best_index);
        if (best_index <= 1)
            best_index = 0;

        if (best_index != last_index)
        {
            last_index = best_index;
            index_time = Time.time;
        }
        if (Time.time - index_time > patience_constant)
        {
            active_index = best_index;
        }

        float buffer_length = Time.time - popped_time;
        float cycles_per_second = active_index / buffer_length;

        //peopleMover.ReportVelocity(5);
        peopleMover.ReportVelocity(cycles_per_second);
    }
}
