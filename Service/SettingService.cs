using Infrastructure;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SettingService
    {
        public SettingService(Database db)
        {
            Db = db;
            Setting = Db.Settings.FirstOrDefault();
        }

        private Database Db { get; }

        private Setting Setting { get; }

        public void SetValue(string key, string value)
        {
            if (Setting.Properties.TryGetValue(key, out _))
            {
                Setting.Properties[key] = value;
            }
            else
            {
                Setting.Properties.TryAdd(key, value);
            }

            Db.Update(Setting);
            Db.SaveChanges();
        }

        public string GetValue(string key) => Setting.Properties.TryGetValue(key, out string value) ? value : null;
    }
}
