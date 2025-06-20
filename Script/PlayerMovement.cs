using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool smoothMovement = true;
    [SerializeField] private float smoothTime = 0.1f;
    
    [Header("Input Settings")]
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    
    [Header("Movement Constraints")]
    [SerializeField] private bool constrainMovement = false;
    [SerializeField] private Vector2 minBounds = new Vector2(-10f, -10f);
    [SerializeField] private Vector2 maxBounds = new Vector2(10f, 10f);
    
    // Private variables
    private Vector2 currentVelocity;
    private Vector2 targetVelocity;
    private Rigidbody2D rb;
    
    // Public properties for settings system
    public float MoveSpeed 
    { 
        get => moveSpeed; 
        set => moveSpeed = Mathf.Max(0f, value); 
    }
    
    public bool SmoothMovement 
    { 
        get => smoothMovement; 
        set => smoothMovement = value; 
    }
    
    public float SmoothTime 
    { 
        get => smoothTime; 
        set => smoothTime = Mathf.Max(0.01f, value); 
    }
    
    // Key binding properties
    public KeyCode MoveUpKey { get => moveUpKey; set => moveUpKey = value; }
    public KeyCode MoveDownKey { get => moveDownKey; set => moveDownKey = value; }
    public KeyCode MoveLeftKey { get => moveLeftKey; set => moveLeftKey = value; }
    public KeyCode MoveRightKey { get => moveRightKey; set => moveRightKey = value; }
    
    void Start()
    {
        // Get or add Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Configure Rigidbody2D for top-down movement
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        HandleInput();
    }
    
    void FixedUpdate()
    {
        MovePlayer();
    }
    
    private void HandleInput()
    {
        Vector2 inputVector = Vector2.zero;
        
        // Check for input using configurable keys
        if (Input.GetKey(moveUpKey))
            inputVector.y += 1f;
        if (Input.GetKey(moveDownKey))
            inputVector.y -= 1f;
        if (Input.GetKey(moveLeftKey))
            inputVector.x -= 1f;
        if (Input.GetKey(moveRightKey))
            inputVector.x += 1f;
        
        // Normalize diagonal movement
        if (inputVector.magnitude > 1f)
            inputVector = inputVector.normalized;
        
        targetVelocity = inputVector * moveSpeed;
    }
    
    private void MovePlayer()
    {
        Vector2 velocity;
        
        if (smoothMovement)
        {
            // Smooth movement using SmoothDamp
            velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        }
        else
        {
            // Direct movement
            velocity = targetVelocity;
        }
        
        rb.velocity = velocity;
        
        // Apply movement constraints if enabled
        if (constrainMovement)
        {
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);
            transform.position = clampedPosition;
        }
    }
    
    // Method to set key bindings (useful for settings menu)
    public void SetKeyBinding(string action, KeyCode newKey)
    {
        switch (action.ToLower())
        {
            case "up":
                moveUpKey = newKey;
                break;
            case "down":
                moveDownKey = newKey;
                break;
            case "left":
                moveLeftKey = newKey;
                break;
            case "right":
                moveRightKey = newKey;
                break;
        }
    }
    
    // Method to get current key binding
    public KeyCode GetKeyBinding(string action)
    {
        switch (action.ToLower())
        {
            case "up": return moveUpKey;
            case "down": return moveDownKey;
            case "left": return moveLeftKey;
            case "right": return moveRightKey;
            default: return KeyCode.None;
        }
    }
    
    // Method to reset to default WASD
    public void ResetToDefaultKeys()
    {
        moveUpKey = KeyCode.W;
        moveDownKey = KeyCode.S;
        moveLeftKey = KeyCode.A;
        moveRightKey = KeyCode.D;
    }
    
    // Optional: Alternative input method using Input.GetAxis (if you prefer)
    private Vector2 GetAxisInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return new Vector2(horizontal, vertical);
    }
}
