using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IGlobalLogListener
{
    void GetNewLog();
}

public static class GameLogger 
{
    public static List<(long tick, string message)> log;
    static List<IGlobalLogListener> listeners;

    static bool initialized = false;

    public static void Initialize()
    {
        log = new List<(long, string)>();
        listeners = new List<IGlobalLogListener>();
        initialized = true;
    }

    public static void Log(string text)
    {
        if (initialized == false)
            Initialize();

        long ticks = GameObject.Find("GameData").GetComponent<GameData>().global_ticks;
        log.Add((ticks, text));

        foreach(IGlobalLogListener l in listeners)
            l.GetNewLog();
    }

    public static void AddListener(IGlobalLogListener listener)
    {
        if (initialized == false)
            Initialize();
            
        listeners.Add(listener);
        listener.GetNewLog();
    }
}
