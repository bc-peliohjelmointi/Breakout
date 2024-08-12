using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Acceleration = 500.0f;
    public float MaxVelocity = 100.0f;

    bool Dynamic = true;

    private float velocity = 0.0f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Kinematic physics 
        // Velocity = CurrentVelocity + Acceleration
        // Pos = currentPos + Velocity

        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            float dt = Time.fixedDeltaTime;
            float horizontal = Input.GetAxis("Horizontal") * Acceleration * dt;
            velocity += horizontal * dt;
            // Limit velocity
            // 1: Clamp
            velocity = Mathf.Clamp(velocity, -MaxVelocity, MaxVelocity);

            // 2 Clamp manuaalisesti
            if (velocity < -MaxVelocity)
            {
                velocity = -MaxVelocity;
            }
            else if (velocity > MaxVelocity)
            {
                velocity = MaxVelocity;
            }

            Vector2 pos = rb.position; // get current position
            Vector2 newPos = pos += new Vector2(velocity, 0.0f) * dt;

            // Kinematic physics pit‰‰ laskea kiihtyvyys ja liikevoima itse
            rb.MovePosition(newPos);
        }
        else if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            float horizontal = Input.GetAxis("Horizontal");
            // Dynamic physics hoitaa kiihtyvyyden ja velocityn
            // automaattisesti
            rb.AddForce(new Vector2(horizontal, 0.0f)); // Dynamic
        }
    }
}
