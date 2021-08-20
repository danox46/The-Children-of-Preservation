using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * EVENT MANAGER
 * This class manages the emitting and the listening of custom events.
 * 
 */

namespace TigerForge
{
    public class EEventManger : MonoBehaviour
    {

        /// <summary>
        /// Start the listening of an Event with the given 'eventName'. 'listener' has to be then function name to call when this Event is received. 
        /// </summary>
        /// <param name="eventName">The name of the Event to listen to.</param>
        /// <param name="listener">The name of the function to call on event receiving.</param>
        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Start the listening of an Event with the given 'eventName'. 'listener' has to be then function name used in this Event. 
        /// </summary>
        /// <param name="eventName">The name of the Event.</param>
        /// <param name="listener">The name of the used function.</param>
        public static void StopListening(string eventName, UnityAction listener)
        {
            if (eventManager == null) return;
            UnityEvent thisEvent = null;

            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Emit the Event with the given 'eventName'. All the functions linked to this Event, through the StartListening() method, will be called.
        /// </summary>
        /// <param name="eventName">The name of the Event to emit.</param>
        /// <param name="data">An optional data to store when this Event is emitted.</param>
        public static void EmitEvent(string eventName, object data = null)
        {
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                Instance.storeDictionary[eventName] = data;
            } else
            {
                Instance.storeDictionary.Add(eventName, data);
            }

            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        /// <summary>
        /// Return the data object that has been stored with this Event emitting. Return a null value if nothing has found.
        /// </summary>
        /// <param name="eventName">The name of the emitted Event.</param>
        /// <returns>
        /// The stored data object.
        /// </returns>
        public static object GetData(string eventName)
        {
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                return Instance.storeDictionary[eventName];
            }
            return null;
        }

        /// <summary>
        /// Return the Integer data that has been stored with this Event emitting. Return 0 if nothing has found.
        /// </summary>
        /// <param name="eventName">The name of the emitted Event.</param>
        /// <returns>
        /// The stored Integer data.
        /// </returns>
        public static int GetInteger(string eventName)
        {
            int defaultValue = 0;
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                try
                {
                    var value = (int)Instance.storeDictionary[eventName];
                    return value;
                }
                catch (System.Exception)
                {

                    return defaultValue;
                }
                
            }
            return defaultValue;
        }

        /// <summary>
        /// Return the String data that has been stored with this Event emitting. Return an empty string if nothing has found.
        /// </summary>
        /// <param name="eventName">The name of the emitted Event.</param>
        /// <returns>
        /// The stored String data.
        /// </returns>
        public static string GetString(string eventName)
        {
            string defaultValue = "";
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                try
                {
                    var value = (string)Instance.storeDictionary[eventName];
                    return value;
                }
                catch (System.Exception)
                {

                    return defaultValue;
                }

            }
            return defaultValue;
        }

        /// <summary>
        /// Return the Boolean data that has been stored with this Event emitting. Return false if nothing has found.
        /// </summary>
        /// <param name="eventName">The name of the emitted Event.</param>
        /// <returns>
        /// The stored Boolean data.
        /// </returns>
        public static bool GetBool(string eventName)
        {
            bool defaultValue = false;
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                try
                {
                    var value = (bool)Instance.storeDictionary[eventName];
                    return value;
                }
                catch (System.Exception)
                {

                    return defaultValue;
                }

            }
            return defaultValue;
        }

        /// <summary>
        /// Return the Float data that has been stored with this Event emitting. Return 0 if nothing has found.
        /// </summary>
        /// <param name="eventName">The name of the emitted Event.</param>
        /// <returns>
        /// The stored Float data.
        /// </returns>
        public static float GetFloat(string eventName)
        {
            float defaultValue = 0f;
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                try
                {
                    var value = (float)Instance.storeDictionary[eventName];
                    return value;
                }
                catch (System.Exception)
                {

                    return defaultValue;
                }

            }
            return defaultValue;
        }

        /// <summary>
        /// Return the GameObject that has been stored with this Event emitting. Return null if nothing has found.
        /// </summary>
        /// <param name="eventName">The name of the emitted Event.</param>
        /// <returns>
        /// The stored GameObject.
        /// </returns>
        public static GameObject GetGameObject(string eventName)
        {
            GameObject defaultValue = null;
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                try
                {
                    var value = (GameObject)Instance.storeDictionary[eventName];
                    return value;
                }
                catch (System.Exception)
                {

                    return defaultValue;
                }

            }
            return defaultValue;
        }

        /// <summary>
        /// Return true if an Event with the given 'eventName' exists in the Event Manager system.
        /// </summary>
        /// <param name="eventName">The name of the Event to check.</param>
        /// <returns>
        /// true if the name exists.
        /// </returns>
        public static bool EventExists(string eventName)
        {
            foreach (var listener in Instance.eventDictionary)
            {
                if (listener.Key == eventName) return true;
            }
            return false;
        }

        /// <summary>
        /// Remove any data stored in the specified 'eventName' Event.
        /// </summary>
        /// <param name="eventName">The name of the Event.</param>
        public static void ClearData(string eventName)
        {
            if (Instance.storeDictionary.ContainsKey(eventName))
            {
                Instance.storeDictionary.Remove(eventName);
            }
        }

        /// <summary>
        /// Clear all the stored data managed by the Event Manager.
        /// </summary>
        public static void ClearAllData()
        {
            Instance.storeDictionary = new Dictionary<string, object>();
        }

        /// <summary>
        /// Return true if the Event Manager is managing at least one Event.
        /// </summary>
        /// <returns>
        /// true if the Event Manager is managing at least one Event.
        /// </returns>
        public static bool IsListening()
        {
            return Instance.eventDictionary.Count > 0;
        }


        // Core part to make this class accessible.

        public bool makePersistent = false;

        private Dictionary<string, UnityEvent> eventDictionary;
        private Dictionary<string, object> storeDictionary;

        private static EEventManger eventManager;
        public static EEventManger Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EEventManger)) as EEventManger;

                    if (!eventManager)
                    {
                        Debug.LogError("Create an Empty GameObject and drag this Script on it.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            
        }

        void Awake()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityEvent>();
                storeDictionary = new Dictionary<string, object>();
            }

            if (makePersistent) DontDestroyOnLoad(gameObject);
        }

        private static UnityEvent FindEventByName(string eventName)
        {
            foreach (var listener in Instance.eventDictionary)
            {
                if (listener.Key == eventName) return listener.Value;
            }
            return null;
        }

    }
}


