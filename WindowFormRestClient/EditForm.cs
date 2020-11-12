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
    public partial class EditForm : Form
    {
        public int id { set; get; }
        public EditForm()
        {
            InitializeComponent();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            var reources = await GoUpdateProduct(id);

            Console.Write(reources);
        }

        

        async Task<string> GoUpdateProduct(int id)
        {
            try
            {
                ProductModel p = new ProductModel();
                p.productName = txtName.Text;
                p.price = float.Parse(txtPrice.Text);

                var json = JsonConvert.SerializeObject(p);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WindowFormRestClient.Properties.Settings.Default.access_token);
                HttpResponseMessage response = await client.PutAsync($"http://localhost:50894/api/product/{id}", data);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
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

        private async void EditForm_Load(object sender, EventArgs e)
        {
            var resources = await GetAll(id);
            ModelTest1 result = JsonConvert.DeserializeObject<ModelTest1>(resources);

            txtName.Text = result.productName;
            txtPrice.Text = result.price.ToString();
        }

        static readonly HttpClient client = new HttpClient();
        static async Task<string> GetAll(int id)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WindowFormRestClient.Properties.Settings.Default.access_token);
                HttpResponseMessage response = await client.GetAsync($"http://localhost:50894/api/product/{id}");
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
    }

    class ModelTest1
    {
        public int productId { set; get; }
        public float price { set; get; }
        public string productName { set; get; }
    }
}
