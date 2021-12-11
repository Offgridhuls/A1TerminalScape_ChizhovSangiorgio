using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float moveSpeed = 1.0f;

    private Rigidbody2D rb;

    public static PlayerController instance;

    public static bool canOpenDoor;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        canOpenDoor = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(inputX * moveSpeed, inputY * moveSpeed);
    }

}
