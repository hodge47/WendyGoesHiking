using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class UIButtonManager : MonoBehaviour
{
    [SerializeField]
    GameObject MenuUI;
    [SerializeField]
    GameObject Title;
    [SerializeField]
    GameObject Options;
    [SerializeField]
    GameObject Credits;

    [SerializeField]
    GlideController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        Debug.Log("Trying Start Button");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Player.enabled = true;
        MenuUI.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        Title.SetActive(false);
        Credits.SetActive(true);
    }
    public void OptionsButton()
    {
        Title.SetActive(false);
        Options.SetActive(true);
    }


    /// Return Buttons

    public void CreditsReturnButton()
    {
        Credits.SetActive(false);
        Title.SetActive(true);
    }
    public void OptionsReturnButton()
    {
        Options.SetActive(false);
        Title.SetActive(true);
    }

}
