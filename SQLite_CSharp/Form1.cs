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
            string connString = "Data Source=" + Application.StartupPath + "\\person.db";
            DBOperation.connectionString = connString;
            btnRefreshAll_Click(sender, e);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                SetCurrentPerson(0, "", 0);
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

        private void SetCurrentPerson(int id, string name, int age)
        {
            txtID.Text = id.ToString();
            txtName.Text = name;
            txtAge.Text = age.ToString();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            Person p = GetCurrentPerson();
            string cmdText = "INSERT INTO " + strTableName +
                " VALUES(" + p.Id.ToString() + ", \"" + p.Name + "\", " + p.Age.ToString() + ")";
            Exception ret = DBOperation.SQLiteRequest_Write(cmdText);
            if (ret != null)
            {
                MessageBox.Show("Failed!\r\nret: " + ret.ToString() + "cmdText: " + cmdText, "Insert a row");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Person p = GetCurrentPerson();
            string cmdText = "UPDATE " + strTableName +
                " SET name=\"" + p.Name + "\", age=" + p.Age.ToString() + " WHERE id = "+ p.Id.ToString();
            Exception ret = DBOperation.SQLiteRequest_Write(cmdText);
            if (ret != null)
            {
                MessageBox.Show("Failed!\r\nret: " + ret.ToString() + "cmdText: " + cmdText, "Update a row");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                SetCurrentPerson(0, "", 0);
                MessageBox.Show("You should select a item in list view at first.", "Delete a row");
                return;
            }

            Person p = GetCurrentPerson();
            string cmdText = "DELETE FROM " + strTableName +
                " WHERE id=" + p.Id.ToString();
            Exception ret = DBOperation.SQLiteRequest_Write(cmdText);
            if (ret != null)
            {
                MessageBox.Show("Failed!\r\nret: " + ret.ToString() + "cmdText: " + cmdText, "Delete a row");
            }
        }

        private void btnRefreshAll_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            ArrayList list = DBOperation.SQLiteRequest_Read("SELECT * FROM " + strTableName);
            foreach (Person item in list)
            {
                ListViewItem viewItem = new ListViewItem(
                    new string[] { item.Id.ToString(), item.Name, item.Age.ToString() });
                listView1.Items.Add(viewItem);
            }
        }
    }
}
