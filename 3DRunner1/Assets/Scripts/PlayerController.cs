using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    private float laneDistance = 1.7f;
    private float laneSwitchSpeed = 10f;

    private int currentLane = 1;
    private Vector3 targetPosition;
    private bool isJumping = false;

    private Vector2 startTouchPosition;
    private bool stopTouch = false;
    private float swipeRange = 50f;

    private Animator anim;
    private Rigidbody rb;

    private Coroutine turnCoroutine;
    public float turnAngle = 50f;
    public float turnDuration = 1f;

    AudioManager audioManager;
    // Find the AudioManager component in the scene using its tag
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        anim = GetComponent<Animator>();
    }
    // Handles forward movement and smooth lane switching
    void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.fixedDeltaTime;

        Vector3 desiredPosition = rb.position.z * Vector3.forward + rb.position.y * Vector3.up;
        switch (currentLane)
        {
            case 0:
                desiredPosition += Vector3.left * laneDistance;
                break;
            case 2:
                desiredPosition += Vector3.right * laneDistance;
                break;
        }

        Vector3 laneMove = Vector3.Lerp(rb.position, desiredPosition, laneSwitchSpeed * Time.fixedDeltaTime);
        Vector3 targetMove = new Vector3(laneMove.x, rb.position.y, rb.position.z) + forwardMove;
        rb.MovePosition(targetMove);
    }
    // Check for swipe input in runtime
    void Update()
    {
        if (Time.timeScale > 0f)
        {
            DetectSwipe();
        }
    }
    //Method for detect swipe on screen
    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    stopTouch = false;
                    break;

                case TouchPhase.Moved:
                    if (!stopTouch)
                    {
                        Vector2 currentPosition = touch.position;
                        Vector2 distance = currentPosition - startTouchPosition;

                        if (distance.magnitude > swipeRange)
                        {
                            float x = distance.x;
                            float y = distance.y;

                            if (Mathf.Abs(x) > Mathf.Abs(y))
                            {
                                if (x > 0)
                                {
                                    SwipeRight();
                                }
                                else
                                {
                                    SwipeLeft();
                                }
                            }
                            else
                            {
                                if (y > 0)
                                {
                                    SwipeUp();
                                }
                            }

                            stopTouch = true;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    stopTouch = false;
                    break;
            }
        }
    }
    void SwipeRight()
    {
        if (currentLane < 2)
        {
            currentLane++;
            if (turnCoroutine != null) StopCoroutine(turnCoroutine);
            turnCoroutine = StartCoroutine(TurnRoutine(turnAngle));
        }
    }
    void SwipeLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
            if (turnCoroutine != null) StopCoroutine(turnCoroutine);
            turnCoroutine = StartCoroutine(TurnRoutine(-turnAngle));
        }
    }
    void SwipeUp()
    {
        if (!isJumping)
        {
            isJumping = true;
            anim.SetTrigger("Jumptrigger");
            rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);
        }
    }
    //Method for smooth turn when swipe left/right
    private IEnumerator TurnRoutine(float angle)
    {
        Quaternion startRot = transform.rotation;
        Quaternion turnRot = startRot * Quaternion.Euler(0f, angle, 0f);

        float turnTime = 0f;
        while (turnTime < turnDuration)
        {
            turnTime += Time.deltaTime;
            float t = turnTime / turnDuration;
            transform.rotation = Quaternion.Slerp(startRot, turnRot, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f); 
        Quaternion returnRot = transform.rotation;
        Quaternion finalRot = Quaternion.Euler(0f, 0f, 0f);
        float returnTime = 0f;
        while (returnTime < turnDuration)
        {
            returnTime += Time.deltaTime;
            float t = returnTime / turnDuration;
            transform.rotation = Quaternion.Slerp(returnRot, finalRot, t);
            yield return null;
        }
        transform.rotation = finalRot;
    }
    // Checking if player is grounded
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    //Method for play audio and show menu when player hit finish or obstacle colliders
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag ("Finish"))
        {
            UIManager.Instance.ShowCollapseMenu();
            if (other.CompareTag("Obstacle"))
            {
                audioManager.PlaySFX(audioManager.Lose);
            }
            else
            {
                audioManager.PlaySFX(audioManager.Finish);
            }
            
        }
    }
}
