using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string HorizontalAxisKey = "Horizontal";
    private const string VerticalAxisKey = "Vertical";

    [SerializeField]
    private float moveSpeed;

    private Rigidbody rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis(HorizontalAxisKey), 0, Input.GetAxis(VerticalAxisKey));
        rbody.MovePosition(transform.position + input * moveSpeed * Time.deltaTime);
    }
}
