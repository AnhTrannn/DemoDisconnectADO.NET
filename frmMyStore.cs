using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoDisconnectADO.NET
{
    public partial class frmMyStore : Form
    {
        public frmMyStore()
        {
            InitializeComponent();
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var strConnection = config["ConnectionStrings:MyStoreDB"];
            return strConnection;
        }

        DataSet dsMyStore = new DataSet();

        private void frmMyStore_Load(object sender, EventArgs e)
        {
            DbProviderFactory factory = SqlClientFactory.Instance;
            using DbConnection conn = factory.CreateConnection();
            if (conn != null)
            {
                Console.WriteLine($"Unable to create the connection object");
                return;
            }

            conn.ConnectionString = GetConnectionString();
            conn.Open();

            string sql = "Select ProductID, ProductName, CategoryID, Price from Products; Select * from Categories";
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, GetConnectionString());
                adapter.Fill(dsMyStore);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Data from Database");
            }
        }

        private void btnViewProducts_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = dsMyStore.Tables[0];
        }

        private void btnViewCategories_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = dsMyStore.Tables[1];
        }
    }
}
