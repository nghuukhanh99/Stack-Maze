using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public GameObject StartButton;

    public GameObject NextRoundButton;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Time.timeScale = 0;  
    }


    void Update()
    {
        StartCoroutine(TurnOnNextRoundButton());
    }

    public void startGame()
    {
        Time.timeScale = 1;
        StartButton.gameObject.SetActive(false);
    }

    public void NextRound()
    {
        NextRoundButton.gameObject.SetActive(false);

        SceneManager.LoadScene("Scene2");

    }
    
    IEnumerator TurnOnNextRoundButton()
    {
        

        if (PlayerCtrlTest.Instance.isNextRound == true)
        {
            yield return new WaitForSeconds(2.5f);

            NextRoundButton.gameObject.SetActive(true);

            Time.timeScale = 0;
        }
    }


}
