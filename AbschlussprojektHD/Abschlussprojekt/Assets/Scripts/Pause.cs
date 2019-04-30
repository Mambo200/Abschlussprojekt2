using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public PlayerEntity m_Player;

    [SerializeField]
    private GameObject m_BackGround;
    public GameObject BackGround { get { return m_BackGround; } }

    [SerializeField]
    private GameObject m_PauseEnter;
    public GameObject PauseEnter { get { return m_PauseEnter; } }

    [SerializeField]
    private GameObject m_Options;
    public GameObject Options { get { return m_Options; } }

    [Header("Specific Pause Enter Variables")]
    [SerializeField]
    private Button m_ExitButton;


    [Header("Option Variables")]
    [SerializeField]
    private Sound opt_Sound;
    [SerializeField]
    private CameraSpeedX opt_CameraSpeedX;
    [SerializeField]
    private CameraSpeedY opt_CameraSpeedY;
    [SerializeField]
    private CameraInvert opt_CameraInvert;
    /// <summary>
    /// Calls when pause menu shall open
    /// </summary>
	public void CallPauseEnter()
    {
        BackGround.SetActive(true);
        Options.SetActive(false);
        PauseEnter.SetActive(true);

        Button.ButtonClickedEvent clickEvent = new Button.ButtonClickedEvent();
        clickEvent.AddListener(() => GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CloseConnection(m_Player));
        m_ExitButton.onClick = clickEvent;
    }

    /// <summary>
    /// Calls when user opens option menu
    /// </summary>
    public void CallOptions()
    {
        PauseEnter.SetActive(false);
        Options.SetActive(true);
        opt_Sound.OnBecomeActive();
        opt_CameraSpeedX.OnBecomeActive();
        opt_CameraSpeedY.OnBecomeActive();
        opt_CameraInvert.OnBecomeActive();
    }

    public void PlayerResuming()
    {
        Options.SetActive(false);
        PauseEnter.SetActive(true);
    }

}
