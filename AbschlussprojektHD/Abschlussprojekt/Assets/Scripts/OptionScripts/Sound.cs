using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Sound : MonoBehaviour {
#pragma warning disable 0649
    [SerializeField]
    private Slider m_Slider;
#pragma warning restore

    [SerializeField]
    private Text m_Text;
#pragma warning disable 0649
    [SerializeField]
    private AudioSource m_Audio;
#pragma warning restore

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
