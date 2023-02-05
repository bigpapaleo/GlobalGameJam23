using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RollaballSphereController : MonoBehaviour
{
    Rigidbody rb;
    bool hasJump;
    // Start is called before the first frame update
    void Start()
    {
        hasJump = true;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasJump = true;

        if (collision.transform.name == "Killbox")
        {
            transform.position = new Vector3(-12.56f, -12.12f, 20f);
        }
        else if (collision.transform.name == "win")
        { 
            GameManager.GotoScene("GameOver");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float zVel = 0;
        float xVel = 0;
        if (Input.GetKey(KeyCode.W)) zVel += 4;
        if (Input.GetKey(KeyCode.S)) zVel -= 4;
        if (Input.GetKey(KeyCode.D)) xVel += 4;
        if (Input.GetKey(KeyCode.A)) xVel -= 4;

        if (Input.GetKey(KeyCode.Space) && hasJump)
        {
            hasJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 10, rb.velocity.z);
        }

        rb.velocity = new Vector3(xVel, rb.velocity.y, zVel);
    }
}
