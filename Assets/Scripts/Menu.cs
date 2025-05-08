using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;



public class MainMenu : MonoBehaviour
{

    public SceneFade sceneFade;
    
    public void StartGame()
    {
        sceneFade.FadeToScene("TRANSITION (CHAPTER 0)");

    }

    public void Options()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

     public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.2f; // Increase the size by 20%
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
