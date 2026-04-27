using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float forse = 15f;
            landerRigidbody2D.AddForce(forse * transform.up);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = 1.5f;
            landerRigidbody2D.AddTorque(turnSpeed);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -1.5f;
            landerRigidbody2D.AddTorque(turnSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        float softLandingVelocityMagnitude = 3f;
        if (collision2D.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            // Landed too hard!
            Debug.Log("Landed too hard!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.9f;
        if (dotVector < minDotVector)
        {
            // Landed on a too steep angle!
            Debug.Log("Landed on a too steep angle!");
            return;
        }

        Debug.Log("Succesful landing!");
    }

}
