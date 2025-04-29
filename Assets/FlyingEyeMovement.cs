using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeMovement : MonoBehaviour
{

    public float moveDistance = 5.0f;
    public float moveSpeed = 8.0f;

    private Vector3 startPos;
    public int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPos + Vector3.right * offset * direction;
    }
}
