using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private save savescript;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        savescript.boidCount = (int)slider.value;

    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


}
