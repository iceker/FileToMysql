using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(@"D:\DATA\mogujie.txt");
            foreach (var item in lines)
            {
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();

                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=root;database=cloudsoft_global"))
                {
                    var line = item.Split(',');
                    for (int j = 0; j < line.Count(); j++)
                    {
                        var sql = "select * from mogujie where name='" + line[j] + "'";
                        ds = MySqlHelper.ExecuteDataset(conn, sql);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            var sql2 = "select * from mogujie_clean where fcid = '" + ds.Tables[0].Rows[i][4] + "'";
                            int rows = MySqlHelper.ExecuteDataset(conn, sql2).Tables[0].Rows.Count;
                            if (rows < 1)
                            {
                                conn.Close();
                                conn.Open();
                                var sql3 = "insert into mogujie_clean(createdDate,name,parentName,fcid,rawData) values(@createdDate,@name,@parentName,@fcid,@rawData)";
                                int executerows = MySqlHelper.ExecuteNonQuery(conn, sql3,
                                    new MySqlParameter("@createdDate", ds.Tables[0].Rows[i][1]),
                                    new MySqlParameter("@name", ds.Tables[0].Rows[i][2]),
                                    new MySqlParameter("@parentName", ds.Tables[0].Rows[i][3]),
                                    new MySqlParameter("@fcid", ds.Tables[0].Rows[i][4]),
                                    new MySqlParameter("@rawData", ds.Tables[0].Rows[i][5])
                                //new MySqlParameter("@createdDate", MySqlDbType.DateTime) { Value=ds.Tables[0].Rows[i][1]},
                                //new MySqlParameter("@name", MySqlDbType.VarChar) { Value = item },
                                //new MySqlParameter("@parentName", MySqlDbType.VarChar) { Value = ds.Tables[0].Rows[i][3] },
                                //new MySqlParameter("@fcid", MySqlDbType.VarChar) { Value = ds.Tables[0].Rows[i][4] },
                                //new MySqlParameter("@rawData", MySqlDbType.VarChar) { Value = ds.Tables[0].Rows[i][5] }
                                );
                                if (executerows > 0)
                                {
                                    Console.WriteLine("插入成功");
                                }
                                else
                                {
                                    Console.WriteLine("插入失败");
                                }

                            }

                        }
                    }
                }
                
            }

        }
    }
}
