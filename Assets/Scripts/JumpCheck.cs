using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    public float jumpHeight = 0.1f; //how high must players jump
    public AudioSource yay;
    public AudioSource yay2;
    public AudioSource boo;

    private float playerHeight = 1.675f;

    public void SetPlayerHeght(float height)
    { //called from heightcheck
        playerHeight = height;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Eye"))
        {
            if (collision.gameObject.transform.localPosition.y > (playerHeight + jumpHeight))
            { //when a player passes through here, if the height is sufficiently
              //higher than the height reported by height check, success
                yay.Play();
                Debug.Log("yay");
                Invoke("StopSound", 0.25f);
            }
            else
            { //otherwise, failure
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
