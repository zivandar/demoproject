using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] private int agentNumber;           // AI Number
    public string agentName;                            // AI Name
    [SerializeField] private GameObject nameText;       // AI TextMeshPro
    private int respawn;                                // AI Respawn Checkline Value
    private Vector3 Finish;                             // Finishline
    private Animator anim;                              // AI animator
    private NavMeshAgent agent;                         // AI Navmesh

    private void Awake()
    {
        Finish = new Vector3(0f, 0.1f, GameObject.FindGameObjectWithTag("Finish").GetComponent<Transform>().position.z); //Reference
    }
    void Start()
    {
        GameManager.rankPlayer[agentNumber] = gameObject;           // Define agent number for sorting
        nameText.GetComponent<TextMeshPro>().text = agentName;      // Define agent name     
        agent = GetComponent<NavMeshAgent>();                       // Reference
        anim = GetComponent<Animator>();                            // Reference
        agent.speed = Random.Range(4.5f, 6.5f);

    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("ObstacleStatic"))
        {
            anim.SetInteger("Control", 2);              // Hit Animation
            agent.enabled = false;                      // Stop AI
            StartCoroutine(Respawn(1f));                // Respawn After 1 second
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("CheckLine"))
            respawn = col.gameObject.GetComponent<CheckLine>().respawn;     // Set Respawn By Checkline
        else if (col.gameObject.CompareTag("DeadZone"))
        {
            anim.SetInteger("Control", 2);              // Hit Animation        
            agent.enabled = false;                      // Stop AI       
            StartCoroutine(Respawn(0.2f));              // Respawn After 0.2 second
        }
        else if (col.gameObject.CompareTag("Finish"))
        {
            gameObject.tag = "Untagged";                                  // Tag Untagged
            agent.enabled = false;                                       // Navmesh false
            gameObject.GetComponent<CapsuleCollider>().enabled = false; // Collider False
            GameManager.finishAIRank += 1;                             // Rank
            transform.position = new Vector3(Random.Range(-3.5f, 3.5f), 0.01f, transform.position.z); // Set Pos
            anim.SetInteger("Control", 0);         // Idle Anim
        }
    }
    void FixedUpdate()
    {
        if (GameManager.IsStart && agent.enabled)
        {
            agent.SetDestination(Finish);      // Set Pos
            anim.SetInteger("Control", 1);    // Run Animation
        }
    }

    IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);   
        GameObject[] checkLine = GameObject.FindGameObjectsWithTag("CheckLine");    // Find objects with checkline tag
        foreach (GameObject checkLineRespawn in checkLine)                         // If the checkline object is the same value as the respawn player object
            if (respawn == checkLineRespawn.GetComponent<CheckLine>().respawn)    // Respawn according to the position of that object
                transform.position = new Vector3(Random.Range(-3.1f, 3.1f), 0.1f, checkLineRespawn.transform.position.z);
        agent.enabled = true; // AI Move
    }

}
