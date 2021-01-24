using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsStart;                                   // Game Start
    public static bool IsPaint;                                   // Player Paint
    public static GameObject[] rankPlayer = new GameObject[11];   // Air Or Player
    public static int finishAIRank;                               // Rank
    private UIManager uiManager;                                  // UIManager Class
    private void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>(); // Reference
        IsStart = false;
        IsPaint = false;
        finishAIRank = 0;
    }
    private void Update()
    {
        if (IsStart && !IsPaint)
            RankSystem();
    }
    private void RankSystem()
    {
        if(finishAIRank < 11)
        {
            int uiRankCount = finishAIRank;
            // We take the player and ai elements in z pos and list them in descending order
            foreach (GameObject rank in rankPlayer.OrderByDescending(rank => rank.transform.position.z))
            {
                if (rank.CompareTag("AI"))
                {
                    uiManager.rank[uiRankCount].text = rank.GetComponent<AI>().agentName; // We print the name ai into ui text      
                    uiRankCount += 1;       // We increased ui rank text by +1
                }
                else if (rank.CompareTag("Player"))
                {
                    uiManager.rank[uiRankCount].text = "ZIVANDAR";   // We print our name on ui text            
                    uiRankCount += 1;       // We increased ui rank text by +1
                }
            }
        }
        
    }
}
