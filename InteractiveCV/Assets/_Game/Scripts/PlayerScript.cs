using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Parameters")]
    public float m_speed;
    public float m_jumpSpeed;
    public float m_throwForce;

    [Header("Ground Detection")]
    public float m_radius;
    public LayerMask m_mask;

    enum STATES
    {
        NORMAL,
        BALL_IN_HAND,
        LAUNCH_BALL
    }

    Camera m_camera;
    Transform m_ballHold;
    LineDrawer m_lineDrawer;
    Rigidbody2D m_ballBody;
    Transform m_initParentBall;
    STATES m_state;
    float m_timeToBeNormal = 0.5f;
    float m_timer;
    Vector2 m_speedBall;

    Rigidbody2D m_body;
    Animator m_animator;
    bool m_isInGround;
    bool m_justWarped;

    public bool JustWarped { get => m_justWarped; set => m_justWarped = value; }

    void Awake()
    {
        m_camera = Camera.main;
        m_ballHold = transform.Find(NamesManager.NAME_BALL_HOLD);
        Transform lineTransform = transform.Find(NamesManager.NAME_LINE_DRAWER);
        m_lineDrawer = lineTransform.GetComponent<LineDrawer>();
        GameObject ballObject = GameObject.FindGameObjectWithTag(NamesManager.TAG_BALL);
        m_ballBody = ballObject.GetComponent<Rigidbody2D>();
        m_initParentBall = ballObject.transform.parent;
        m_state = STATES.NORMAL;

        m_timer = 0;
        m_body = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_justWarped = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        m_isInGround = (Physics2D.OverlapCircle(transform.position, m_radius, m_mask) != null);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        float directionX = (Input.acceleration.x > 0) ? 1 : -1;
        float x = (Mathf.Abs(Input.acceleration.x) >= 0.12f) ? directionX : 0;
#else
        float x = Input.GetAxisRaw("Horizontal");
#endif
        float speedX = x * m_speed;
        transform.rotation = (speedX > 0) ? (Quaternion.Euler(0, 0, 0)) : (Quaternion.Euler(0, 180, 0));
        m_body.velocity = new Vector2(speedX, m_body.velocity.y);
        m_animator.SetFloat(NamesManager.ANIMATOR_FLOAT_SPEEDX, Mathf.Abs(speedX));

#if UNITY_ANDROID
        if (Input.GetMouseButtonDown(0) && m_state != STATES.BALL_IN_HAND)
#else
        if (Input.GetKeyDown(KeyCode.Space))
#endif
        {
            if (m_isInGround)
            {
                SoundManager.instance.PlaySound(SoundManager.ID_SOUND_JUMP);
                m_body.velocity = new Vector2(m_body.velocity.x, m_jumpSpeed);
            }
        }
#if UNITY_ANDROID
        if (Input.GetMouseButtonUp(0) && m_state != STATES.BALL_IN_HAND)
#else
        if (Input.GetKeyUp(KeyCode.Space))
#endif
        {
            float velY = m_body.velocity.y;
            if (velY > 0)
            {
                m_body.velocity = new Vector2(m_body.velocity.x, velY * 0.5f);
            }
        }

        switch (m_state)
        {
            case STATES.BALL_IN_HAND:
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 ballPos = (Vector2)m_ballHold.position;
                Vector2 direction = mousePos - ballPos;
                Vector2 speed = direction.normalized * m_throwForce;
                m_lineDrawer.ShowBallTrajectory(ballPos, speed, m_body.gravityScale * Physics2D.gravity.y);
                break;
            case STATES.LAUNCH_BALL:
                if (m_timer > 0)
                {
                    m_timer -= Time.deltaTime;
                    if (m_timer <= 0)
                    {
                        m_timer = 0;
                        m_state = STATES.NORMAL;
                    }
                }
                break;
        }

#if UNITY_ANDROID
        if (Input.GetMouseButtonUp(0))
#else
        if (Input.GetMouseButtonDown(0))
#endif
        {
            if (m_state == STATES.BALL_IN_HAND)
            {
                m_state = STATES.LAUNCH_BALL;
                m_ballBody.transform.parent = m_initParentBall;
                m_ballBody.simulated = true;

                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = mousePos - (Vector2)m_ballHold.position;

                m_speedBall = direction.normalized * m_throwForce;
                m_ballBody.velocity = m_speedBall;

                m_timer = m_timeToBeNormal;

                m_lineDrawer.EnablePoints(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(NamesManager.TAG_BALL))
        {
            if (m_state == STATES.NORMAL)
            {
                Debug.Log("Ball Detected");
                Transform ballParent = collision.transform.parent;
                ballParent.parent = m_ballHold;
                ballParent.localPosition = Vector3.zero;
                m_ballBody = ballParent.GetComponent<Rigidbody2D>();
                m_state = STATES.BALL_IN_HAND;
                m_ballBody.simulated = false;

                // Tutorial ball.
                EventManager.instance.CallActivateTutorial(true, 4);

                m_lineDrawer.EnablePoints(true);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, m_radius);
    }
}
