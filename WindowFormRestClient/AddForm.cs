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
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var resources = await GoAddProduct();

            Console.WriteLine(resources);
        }

        static readonly HttpClient client = new HttpClient();
        async Task<string> GoAddProduct()
        {
            try
            {
                ProductModel p = new ProductModel();
                p.productName = txtName.Text;
                p.price = float.Parse(txtPrice.Text);

                var json = JsonConvert.SerializeObject(p);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", WindowFormRestClient.Properties.Settings.Default.access_token);
                HttpResponseMessage response = await client.PostAsync("http://localhost:50894/api/product", data);

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
    }

    class ProductModel
    {
        public string productName { set; get; }
        public float price { set; get; }
    }
}
