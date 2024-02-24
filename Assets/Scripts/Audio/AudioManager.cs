using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    private List<EventInstance> eventInstances;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            eventInstances = new List<EventInstance>();
            DontDestroyOnLoad(gameObject);
        }
    }

    // For sounds that play instantly
    public void Play(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.start();
        eventInstance.release();
    }

    public void SetPlayOneShot(EventReference sound, Transform transform, string paramName, float paramVal)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.setParameterByName(paramName, paramVal);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.start();
        eventInstance.release();
    }

    // For sounds that may rely on conditions before playing
    public EventInstance CreateEventInstance(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, transform);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void SetParameter(EventInstance eventInstance, string paramName, float paramVal)
    {
        eventInstance.setParameterByName(paramName, paramVal);
    }

    // Management
    public void PauseAll()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.setPaused(true);
        }
    }

    public void UnpauseAll()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.setPaused(false);
        }
    }

    public void StopAll()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        if (eventInstances != null)
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
    }
}
