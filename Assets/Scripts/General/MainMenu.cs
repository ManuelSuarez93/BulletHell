 using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToLevel(string level) => SceneManager.LoadScene(level);
    public void ResetHighscore() => PlayerPrefs.SetFloat("Highscore",0);
    public void ExitGame() => Application.Quit();
}
