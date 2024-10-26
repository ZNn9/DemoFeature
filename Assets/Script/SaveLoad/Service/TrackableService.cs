using System.Collections.Generic;
using Systems.SaveLoad.Model;

namespace Systems.SaveLoad.Service
{
    public class TrackableService
    {
        private TrackableObject trackableObject;

        public TrackableService()
        {
            trackableObject = new TrackableObject();
        }

        public void AddChange(string key)
        {
            if (trackableObject.IsModified.ContainsKey(key))
            {
                trackableObject.IsModified[key] = false;
            }
            else
            {
                trackableObject.IsModified.Add(key, false); 
            }
        }

        public void UpdateChange(string key, bool value)
        {
            if (trackableObject.IsModified.ContainsKey(key))
            {
                trackableObject.IsModified[key] = value;
            }
            else
            {
                trackableObject.IsModified.Add(key, value); 
            }
        }

        public bool GetChange(string key)
        {
            if (trackableObject.IsModified.TryGetValue(key, out bool value))
            {
                return value;
            }
            return false; 
        }

        public Dictionary<string, bool> GetAllChanges()
        {
            return trackableObject.IsModified;
        }

        public void ResetChanges()
        {
            foreach (var key in trackableObject.IsModified.Keys)
            {
                trackableObject.IsModified[key] = false;
            }
        }
    }
}