using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//goes on a collider just before a jump
public class HeightCheck : MonoBehaviour
{
    public JumpCheck jumpCheck;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Eye"))
        {
            //when the player passes through here, not the height
            //and report it to jump check
            jumpCheck.SetPlayerHeght(collision.gameObject.transform.localPosition.y);
        }
    }
}
