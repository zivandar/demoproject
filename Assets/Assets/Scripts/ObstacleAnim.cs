using UnityEngine;
public class ObstacleAnim : MonoBehaviour
{
    [Header("CURRENT CONF")]
    [SerializeField] private float animSpeed;   
    [Space(10)]
    [Header("HORIZONTAL OBSTACLE")]
    [SerializeField] private bool horizAnim;
    [SerializeField] private bool vertY;
    [SerializeField] private float horizNextPosX;
    [SerializeField] private float vertNextPosY;
    private Vector3 currentPos;
    [Space(10)]
    [Header("ROTATE OBSTACLE")]
    [SerializeField] private bool rotateAnim;
    [SerializeField] private bool left;
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis;
    [SerializeField] private bool zAxis;

    private void Start()
    {
        currentPos = transform.position;
    }
    private void LateUpdate()
    {
        //Horiz
        if (horizAnim)
            Move();
        //Rotate
        else if (rotateAnim)
            Rotate();

    }
    //Horiz
    private void Move()
    {
        //Ping Pong
        float pingPong = Mathf.PingPong(Time.time * animSpeed, 1);
        if (!vertY)
        {
            transform.position = Vector3.Lerp(
                new Vector3(currentPos.x, transform.position.y, transform.position.z),
                new Vector3(horizNextPosX, transform.position.y, transform.position.z),
                pingPong);
        }
        else
        {
            transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, currentPos.y, transform.position.z),
                new Vector3(transform.position.x, vertNextPosY, transform.position.z),
                pingPong);
        }

    }
    //Rotation
    private void Rotate()
    {
        //X Y Z rotate axis
        if (xAxis)
            if (left)
                transform.rotation = Quaternion.Euler(360 * Mathf.Sin(Time.time * animSpeed), 0f, 0f);
            else
                transform.rotation = Quaternion.Euler(-360 * Mathf.Sin(Time.time * animSpeed), 0f, 0f);

        else if (yAxis)
            if (left)
                transform.rotation = Quaternion.Euler(0f, 360 * Mathf.Sin(Time.time * animSpeed), 0f);
            else
                transform.rotation = Quaternion.Euler(0f, -360 * Mathf.Sin(Time.time * animSpeed), 0f);
        else if (zAxis)
            if (left)
                transform.rotation = Quaternion.Euler(0f, 0f, 360 * Mathf.Sin(Time.time * animSpeed));
            else
                transform.rotation = Quaternion.Euler(0f, 0f, -360 * Mathf.Sin(Time.time * animSpeed));
    }
}
