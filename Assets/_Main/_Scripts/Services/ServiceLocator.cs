using System.Collections.Generic;
using UnityEngine;

namespace _Main._Scripts.Services
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance => _instance ??= new ServiceLocator();

        private static ServiceLocator _instance;
        private static readonly Dictionary<ServiceType,IService> Services = new();

        public static void ClearInstance()
        {
            _instance = null;
        }

        public bool TryAddService(ServiceType type, IService service)
        {
            if (Services.TryAdd(type, service))
                return true;
            Debug.Log($"Error, the service {type} already exists!");
            return false;
        }

        public bool TryGetService(ServiceType type, out IService service)
        {
            if (Services.TryGetValue(type, out service)) return true;
            Debug.Log($"Service - {type} not created yet");
            return false;
        }
    }
}