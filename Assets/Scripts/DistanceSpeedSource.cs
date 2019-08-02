using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSpeedSource : MonoBehaviour
{
    public bool mouseTest = false; 
                                       //check this in the Unity editor to use mouse Y
                                       //position instead of headset data
    public PeopleMover peopleMover;
                                       //all speed sources must report speed to
                                       //PeopleMover script
    public GameObject head;
                                       //data is gathered from the Y position of
                                       //this object, which should correspond to
                                       //oculus player's head/headset
    public float buffer_size = 1f;
                                       //how many seconds of data should be used
                                       //in the running distance sum
    public float speed_multiplier = 3f;
                                       //the equation is m*(d^x), this is m, distance
                                       //is d
    public float curve = 1.5f;
                                       //and this is x
    public float threshhold = 1f;
                                       //if distance falls below this threshhold,
                                       //set it to 0
    public UnityEngine.UI.Text multiplier_text;
                                       //this will be updated to the multiplier
                                       //for testing/debugging
    public UnityEngine.UI.Text exponent_text;
                                       //same for the exponent


    private Queue<float> running_sum = new Queue<float>();
    private Queue<float> samples_times = new Queue<float>();
    private float index_time = 0f;
    private int last_index;
    private int active_index;
    private float last_sum = 0f;
    private float last_sample = 0f;

    // Update is called once per frame
    void Update()
    {
        //update display info for debug/testing
        if (Time.frameCount%100 == 1)
        {
            multiplier_text.text = "multiplier: " + speed_multiplier.ToString("F1");
            exponent_text.text = "exponent: " + curve.ToString("F1");
        }


        //collect input for modifying multiplier and exponent
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) || Input.GetKeyDown(KeyCode.A))
        {
            speed_multiplier += 0.1f;
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger) || Input.GetKeyDown(KeyCode.S))
        {
            speed_multiplier -= 0.1f;
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.D))
        {
            curve += 0.1f;
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger) || Input.GetKeyDown(KeyCode.F))
        {
            curve -= 0.1f;
        }


        //update running sum with headset data, and find speed using running sum
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

        //report to PeopleMover
        peopleMover.ReportVelocity(speed_multiplier * Mathf.Pow(distance_per_second, curve));
    }

    private float GetHeadsetY()
    {
        return head.transform.position.y;
    }
}
