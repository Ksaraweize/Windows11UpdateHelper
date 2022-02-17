using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace Windows11UpdateHelper
{
    class RegistryUtil
    {
        public static string? GetString(RegistryKey key, string subKeyPath, string name)
        {

            using var subKey = key.OpenSubKey(subKeyPath);
            return subKey?.GetValue(name, null)?.ToString();
        }

        public static void SetString(RegistryKey key, string subKeyPath, string name, string value)
        {
            using var subKey = key.CreateSubKey(subKeyPath);
            subKey?.SetValue(name, value, RegistryValueKind.String);
        }

        public static uint? GetDword(RegistryKey key, string subKeyPath, string name)
        {
            using var subKey = key.OpenSubKey(subKeyPath);
            return uint.TryParse(subKey?.GetValue(name, null)?.ToString(), out UInt32 result) ? result : null;
        }

        public static void SetDword(RegistryKey key, string subKeyPath, string name, UInt32 value)
        {
            using var subKey = key.CreateSubKey(subKeyPath);
            subKey?.SetValue(name, value, RegistryValueKind.DWord);
        }

    }
}
