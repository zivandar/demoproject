using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    private float speed = 4f;            // Player Speed
    private bool isRespawn = false;      // Respawn Control
    private int respawn = 0;             // Respawn Checkline Value
    private Rigidbody rb;                // Player Rigidbody
    private Animator anim;               // Player Animator
    private UIManager uiManager;         // UIManager Class
    private Paint paint = new Paint();   // Paint Class

    private void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>(); // Reference
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();             // Reference
        anim = GetComponent<Animator>();           // Reference
        GameManager.rankPlayer[10] = gameObject;  // Player Rank Config
        paint.PaintInit();                       // Paint Wall Config
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && GameManager.IsStart && !isRespawn)
            Controller();   //Player Controller

        else if (Input.GetMouseButton(0) && !GameManager.IsStart && GameManager.IsPaint && !isRespawn)
            paint.PaintController(uiManager.percentageBar); //Paint Controller
           
        if (Input.GetMouseButtonUp(0) && GameManager.IsStart)
        {
            anim.SetFloat("VelX", 0);                   // Idle Animation
            anim.SetFloat("VelY", 0);                  // Idle Animation
            rb.velocity = new Vector3(0f, 0f, 0f);    // Rigidbody Velocity Reset
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("ObstacleStatic"))
        {
            isRespawn = true;                    // Stop Player Controller
            anim.SetInteger("Control", 1);      // Hit Animation
            StartCoroutine(Respawn(1f));       // Respawn After 1 second
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("CheckLine"))
            respawn = col.gameObject.GetComponent<CheckLine>().respawn;      // Set Respawn By Checkline

        else if (col.gameObject.CompareTag("DeadZone"))
        {
            isRespawn = true;                   // Stop Player Controller
            anim.SetInteger("Control", 1);      // Hit Animation
            StartCoroutine(Respawn(0.2f));      // Respawn After 0.2 second
        }

        else if (col.gameObject.CompareTag("PaintWall"))
        {
            GameManager.IsStart = false;      // Game Start Off
            GameManager.IsPaint = true;       // Paint ON
            isRespawn = false;                // Stop Player Controller
            rb.isKinematic = true;            // Rigidbody is now turned off
            anim.SetFloat("VelX", 0);         // Idle Animation
            anim.SetFloat("VelY", 0);         // Idle Animation
            uiManager.OpenPaintPanel();       // Paint Percentage Panel       
            transform.position = new Vector3(0, 0.1f, col.gameObject.transform.position.z);     //Player Pos Set
            col.gameObject.GetComponent<BoxCollider>().enabled = false;   //Col Object Collider False
        }
    }
    private void Controller()
    {
        anim.SetInteger("Control", 0);      // Move Animation
        float mouseX = Mathf.Clamp((Input.mousePosition.x / Screen.width) * 2 - 1, -1.0F, 1.0F); // Scale the screen, adjust mouse position.x
        float mouseY = Mathf.Clamp((Input.mousePosition.y / Screen.width) * 2 - 1, -1.0F, 1.0F); // Scale the screen, adjust mouse position.y
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);  // Reset Player Rotation
        anim.SetFloat("VelX", mouseX);      // Player Blend Animation Settings
        anim.SetFloat("VelY", mouseY);      // Player Blend Animation Settings

        //Rigidbody and Move Player
        Vector3 targetVelocity = new Vector3(mouseX, 0, mouseY);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -10f, 10f);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -10f, 10f);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetInteger("Control", 0);  // Idle Animation
        anim.SetFloat("VelX", 0);      // Idle Animation
        anim.SetFloat("VelY", 0);     // Idle Animation

        // Find objects with checkline tag
        // If the checkline object is the same value as the respawn player object
        // Respawn according to the position of that object
        GameObject[] checkLine = GameObject.FindGameObjectsWithTag("CheckLine");
        foreach (GameObject checkLineRespawn in checkLine)
            if (respawn == checkLineRespawn.GetComponent<CheckLine>().respawn)
                transform.position = new Vector3(0f, 0.1f, checkLineRespawn.transform.position.z);

        isRespawn = false;  // Play Player Controller
    }

}
