using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject rankPanel;
    [SerializeField] private GameObject paintPanel;
    public Image percentageBar;
    public Text[] rank = new Text[11];

    public void Play()
    {
        playButton.SetActive(false);
        GameManager.IsStart = true;
    }
    public void Restart()
    {
        Button clickButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        clickButton.interactable = false;
        SceneManager.LoadSceneAsync("InGame");
    }
    public void FinishGame()
    {
        restartButton.SetActive(true);
    }
    public void OpenPaintPanel()
    {
        rankPanel.SetActive(false);
        paintPanel.SetActive(true);
    }

}
