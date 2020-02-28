using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private CanvasRenderer panel;

    private Button startButton;
    private Button quitButton;

    /// <summary>
    /// Sets the panel to fade away when the player presses startButton
    /// </summary>
    public void FadeAway()
    {
        panel.SetAlpha(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Start()
    {
        panel.SetAlpha(1);
        Cursor.visible = true;
    }
}
