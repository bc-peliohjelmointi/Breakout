using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This ball controller uses Dynamic physics
 * and Collision callbacks to do bouncing.
 * The material needs to have 
 * Friction: 0
 * Bounciness: 1
 * So that collisions dont get stuck
 * 
 * Need to limit the velocity, otherwise
 * physics will tunnel through objects
 */

public class DynamicBallController : MonoBehaviour
{
    public float startForce; // How fast the ball is launched
    public float blockForce; // How much force is added when a block is hit
    public float maxVelocity;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
        rb.AddForce(new Vector3(-startForce, startForce, 0.0f));
    }

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Block")
		{
			Destroy(coll.gameObject);
		}
        // Get contact and add force aligned to collision normal: aka opposite direction
        Vector2 n = coll.GetContact(0).normal * blockForce;
        rb.AddForce(new Vector3(n.x, n.y, 0.0f));

        if (rb.velocity.magnitude > maxVelocity)
        {
            float vx = Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity);
            float vy = Mathf.Clamp(rb.velocity.y, -maxVelocity, maxVelocity);
            rb.velocity = new Vector2(vx, vy);
        }
	}

	// Update is called once per frame
	void Update()
    {
    }
}
