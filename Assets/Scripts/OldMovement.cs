using UnityEngine;

public class OldMovement : MonoBehaviour
{
    private float _moveX; // reference for x movement
    private float _moveY; // reference for y movement
    private float _moveSpeed = 5.0f; // movement speed
    
    private Vector3 _moveDirection; // new vector3 for combining x,y

    private void Update()
    {
        // get the x-axis input
        _moveX = Input.GetAxis("Horizontal");
        // get the y-axis input
        _moveY = Input.GetAxis("Vertical");
        // store the input in a new vector3
        _moveDirection = new Vector3(_moveX, _moveY, 0);
        // move the position of the object based on the moveDirection * the moveSpeed
        // we need to multiply by Time.deltaTime to make it frame rate independent
        transform.position += _moveDirection * (_moveSpeed * Time.deltaTime);
    }
}
