using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;

    [SerializeField] private float sprintMultiplier;

    [SerializeField] private float jumpHeight;

    private Rigidbody2D rb;

    private bool isGrounded = false;

    public Transform respawnPoint;

    private bool canMove = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("Respawn");
    }

    // Update is called once per frame
    private void Update()
    {
        if (canMove)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            if (horizontalInput != 0)
            {
                transform.position += Input.GetButton("Sprint") && isGrounded
                    ? new Vector3(horizontalInput * speed * sprintMultiplier * Time.deltaTime, 0, 0)
                    : new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpHeight));
                isGrounded = false;
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.3f);
        transform.position = respawnPoint.position;
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Killbox"))
        {
            StartCoroutine("Respawn");
        }
    }
}
