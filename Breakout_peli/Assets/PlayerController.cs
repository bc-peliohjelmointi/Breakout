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
    public float speed = 1.0f;

    private float velocity = 0.0f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
    }

	// TODO: How to stop player at wall?

	private void OnTriggerEnter2D(Collider2D collision)
	{
        // stop when hits wall
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Player hit wall");
            velocity = 0.0f;
        }
	}

	private void OnCollisionEnter2D(Collision2D collision)
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

	private void Update()
	{
		AcceleratingMove();
	}

    void SimpleMove()
    {
		Vector2 move = Vector2.zero;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			move += new Vector2(-1.0f, 0.0f) * speed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			move += new Vector2(+1.0f, 0.0f) * speed * Time.deltaTime;
		}
		rb.MovePosition(rb.position + move);
	}

	void AcceleratingMove()
	{
        float horizontal = Input.GetAxis("Horizontal");
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
			velocity += slow;

			// If velocity has become slow enough or changed sign, stop completely
			bool changed = sign != GetSign(velocity);
			if (Mathf.Abs(velocity) < StopTreshold || changed)
			{
				velocity = 0.0f;
			}
		}
		else
		{
			velocity += horizontal * Acceleration;
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
		Vector2 newPos = pos += new Vector2(velocity, 0.0f) * Time.deltaTime;

		// Kinematic physics pit‰‰ laskea kiihtyvyys ja liikevoima itse
		rb.MovePosition(newPos);
	}
}

