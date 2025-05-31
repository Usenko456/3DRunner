using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    private float laneDistance = 2f; // відстань між "смугами" руху вліво-вправо
    private float laneSwitchSpeed = 10f;

    private int currentLane = 1; // 0 - ліво, 1 - центр, 2 - право
    private Vector3 targetPosition;
    private bool isJumping = false;

    // Для розпізнавання свайпів
    private Vector2 startTouchPosition;
    private bool stopTouch = false;
    private float swipeRange = 50f; // мінімальна відстань свайпу
    private float tapRange = 10f;

    private Rigidbody rb;

    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
    }

    void Update()
    {
        // Рух вперед постійний
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;
        transform.position += forwardMove;

        // Обробка свайпів
        DetectSwipe();

        // Обробка руху вліво/вправо
        Vector3 desiredPosition = transform.position.z * Vector3.forward + transform.position.y * Vector3.up;

        switch (currentLane)
        {
            case 0:
                desiredPosition += Vector3.left * laneDistance;
                break;
            case 2:
                desiredPosition += Vector3.right * laneDistance;
                break;
        }

        // Плавне переміщення в бік
        Vector3 moveVector = Vector3.Lerp(transform.position, desiredPosition, laneSwitchSpeed * Time.deltaTime);
        transform.position = new Vector3(moveVector.x, transform.position.y, transform.position.z);
    }

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
                                // Горизонтальний свайп
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
                                // Вертикальний свайп
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
            
        }
    }

    void SwipeLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
        }
    }

    void SwipeUp()
    {
        if (!isJumping)
        {
            isJumping = true;
       
            rb.AddForce(Vector3.up * 7f, ForceMode.Impulse); // сила стрибка - підлаштуй
        }
    }

    // Метод, який викликає анімація при приземленні (Animation Event)
    public void OnLanding()
    {
        isJumping = false;
    }
}
