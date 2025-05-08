// TargetManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TargetManager : MonoBehaviour
{
    public TMP_Text targetCounterText; // Assign your TMPro Text object here
    public string nextSceneName; // Scene to load when all targets are activated
    public SceneFade sceneFade;
    public GameObject BKPanel;
    public GameObject Enemy;
    public float WaitBetweenFade = 4f;
   

    private int totalTargets;
    private int activeTargets = 0;
    private bool gameStarted = false;

    void Start()
    {
        totalTargets = GameObject.FindGameObjectsWithTag("Target").Length;
        if (gameStarted){
            UpdateTargetCounter();
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        UpdateTargetCounter();
    }

    public void ActivateTarget()
    {
        if (!gameStarted) return;

        activeTargets++;
        UpdateTargetCounter();

        if (activeTargets >= totalTargets)
        {
                Enemy.SetActive(false);
            StartCoroutine(WaitAndLoadNextScene());
        }
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(WaitBetweenFade);
        LoadNextScene();
    }

    void UpdateTargetCounter()
    {
        BKPanel.SetActive(true);
        targetCounterText.text = "Quest: Activate all targets" + "\n" + "     - " + activeTargets + "/" + totalTargets + " active";
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            sceneFade.FadeToScene(nextSceneName);
        }
    }
}
