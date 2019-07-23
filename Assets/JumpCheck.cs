using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    public float jumpHeight = 0.1f;
    public AudioSource yay;
    public AudioSource yay2;
    public AudioSource boo;

    private float playerHeight = 1.675f;

    public void SetPlayerHeght(float height)
    {
        playerHeight = height;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Eye"))
        {
            if (collision.gameObject.transform.localPosition.y > (playerHeight + jumpHeight))
            {
                yay.Play();
                Debug.Log("yay");
                Invoke("StopSound", 0.25f);
            }
            else
            {
                boo.Play();
                Debug.Log("boo");
            }
        }
    }

    private void StopSound()
    {
        yay.Stop();
        yay2.Play();
        Invoke("StopTwo", 0.5f);
    }

    private void StopTwo()
    {
        yay2.Stop();
    }
}
