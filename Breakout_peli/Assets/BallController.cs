using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/* This ball controller uses Kinematic physics
 * and trigger detection to do collisions
 * 
 * If uses Square collision, the collisions are not that erratic
 * as with Circle collision
 * 
 */
public class BallController : MonoBehaviour
{
    Vector3 direction; // Where the ball is going
    public float speed = 1.0f; // How fast the ball is going

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction.x = 1.0f;
        direction.y = 1.0f;
        direction.Normalize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 posNow = rb.position;
        rb.MovePosition(posNow + direction * speed * Time.fixedDeltaTime);
    }
    
	private void OnTriggerEnter2D(Collider2D other)
	{
        // When ball hits something, find the closest point
        Vector2 hit = other.ClosestPoint(rb.position);
        // Calculate the vector from self to hit and normalize -> Where is the collision
        Vector2 normal = (hit - rb.position).normalized;

        float pushAmount = speed * Time.fixedDeltaTime;
        float normaLimit = 0.5f; // Limit what collisions are counted

        // Because of trigger, need to manually push out of collision
        if (Mathf.Abs(normal.x) > normaLimit) 
        {
            // On the right or left
            direction.x *= -1.0f;
            // Push away from point
            rb.MovePosition(rb.position + new Vector2(-normal.x * pushAmount, 0.0f));
        }
        if (Mathf.Abs(normal.y) > normaLimit)
        {
            // On the above or below
            direction.y *= -1.0f;
            rb.MovePosition(rb.position + new Vector2(-normal.y * pushAmount, 0.0f));
        }

		if (other.gameObject.tag == "Block") {
			// Destroy block
			Destroy(other.gameObject);
		}
		if (other.gameObject.tag == "Player") {
            // If player is moving the direction is affected???
		}
	}
}
