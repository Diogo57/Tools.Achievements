using Stump.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Achievements
{
    public class DatabaseManager
    {
        public static Stump.ORM.Database DefaultDatabase;

        private static Stump.ORM.Database m_database;

        public static Stump.ORM.Database Database
        {
            get
            {
                return m_database ?? DefaultDatabase;
            }
        }

        public static void Initialize()
        {
            m_database = new Stump.ORM.Database("database=;uid=;password=;server=;port=3306", "MySql.Data.MySqlClient");
            Database.OpenSharedConnection();
        }
    }
}
