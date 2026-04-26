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
}
