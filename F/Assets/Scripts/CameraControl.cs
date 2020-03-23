using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
    public float size = 8.89f;
    [HideInInspector] public Transform m_Targets; // All the targets the camera needs to encompass.

    private Camera m_Camera;                        // Used for referencing the camera.
    private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 m_DesiredPosition;              // The position the camera is moving towards.
    
    public GameObject Origin;

    private void Awake ()
    {
        m_Camera = GetComponentInChildren<Camera> ();
    }

    private void FixedUpdate ()
    {
        Move ();
        Zoom ();
    }

    public void Move ()
    {
        FindAveragePosition ();
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    public void FindAveragePosition ()
    {
        Vector3 averagePos = new Vector3 ();

        averagePos += m_Targets.position;

        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos;
    }


    public void Zoom ()
    {
        m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, size, ref m_ZoomSpeed, m_DampTime);
    }



    public void SetStartPositionAndSize ()
    {
        m_Targets = Origin.transform;
        m_DesiredPosition = new Vector3(0.0f,0.0f,0.0f);
        size = 8.89f;
    }
}