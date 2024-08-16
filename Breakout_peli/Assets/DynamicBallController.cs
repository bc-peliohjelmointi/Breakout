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
    public float playerHitMultiplier;
    public int ParticleLimit;

    // Signal to LevelCreator when destroyed
    public delegate void BallDestroyedDelegate();
    public event BallDestroyedDelegate BallDestroyedEvent;

    private Rigidbody2D rb;

    public GameObject particlePrefab;

    private List<GameObject> particles;

    private Vector2 startPosition;

    void Start()
    {
    }

    public void Launch()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
		rb.velocity = Vector2.zero;
		rb.AddForce(new Vector3(-startForce, startForce, 0.0f));
    }

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Block")
		{
			Destroy(coll.gameObject);
		}
        else if (coll.gameObject.tag == "KillArea")
        {
            // Destroy self?
            // Reset?

            BallDestroyedEvent();
        }
        else if (coll.gameObject.tag == "Player")
        {
            // Add player's velocity to own velocity
            rb.AddForce(coll.gameObject.GetComponent<Rigidbody2D>().velocity * playerHitMultiplier);
        }

        // Get contact and add force aligned to collision normal: aka opposite direction
        Vector2 n = coll.GetContact(0).normal * blockForce;
        rb.AddForce(new Vector3(n.x, n.y, 0.0f));

        // Rajoita pallon nopeus maxVelocity:yn
        if (rb.velocity.magnitude > maxVelocity)
        {
            // Tapa 1: Clamp x ja y
            float vx = Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity);
            float vy = Mathf.Clamp(rb.velocity.y, -maxVelocity, maxVelocity);
            rb.velocity = new Vector2(vx, vy);

            // Tapa 2: Kerro normaalivektori maksiminopeudella
            Vector2 limited = rb.velocity.normalized * maxVelocity;
            rb.velocity = limited;
        }
	}

	// Update is called once per frame
	void Update()
    {

    }
}
