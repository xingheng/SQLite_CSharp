using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace SQLite_CSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static readonly string strTableName = "person";

        private void Form1_Load(object sender, EventArgs e)
        {
            FileInfo dbFile = new FileInfo(@"..\..\Resources\person.db");
            if (dbFile.Exists)
            {
                string fullpath = dbFile.FullName;
                txtDBPath.Text = fullpath;
                string connString = "Data Source=" + fullpath;
                DBOperation.connectionString = connString;
                btnRefreshAll_Click(sender, e);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                SetCurrentPerson(null);
                return;
            }

            int index = listView1.SelectedItems[0].Index;
            ListViewItem viewItem = listView1.Items[index];

            int curIndex = 0;
            foreach (var item in viewItem.SubItems)
            {
                string value = ((ListViewItem.ListViewSubItem)item).Text.ToString();
                switch (curIndex++)
                {
                    case 0:
                        txtID.Text = value;
                        break;
                    case 1:
                        txtName.Text = value;
                        break;
                    case 2:
                        txtAge.Text = value;
                        break;
                    default:
                        MessageBox.Show("Additional columns???");
                        break;
                }
            }
        }

        private Person GetCurrentPerson()
        {
            int id = 0, age = 0;
            string name = txtName.Text.ToString().Trim();
            Int32.TryParse(txtID.Text.ToString().Trim(), out id);
            Int32.TryParse(txtAge.Text.ToString().Trim(), out age);

            return new Person(id, name, age);
        }

        private void SetCurrentPerson(Person p)
        {
            if (p != null)
            {
                txtID.Text = p.Id.ToString();
                txtName.Text = p.Name;
                txtAge.Text = p.Age.ToString();
            }
            else
                txtID.Text = txtName.Text = txtAge.Text = "";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            Person p = GetCurrentPerson();
            string cmdText = "INSERT INTO " + strTableName +
                " VALUES(@id,@name,@age)";
            Exception ret = DBOperation.SQLiteRequest_Write(cmdText,
                "@id", p.Id,
                "@name", p.Name,
                "@age", p.Age);
            if (ret != null)
            {
                MessageBox.Show("Failed!\r\nret: " + ret.ToString() + "cmdText: " + cmdText, "Insert a row");
            }
            btnRefreshAll_Click(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Person p = GetCurrentPerson();
            string cmdText = "UPDATE " + strTableName +
                " SET name=@name, age=@age WHERE id=@id";
            Exception ret = DBOperation.SQLiteRequest_Write(cmdText,
                "@id", p.Id,
                "@name", p.Name,
                "@age", p.Age);
            if (ret != null)
            {
                MessageBox.Show("Failed!\r\nret: " + ret.ToString() + "cmdText: " + cmdText, "Update a row");
            }
            btnRefreshAll_Click(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                SetCurrentPerson(null);
                MessageBox.Show("You should select a item in list view at first.", "Delete a row");
                return;
            }

            Person p = GetCurrentPerson();
            string cmdText = "DELETE FROM " + strTableName +
                " WHERE id=@id";
            Exception ret = DBOperation.SQLiteRequest_Write(cmdText,
                "@id", p.Id);
            if (ret != null)
            {
                MessageBox.Show("Failed!\r\nret: " + ret.ToString() + "cmdText: " + cmdText, "Delete a row");
            }
            btnRefreshAll_Click(sender, e);
        }

        private void btnRefreshAll_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            DataTable dt = null;
            dt = (DataTable)DBOperation.SQLiteRequest_Read("SELECT * FROM " + strTableName);
            foreach (DataRow row in dt.Rows)
            {
                int count = row.ItemArray.Length;
                string[] cellList = new string[count];
                for (int i = 0; i < count; i++)
                    cellList[i] = row.ItemArray[i].ToString();

                ListViewItem viewItem = new ListViewItem(cellList);
                listView1.Items.Add(viewItem);
            }
        }

        private void btnChangeDB_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = openFileDialog1.FileName;
                txtDBPath.Text = filepath;
                string connString = "Data Source=" + filepath;
                DBOperation.connectionString = connString;
                btnRefreshAll_Click(sender, e);
            }
        }

    }
}
