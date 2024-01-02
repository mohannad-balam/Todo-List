using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_List
{
    internal class DBConnect
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr;
        private string conString;
        public string myConnection()
        {
            conString = @"Data Source=APTX4869;Initial Catalog=TodoList;Integrated Security=True";
            return conString;
        }
    }
}
