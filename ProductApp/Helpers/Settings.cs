using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductApp
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string ListKey = "List_key_demo";
        private static readonly string ListKeyDefault = string.Empty;

        #endregion


        public static string AppCartList
        {
            get
            {
                return AppSettings.GetValueOrDefault(ListKey, ListKeyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ListKey, value);
            }
        }
    }
}