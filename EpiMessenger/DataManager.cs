using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;

namespace EpiMessenger
{
    public class DataManager
    {
        static DataManager s_DataManager = null;

        static public DataManager GetDataManager()
        {
            if (s_DataManager == null)
                s_DataManager = new DataManager();
            return (s_DataManager);
        }

        public void StoreData<T>(string p_id, T p_data)
        {
            try
            {
                var prefs = Application.Context.GetSharedPreferences("EpiMessenger", FileCreationMode.Private);
                var prefEditor = prefs.Edit();

                if (typeof(T) == typeof(bool))
                    prefEditor.PutBoolean(p_id, (bool)(object)p_data);
                else if (typeof(T) == typeof(float))
                    prefEditor.PutFloat(p_id, (float)(object)p_data);
                else if (typeof(T) == typeof(int))
                    prefEditor.PutInt(p_id, (int)(object)p_data);
                else if (typeof(T) == typeof(long))
                    prefEditor.PutLong(p_id, (long)(object)p_data);
                else if (typeof(T) == typeof(string))
                    prefEditor.PutString(p_id, (string)(object)p_data);
                else
                {
                    Console.WriteLine("Error in DataManager.StoreData : unknown data type !");
                }
                prefEditor.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DataManager.StoreData : " + e.Message);
            }
        }


        // CAUTION :    THIS FUNCTION IS NOT SAFE !
        //              BE CAREFUL
        public T RetreiveData<T>(string p_id)
        {
            try
            {
                var prefs = Application.Context.GetSharedPreferences("EpiMessenger", FileCreationMode.Private);

                if (typeof(T) == typeof(bool))
                    return ((T)(object)prefs.GetBoolean(p_id, false));
                else if (typeof(T) == typeof(float))
                    return ((T)(object)prefs.GetFloat(p_id, -1f));
                else if (typeof(T) == typeof(int))
                    return ((T)(object)prefs.GetInt(p_id, -1));
                else if (typeof(T) == typeof(long))
                    return ((T)(object)prefs.GetLong(p_id, -1));
                else if (typeof(T) == typeof(string))
                    return ((T)(object)prefs.GetString(p_id, null));
                else
                {
                    Console.WriteLine("Error in DataManager.RetreiveData : unknown data type !");
                    return ((T)(object)null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DataManager.RetreiveData : " + e.Message);
                return ((T)(object)null);
            }
        }

        public void RemoveData(string p_id)
        {
            try
            {
                var prefs = Application.Context.GetSharedPreferences("EpiMessenger", FileCreationMode.Private);
                var prefEditor = prefs.Edit();

                prefEditor.Remove(p_id);
                prefEditor.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in DataManager.RemoveData : " + e.Message);
            }
        }
    }
}