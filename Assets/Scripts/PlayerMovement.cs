using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   private PlayerInput _playerInput; // stores the PlayerInput instance
   private Vector2 _moveInput; // stores movement input values
   private Vector2 _screenBounds;
   private float _moveSpeed = 5.0f; // stores the movement speed value
   private float _maxSpeed = 10.0f;
   
   private Rigidbody2D _rb;

   private void OnEnable()
   {
      _playerInput.Player.Enable();
   }
   private void OnDisable()
   {
      _playerInput.Player.Disable();
   }
   private void Awake()
   {
      _playerInput = new PlayerInput(); // create a new instance of an object

      _playerInput.Player.Move.performed += OnMovePerformed; // subscribe to the OnMovePerformed function
      _playerInput.Player.Move.canceled += OnMoveCanceled; // subscribe to the OnMoveCanceled function
   }

   private void Start()
   {
      _rb = GetComponent<Rigidbody2D>();
      _screenBounds = GetScreenBounds(Camera.main);
   }

   private void OnMovePerformed(InputAction.CallbackContext context)
   {
      _moveInput = context.ReadValue<Vector2>();
   }
   private void OnMoveCanceled(InputAction.CallbackContext context)
   {
      _moveInput = Vector2.zero;
   }

   private void Update()
   {
      // HandleMovement();
      // ClampToScreen();
      WarpAcrossScreen();
   }

   private void FixedUpdate()
   {
      HandlePhysicsMovement();
      VelocityLimiter();
   }

   private void HandleMovement()
   {
      transform.position += (Vector3)_moveInput * (_moveSpeed * Time.deltaTime);
   }

   private void HandlePhysicsMovement()
   {
      // _rb.AddForce(_moveInput, ForceMode2D.Impulse);
      _rb.AddForce(_moveInput * _moveSpeed, ForceMode2D.Force);
      // _rb.linearVelocity = _moveInput * _moveSpeed;
   }

   private Vector2 GetScreenBounds(Camera cam)
   {
      Vector3 screenTopRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
      return new Vector2(screenTopRight.x, screenTopRight.y);
   }

   private void ClampToScreen()
   {
      Vector2 position = transform.position;
      float halfShipWidth = transform.localScale.x / 2.0f;
      float halfShipHeight = transform.localScale.y / 2.0f;
      
      position.x = Mathf.Clamp(position.x, -_screenBounds.x + halfShipWidth, _screenBounds.x - halfShipWidth);
      position.y = Mathf.Clamp(position.y, -_screenBounds.y + halfShipHeight, _screenBounds.y - halfShipHeight);
      
      transform.position = position;
   }

   private void WarpAcrossScreen()
   {
      Vector2 position = transform.position;
      
      // Horizontal Warp
      if (position.x > _screenBounds.x)
      {
         position.x = -_screenBounds.x;
      }
      else if (position.x < -_screenBounds.x)
      {
         position.x = _screenBounds.x;
      }
      
      // Vertical Warp
      if (position.y > _screenBounds.y)
      {
         position.y = -_screenBounds.y;
      }
      else if (position.y < -_screenBounds.y)
      {
         position.y = _screenBounds.y;
      }
      
      transform.position = position;
   }

   private void VelocityLimiter()
   {
      if (_rb.linearVelocity.magnitude > _maxSpeed)
      {
         _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;
      }
   }
}
