using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    private List<EventInstance> eventInstances;
    private Dictionary<string, EventInstance> eventSingletons;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            eventInstances = new List<EventInstance>();
            eventSingletons = new Dictionary<string, EventInstance>();
            DontDestroyOnLoad(gameObject);
        }
    }

    // For sounds that play instantly
    public void Play(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventInstances.Add(eventInstance);
        eventInstance.start();
    }

    public void PlaySingleton(string key, EventReference sound, Transform transform)
    {
        if (eventSingletons.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventSingletons.Add(key, eventInstance);
        eventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventInstances.Add(eventInstance);
        eventInstance.start();

        eventInstances.Remove(eventInstance);
        eventInstance.release();
    }

    public void PlayOneShotSingleton(string key, EventReference sound, Transform transform)
    {
        if (eventSingletons.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventSingletons.Add(key, eventInstance);
        eventInstance.start();

        eventSingletons.Remove(key);
        eventInstance.release();
    }

    public void SetPlay(EventReference sound, Transform transform, string paramName, float paramVal)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventInstances.Add(eventInstance);
        eventInstance.start();
    }

    public void SetPlaySingleton(string key, EventReference sound, Transform transform, string paramName, float paramVal)
    {
        if (eventSingletons.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventSingletons.Add(key, eventInstance);
        eventInstance.start();
    }

    public void SetPlayOneShot(EventReference sound, Transform transform, string paramName, float paramVal)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventInstances.Add(eventInstance);
        eventInstance.start();

        eventInstances.Remove(eventInstance);
        eventInstance.release();
    }

    public void SetPlayOneShotSingleton(string key, EventReference sound, Transform transform, string paramName, float paramVal)
    {
        if (eventSingletons.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventSingletons.Add(key, eventInstance);
        eventInstance.start();

        eventSingletons.Remove(key);
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

    public EventInstance CreateEventSingleton(string key, EventReference sound, Transform transform)
    {
        if (eventSingletons.ContainsKey(key)) return eventSingletons[key];

        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, transform);
        eventSingletons.Add(key, eventInstance);
        return eventInstance;
    }

    public void SetParameter(EventInstance eventInstance, string paramName, float paramVal)
    {
        eventInstance.setParameterByName(paramName, paramVal);
    }

    public void SetParameterSingleton(string key, string paramName, float paramVal)
    {
        if (!eventSingletons.ContainsKey(key)) return;
        eventSingletons[key].setParameterByName(paramName, paramVal);
    }

    // Management
    public void PauseAll()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.setPaused(true);
        }

        foreach (EventInstance eventInstance in eventSingletons.Values)
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

        foreach (EventInstance eventInstance in eventSingletons.Values)
        {
            eventInstance.setPaused(false);
        }
    }

    public void StopSingleton(string key)
    {
        if (!eventSingletons.ContainsKey(key)) return;
        eventSingletons[key].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventSingletons[key].release();
        eventSingletons.Remove(key);
    }

    public void StopAll()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
        eventInstances.Clear();

        foreach (EventInstance eventInstance in eventSingletons.Values)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
        eventSingletons.Clear();
    }
}
