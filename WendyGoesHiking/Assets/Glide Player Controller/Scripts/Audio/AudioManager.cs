using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    //PLEASE IGNORE THIS, IT IS ENTIRELY DESIGNED TO MANAGE AUDIO INTENRALLY.

    [HideInInspector] public List<AudioInstance> instances;
    public static AudioManager current;

    public void Init()
    {
        current = this;

        instances = new List<AudioInstance>();

        for (int i = 0; i < 4; i++)
        {
            GameObject m_obj;
            m_obj = new GameObject("AudioInstance");
            m_obj.transform.SetParent(GlideController.current.gameObject.transform);
            m_obj.AddComponent<AudioSource>();
            m_obj.AddComponent<AudioInstance>();
            m_obj.SetActive(false);
            instances.Add(m_obj.GetComponent<AudioInstance>());
            m_obj.transform.localPosition = Vector3.zero;
        }
    }

    public static void PlaySound(AudioClip sound)
    {
        for (int i = 0; i < current.instances.Count; i++)
        {
            if (!current.instances[i].gameObject.activeSelf)
            {
                current.instances[i].GetComponent<AudioSource>().clip = sound;
                current.instances[i].GetComponent<AudioSource>().volume = 1f;
                current.instances[i].GetComponent<AudioSource>().pitch = 1f;
                current.instances[i].gameObject.SetActive(true);
                current.instances[i].GetComponent<AudioSource>().Play();
                return;
            }
        }

        GameObject m_obj = new GameObject("AudioInstance");
        m_obj.transform.SetParent(GlideController.current.gameObject.transform);
        m_obj.AddComponent<AudioSource>();
        m_obj.AddComponent<AudioInstance>();
        current.instances.Add(m_obj.GetComponent<AudioInstance>());
        m_obj.GetComponent<AudioSource>().clip = sound;
        m_obj.GetComponent<AudioSource>().volume = 1f;
        m_obj.GetComponent<AudioSource>().pitch = 1f;
        m_obj.GetComponent<AudioSource>().Play();
        m_obj.transform.localPosition = Vector3.zero;
    }

    public static void PlaySound(AudioClip sound, float volume)
    {
        for (int i = 0; i < current.instances.Count; i++)
        {
            if (!current.instances[i].gameObject.activeSelf)
            {
                current.instances[i].GetComponent<AudioSource>().clip = sound;
                current.instances[i].GetComponent<AudioSource>().volume = volume;
                current.instances[i].GetComponent<AudioSource>().pitch = 1f;
                current.instances[i].gameObject.SetActive(true);
                current.instances[i].GetComponent<AudioSource>().Play();
                return;
            }
        }


        GameObject m_obj = new GameObject("AudioInstance");
        m_obj.transform.SetParent(GlideController.current.gameObject.transform);
        m_obj.AddComponent<AudioSource>();
        m_obj.AddComponent<AudioInstance>();
        current.instances.Add(m_obj.GetComponent<AudioInstance>());
        m_obj.GetComponent<AudioSource>().clip = sound;
        m_obj.GetComponent<AudioSource>().volume = volume;
        m_obj.GetComponent<AudioSource>().pitch = 1f;
        m_obj.GetComponent<AudioSource>().Play();
        m_obj.transform.localPosition = Vector3.zero;
    }

    public static void PlaySound(AudioClip sound, float volume, float pitch)
    {
        for (int i = 0; i < current.instances.Count; i++)
        {
            if (!current.instances[i].gameObject.activeSelf)
            {
                current.instances[i].GetComponent<AudioSource>().clip = sound;
                current.instances[i].GetComponent<AudioSource>().volume = volume;
                current.instances[i].GetComponent<AudioSource>().pitch = pitch;
                current.instances[i].gameObject.SetActive(true);
                current.instances[i].GetComponent<AudioSource>().Play();
                return;
            }
        }

        GameObject m_obj = new GameObject("AudioInstance");
        m_obj.transform.SetParent(GlideController.current.gameObject.transform);
        m_obj.AddComponent<AudioSource>();
        m_obj.AddComponent<AudioInstance>();
        current.instances.Add(m_obj.GetComponent<AudioInstance>());
        m_obj.GetComponent<AudioSource>().clip = sound;
        m_obj.GetComponent<AudioSource>().volume = volume;
        m_obj.GetComponent<AudioSource>().pitch = pitch;
        m_obj.GetComponent<AudioSource>().Play();
        m_obj.transform.localPosition = Vector3.zero;
    }
}
