using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Todo_List
{
    public partial class Todo : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader rdr;
        DBConnect dbConnect = new DBConnect();
        public Todo()
        {
            InitializeComponent();
            con = new SqlConnection(dbConnect.myConnection());
            loadTasks(0);
        }


        public void loadTasks(int completed)
        {
            int i = 0;
            dgvTasks.Rows.Clear();
            con.Open();
            string query = "SELECT * FROM tbTask WHERE completed = @completed";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@completed", completed);
            rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                dgvTasks.Rows.Add(i, rdr["id"].ToString(), rdr["name"].ToString(), rdr["description"].ToString());
                i++;
            }
            rdr.Close();
            con.Close();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "INSERT INTO tbTask (name,description) VALUES (@name,@description)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", nameTb.Text);
                cmd.Parameters.AddWithValue("@description", descTb.Text);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                loadTasks(0);
            }
        }
        int key;
        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            key = int.Parse(dgvTasks.Rows[e.RowIndex].Cells[1].Value.ToString());
            string colName = dgvTasks.Columns[e.ColumnIndex].Name;
            if(colName == "Edit")
            {
                nameTb.Text = dgvTasks.Rows[e.RowIndex].Cells[2].Value.ToString();
                descTb.Text = dgvTasks.Rows[e.RowIndex].Cells[3].Value.ToString();
                nameTb.Enabled = false;
                descTb.Enabled = false;
                addBtn.Enabled = false;
            }
            else if(colName == "Delete")
            {
                try
                {
                    con.Open();
                    string query = "DELETE FROM tbTask WHERE id = @id";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", key);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    loadTasks(0);
                    con.Close();
                }
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "UPDATE tbTask SET completed = " +1+ " WHERE id = @id";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", key);
                cmd.ExecuteNonQuery();
                con.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                loadTasks(0);
            }
        }

        private void showBtn_Click(object sender, EventArgs e)
        {
            loadTasks(1);
        }

        private void showNotBtn_Click(object sender, EventArgs e)
        {
            loadTasks(0);
        }
    }
}