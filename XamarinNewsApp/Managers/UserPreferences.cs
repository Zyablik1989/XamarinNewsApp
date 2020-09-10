using System;
using System.Collections.Generic;
using System.Text;

namespace NewsApp.Managers
{
    /// <summary>
    /// Here should be sqlite-entity-framework-integration
    /// </summary>
    class UserPreferences
    {
        private static UserPreferences _instance;
        public static UserPreferences Current => _instance ?? (_instance = new UserPreferences());

        public List<long> MarkedDownEntries = new List<long>();
    }
}
