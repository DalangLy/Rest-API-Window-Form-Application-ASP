using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowFormRestClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetAll_Click(object sender, EventArgs e)
        {
            displayData();
        }

        private async void displayData()
        {
            var resources = await GetAll();

            List<TestModel> result = JsonConvert.DeserializeObject<List<TestModel>>(resources);

            DataTable dt = new DataTable();
            dt.Columns.Add("Product ID", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Price", typeof(string));

            foreach (var item in result)
            {
                string pId = item.productId.ToString();
                string name = item.productName.ToString();
                string price = item.price.ToString();

                dt.Rows.Add(new object[] { pId, name, price });
            }

            dgvProduct.DataSource = dt;
        }

        static readonly HttpClient client = new HttpClient();
        static async Task<string> GetAll()
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WindowFormRestClient.Properties.Settings.Default.access_token);
                HttpResponseMessage response = await client.GetAsync("http://localhost:50894/api/product");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                if (responseBody != null)
                {
                    return responseBody;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return string.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddForm ap = new AddForm();
            ap.Show();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProduct.Rows.Count > 0 && dgvProduct.SelectedRows[0].Index != dgvProduct.Rows.Count)
                {
                    int selectedId = int.Parse(dgvProduct.SelectedRows[0].Cells[0].Value.ToString());
                    var resource = await GoDelete(selectedId);
                    Console.WriteLine(resource);
                    displayData();
                }
                else
                {
                    MessageBox.Show("No Product Selected Or No Product In List!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Row First " + ex);
            }
        }

        static async Task<string> GoDelete(int id)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WindowFormRestClient.Properties.Settings.Default.access_token);
                HttpResponseMessage response = await client.DeleteAsync($"http://localhost:50894/api/product/{id}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                if (responseBody != null)
                {
                    return responseBody;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return string.Empty;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProduct.Rows.Count > 0 && dgvProduct.SelectedRows[0].Index != dgvProduct.Rows.Count)
                {
                    EditForm editForm = new EditForm();
                    editForm.id = int.Parse(dgvProduct.SelectedRows[0].Cells[0].Value.ToString());
                    editForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Product Selected Or No Product In List!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Row First " + ex);
            }
        }
    }

    class TestModel    {
        public string productId { get; set; }
        public string productName { get; set; }
        public string price { set; get; }
    }
}
