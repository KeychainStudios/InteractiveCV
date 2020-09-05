using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [Header("References")]
    public GameObject m_prefab;

    [Header("Parameters")]
    public int m_countPoints;

    Transform[] m_points;

    void Awake()
    {
        m_points = new Transform[m_countPoints];
        for (int i = 0; i < m_countPoints; i++)
        {
            GameObject pointObject = Instantiate(m_prefab, transform);
            pointObject.transform.localPosition = Vector3.zero;
            pointObject.SetActive(false);
            m_points[i] = pointObject.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBallTrajectory(Vector2 initPos, Vector2 speed, float gravity)
    {
        float stepTime = 0.05f;
        for (int i = 0; i < m_points.Length; i++)
        {
            float time = stepTime * i;
            float xPos = initPos.x + speed.x * time;
            float yPos = initPos.y + speed.y * time + gravity * time * time * 0.5f;
            m_points[i].position = new Vector3(xPos, yPos, 0);
        }
    }

    public void EnablePoints(bool enable)
    {
        for (int i = 0; i < m_points.Length; i++)
        {
            m_points[i].gameObject.SetActive(enable);
        }
    }
}
