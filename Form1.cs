using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Price
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool status = false;
        string goodsid = "";
        float lowprice = 0;
        private void Button1_Click(object sender, EventArgs e)
        {
            if (status)
            {
                status = false;
                button1.Text = "开始";
                timer1.Stop();
            }
            else
            {
                status = true;
                button1.Text = "停止";
                goodsid = textBox1.Text.Trim();
                
                lowprice = float.Parse(textBox2.Text.Trim());
                setdata(goodsid);

                timer1.Start();

                this.notifyIcon1.ShowBalloonTip(15, "开始", "已开始监控", ToolTipIcon.Info);
            }

        }

        public void setdata(string _goodsid)
        {
            string url = "https://goods.kaola.com/product/getGoodsRecommendInfoRecentlyView.json?goodsIdString=" + _goodsid + "&v=1";


            string jsonText = HttpUitls.Get(url);
            JObject json1 = (JObject)JsonConvert.DeserializeObject(jsonText);
            JArray array = (JArray)json1["data"];

            float price = float.Parse(array[0]["currentPrice"].ToString());
            string title = array[0]["title"].ToString();

            current.Text = price.ToString();
            goodsname.Text = title;


        }
        public float getprice(string _goodsid)
        {
            string url = "https://goods.kaola.com/product/getGoodsRecommendInfoRecentlyView.json?goodsIdString="+ _goodsid + "&v=1";
      

            string jsonText = HttpUitls.Get(url);
            JObject json1 = (JObject)JsonConvert.DeserializeObject(jsonText);
            JArray array = (JArray)json1["data"];

            float price = float.Parse(array[0]["currentPrice"].ToString());



            return price;


        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            float price = getprice(goodsid); 
            

            if (price <= lowprice)
            {
                listBox1.Items.Add(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " !!!监控到警戒价格!!! 商品ID:" + goodsid + " 价格:" + price.ToString());
                this.notifyIcon1.ShowBalloonTip(15, "监控到警戒价格", "当前商品价格为" + price, ToolTipIcon.Info);
            }
            else {
                listBox1.Items.Add(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " 监控中... 商品ID:" + goodsid + " 价格:" + price.ToString());
                
            }
            listBox1.SetSelected(listBox1.Items.Count - 1, true);
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("商品ID就是商品的序号，如商品页面：https://goods.kaola.com/product/5674256.html，那么5674256就是商品ID");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("即警戒价格，监控到低于或等于改价格则会在桌面发出通知");
        }
    }
}
