using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    public FlyingEyeMovement[] flyingEyeControl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone"))
        {
            foreach (FlyingEyeMovement flyingEye in flyingEyeControl)
            {
                if (flyingEye != null)
                {
                    flyingEye.enabled = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone"))
        {
            foreach (FlyingEyeMovement flyingEye in flyingEyeControl)
            {
                if (flyingEye != null)
                {
                    flyingEye.enabled = true;
                }
            }
        }
    }
}
