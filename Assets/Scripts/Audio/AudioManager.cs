using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    private Dictionary<string, EventInstance> eventInstances;
    private int counter = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            eventInstances = new Dictionary<string, EventInstance>();
            DontDestroyOnLoad(gameObject);
        }
    }

    // For sounds that play instantly
    public void Play(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventInstances.Add(counter.ToString(), eventInstance);
        counter++;
        eventInstance.start();
    }

    public void PlaySingleton(string key, EventReference sound, Transform transform)
    {
        if (eventInstances.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventInstances.Add(key, eventInstance);
        eventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventInstances.Add(counter.ToString(), eventInstance);
        counter++;
        eventInstance.start();

        eventInstances.Remove(counter.ToString());
        counter--;
        eventInstance.release();
    }

    public void PlayOneShotSingleton(string key, EventReference sound, Transform transform)
    {
        if (eventInstances.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        eventInstances.Add(key, eventInstance);
        eventInstance.start();

        eventInstances.Remove(key);
        eventInstance.release();
    }

    public void SetPlay(EventReference sound, Transform transform, string paramName, float paramVal)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventInstances.Add(counter.ToString(), eventInstance);
        counter++;
        eventInstance.start();
    }

    public void SetPlaySingleton(string key, EventReference sound, Transform transform, string paramName, float paramVal)
    {
        if (eventInstances.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventInstances.Add(key, eventInstance);
        eventInstance.start();
    }

    public void SetPlayOneShot(EventReference sound, Transform transform, string paramName, float paramVal)
    {
        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventInstances.Add(counter.ToString(), eventInstance);
        counter++;
        eventInstance.start();

        eventInstances.Remove(counter.ToString());
        counter--;
        eventInstance.release();
    }

    public void SetPlayOneShotSingleton(string key, EventReference sound, Transform transform, string paramName, float paramVal)
    {
        if (eventInstances.ContainsKey(key)) return;

        EventInstance eventInstance = CreateEventInstance(sound, transform);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.setParameterByName(paramName, paramVal);

        eventInstances.Add(key, eventInstance);
        eventInstance.start();

        eventInstances.Remove(key);
        eventInstance.release();
    }

    // For sounds that may rely on conditions before playing
    public EventInstance CreateEventInstance(EventReference sound, Transform transform)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, transform);
        eventInstances.Add(counter.ToString(), eventInstance);
        counter++;
        return eventInstance;
    }

    public EventInstance CreateEventSingleton(string key, EventReference sound, Transform transform)
    {
        if (eventInstances.ContainsKey(key)) return eventInstances[key];

        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, transform);
        eventInstances.Add(key, eventInstance);
        return eventInstance;
    }

    public void SetParameter(EventInstance eventInstance, string paramName, float paramVal)
    {
        eventInstance.setParameterByName(paramName, paramVal);
    }

    public void SetParameter(string key, string paramName, float paramVal)
    {
        eventInstances[key].setParameterByName(paramName, paramVal);
    }

    // Management
    public void PauseAll()
    {
        foreach (EventInstance eventInstance in eventInstances.Values)
        {
            eventInstance.setPaused(true);
        }
    }

    public void UnpauseAll()
    {
        foreach (EventInstance eventInstance in eventInstances.Values)
        {
            eventInstance.setPaused(false);
        }
    }

    public void Stop(string key)
    {
        if (!eventInstances.ContainsKey(key)) return;
        eventInstances[key].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstances[key].release();
        eventInstances.Remove(key);
    }

    public void StopAll()
    {
        foreach (EventInstance eventInstance in eventInstances.Values)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
        eventInstances.Clear();
    }

    private void OnDestroy()
    {
        StopAll();
    }
}
