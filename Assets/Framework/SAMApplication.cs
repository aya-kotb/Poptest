using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Poptropica2
{
    public class SAMApplication : MonoBehaviour
    {
		public GameConfiguration configuration;

        public static SAMApplication mainInstance
        {
            get
            {
                if (_instance == null) Instantiate(); //if Instantiate() needs parameters after the code structure is built, this should throw an error instead.
                return _instance;
            }
        }
        private static SAMApplication _instance;


        public static void Instantiate()
        {
            GameObject newGO = new GameObject("SAMApplication Instance");

            _instance = newGO.AddComponent<SAMApplication>();
            _instance.servicesByName = new Dictionary<string, IService>();
        }

        void Awake()
        {
            if (_instance == null) _instance = this;
            if (servicesByName == null) servicesByName = new Dictionary<string, IService>();
        }

		void Start()
		{
			configuration.CreateServices(this);
		}

        
        private Dictionary<string, IService> servicesByName;

#if UNITY_EDITOR
        public Dictionary<string, IService> CopyServiceDictionary()
        {
            if (servicesByName == null) return new Dictionary<string, IService>();
            return new Dictionary<string, IService>(servicesByName);
        }

        public void RenameService(string from, string to)
        {
            if (servicesByName.ContainsKey(from))
            {
                servicesByName.Add(to, servicesByName[from]);
                servicesByName.Remove(from);
            }
        }
#endif


        /// <summary>
        /// Adds a given service to the SAMApplication with the name given.
        /// </summary>
        /// <param name="name">The name of the service being added.</param>
        /// <param name="service">The instance of the service being added.</param>
        public void AddService(string name, IService service)
        {
            if (servicesByName == null) servicesByName = new Dictionary<string, IService>();
            if (service == null)
            {
                throw new System.Exception("cannot add a null service to the SAMApplication");
            }
            servicesByName.Add(name, service);
			service.StartService(this);
        }

        public T GetService<T>(string name) where T : class, IService
        {
            if (servicesByName == null) servicesByName = new Dictionary<string, IService>();
            if (servicesByName.ContainsKey(name))
            {
                if (servicesByName[name] is T)
                {
                    return (T)servicesByName[name];
                }
                else
                {
                    Debug.LogWarning("SAMApplication has a service named \"" + name + "\", but it is type " + name.GetType().ToString() + ", whice is not derived from the specified type " + typeof(T).ToString());
                }
            }
            else
            {
                Debug.LogWarning("SAMApplication does not have a service named " + name);
            }
            return null;
        }

        public T GetService<T>() where T : class, IService
        {
            if (servicesByName == null) servicesByName = new Dictionary<string, IService>();
            foreach (var serviceKVP in servicesByName)
            {
                if (serviceKVP.Value is T) return (T)serviceKVP.Value;
            }
            return null;
        }

        public void RemoveService(string serviceName)
        {
            if (servicesByName == null) return;
            if (servicesByName.ContainsKey(serviceName))
            {
				servicesByName[serviceName].StopService(this);
                servicesByName.Remove(serviceName);
            }
        }

        /// <summary>
        /// Removes the service with the given name and type, and returns it.
        /// </summary>
        /// <typeparam name="T">The type to remove.</typeparam>
        /// <param name="serviceName">The name to seek.</param>
        /// <returns>The service just removed.</returns>
        public T RemoveService<T>(string serviceName) where T : class, IService
        {
            T returningService = GetService<T>(serviceName);
            if (returningService != null)
            {
				servicesByName[serviceName].StopService(this);
                servicesByName.Remove(serviceName);
            }
            return returningService;
        }

        /// <summary>
        /// Removes the first service matching the given type it can find, and returns it. Slower than finding by name.
        /// </summary>
        /// <typeparam name="T">The type to remove.</typeparam>
        /// <returns>The service just removed.</returns>
        public T RemoveService<T>() where T : class, IService
        {
            if (servicesByName == null) servicesByName = new Dictionary<string, IService>();
            foreach (var serviceKVP in servicesByName)
            {
                if (serviceKVP.Value is T)
                {
                    T rtn = (T)serviceKVP.Value;
					servicesByName[serviceKVP.Key].StopService(this);
                    servicesByName.Remove(serviceKVP.Key);
                    return rtn;
                }
            }
            return null;
        }
    }


    public interface IService
    {
		/// <summary>
		/// Starts the service. Called when the service is added to the application.
		/// </summary>
		/// <param name="application">The SAM Application instance.</param>
		void StartService(SAMApplication application);

		/// <summary>
		/// Stops the service. Called when the service is removed frmo the application.
		/// </summary>
		/// <param name="application">The SAM Application instance.</param>
		void StopService(SAMApplication application);

		/// <summary>
		/// Configure the service with the specified config. This is called during game startup.
		/// </summary>
		/// <param name="config">The configuration for the service. Will be the specific type for the class.</param>
		void Configure(ServiceConfiguration config);
    }


}