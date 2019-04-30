using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Sound : MonoBehaviour {

    [SerializeField]
    private Slider m_Slider;

    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private AudioSource m_Audio;

    /// <summary>
    /// Set sound volume
    /// </summary>
    public void SetSound()
    {
        m_Audio.volume = m_Slider.value;
    }

    public void OnBecomeActive()
    {
        m_Slider.value = m_Audio.volume;
    }
}
