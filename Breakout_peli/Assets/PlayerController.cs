using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Acceleration = 500.0f;
    public float Slowdown = 800.0f;
    public float MaxVelocity = 100.0f;
    public float StopTreshold = 0.1f;

    public float DynamicMultiplier = 1.0f;
    public float DynamicSlowDown = 1.3f;

    bool Dynamic = true;

    private float velocity = 0.0f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        // stop when hits wall
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Player hit wall");
            velocity = 0.0f;
        }
	}

	float GetSign(float v)
    {
        if (v < 0.0f)
        {
            return -1.0f;
        }
        else
        {
            return 1.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Kinematic physics 
        // Velocity = CurrentVelocity + Acceleration
        // Pos = currentPos + Velocity
		float dt = Time.fixedDeltaTime;

        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            float horizontal = Input.GetAxis("Horizontal") * Acceleration * dt;
            if (horizontal == 0.0f && Mathf.Abs(velocity) > StopTreshold)
            {
                // No input, slow down
                float slow = Slowdown;
                float sign = GetSign(velocity);
                // If going right, slow down is negative
                if (velocity > 0.0f)
                {
                    slow = -Slowdown;
                }
                velocity += slow * dt;

                // If velocity has become slow enough or changed sign, stop completely
                bool changed = sign != GetSign(velocity);
                if (Mathf.Abs(velocity) < StopTreshold || changed)
                {
                    velocity = 0.0f;
                }
            }
            else
            {
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
            }

            Vector2 pos = rb.position; // get current position
            Vector2 newPos = pos += new Vector2(velocity, 0.0f) * dt;

            // Kinematic physics pitää laskea kiihtyvyys ja liikevoima itse
            rb.MovePosition(newPos);
        }
        else if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            float horizontal = Input.GetAxis("Horizontal");
            // Dynamic physics hoitaa kiihtyvyyden ja velocityn
            // automaattisesti
            if (horizontal != 0.0f)
            {
                rb.AddForce(new Vector2(horizontal * DynamicMultiplier * dt, 0.0f)); // Dynamic
            }
            else
            {
                // Add force to opposite direction until reaches StopTreshold
                if (Mathf.Abs(rb.velocity.x) > StopTreshold)
                {
					float sign = GetSign(rb.velocity.x);
                    rb.AddForce(new Vector2(-sign * DynamicSlowDown * dt, 0.0f)); // Dynamic
                }
                else
                {
                    // Stop
                    rb.velocity = new Vector2(0.0f, 0.0f);    
                }
            }
            // limit max velocity
            if (rb.velocity.magnitude > MaxVelocity)
            {
                rb.velocity = rb.velocity.normalized * MaxVelocity;
            }
        }
    }
}
