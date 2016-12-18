using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSaver
{
    public class Settings
    {
        private const string RegistryKeyKey = "SOFTWARE\\3ft9\\MazeSaver";
        private const string BoxSizeKey = "boxsize";
        private const string RestartDelayKey = "restartdelay";
        private const string AnimationSpeedKey = "animationspeed";

        static private int GetInt(String key, int defaultValue)
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(RegistryKeyKey);
            if (null == regkey)
            {
                return defaultValue;
            }
            return Convert.ToInt32(regkey.GetValue(key, defaultValue));
        }

        static private void Set(String key, int value)
        {
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey(RegistryKeyKey);
            regkey.SetValue(key, value);
        }

        static public int BoxSize
        {
            get
            {
                return GetInt(BoxSizeKey, 65);
            }

            set
            {
                Set(BoxSizeKey, value);
            }
        }

        static public int RestartDelay
        {
            get
            {
                return GetInt(RestartDelayKey, 3);
            }

            set
            {
                Set(RestartDelayKey, value);
            }
        }

        static public int AnimationSpeed
        {
            get
            {
                return GetInt(AnimationSpeedKey, 20);
            }

            set
            {
                Set(AnimationSpeedKey, value);
            }
        }
    }
}
