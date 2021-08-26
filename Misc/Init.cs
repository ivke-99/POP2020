using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace SF_16_POP2020.Misc
{
    static class Init
    {
        public static void RunSchema()
        {
            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\schema.sql"));
            var dataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\data.sql"));
            string schema = File.ReadAllText(path);
            string data = File.ReadAllText(dataPath);
            using (var con = new MySqlConnection(Util.CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new MySqlCommand(schema, con))
                {
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new MySqlCommand(data, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
