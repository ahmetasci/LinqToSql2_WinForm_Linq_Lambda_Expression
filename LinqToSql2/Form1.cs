using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqToSql2
{
    public partial class Form1 : Form
    {
        DataClasses1DataContext dtx = new DataClasses1DataContext();
        //DataContext instance'ını global aldık.
        //Facade'lara ulaşabilmemiz için bu kesinlikle gerekli

        private void dataGridDoldur()
        {
            //----------------------------Linq Expression ile 3 tablo için joinleme yaptık************************
            var query = from prod in dtx.Products
                        join ordet in dtx.Order_Details
                        on prod.ProductID equals ordet.ProductID

                        join or in dtx.Orders
                        on ordet.OrderID equals or.OrderID

                        join cus in dtx.Customers
                        on or.CustomerID equals cus.CustomerID

                        select new
                        {
                            prod.ProductID,      //anonymous type
                            prod.ProductName,
                            ordet.UnitPrice,
                            or.OrderDate,
                            cus.CompanyName,
                            cus.CustomerID,
                            or.OrderID,
                            ordet.Quantity

                        };
            
            dataGridView1.DataSource = query;
            //anonymous type'da eklediğimiz alanları datagridview kaynağı olarak ekledik.

            dataGridView1.Columns["CustomerID"].Visible = false;
            dataGridView1.Columns["OrderID"].Visible = false;
            //dataGridView1.Columns["ProductID"].Visible = false;
            //Görünürlüğünü kapattık.
        }

       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridDoldur();
        }

        private void btnGroupBy_Click(object sender, EventArgs e)
        {
            // butona tıklandığında satışları ürün isimlerine göre guruplayacak.Linq Expression ile
            var sonuc = from prod in dtx.Products
                        join ordet in dtx.Order_Details
                        on prod.ProductID equals ordet.ProductID

                        join or in dtx.Orders
                        on ordet.OrderID equals or.OrderID

                        //join cus in dtx.Customers
                        //on or.CustomerID equals cus.CustomerID

                        group ordet by prod.ProductName into guruplama
                        //ürün isimlerine göre satışları gösterecek..
                        orderby guruplama.Count() descending
                        //Satış adedine göre azalan sıralama yapar.
                        // guruplama.Key dersek ProductName'e göre azalan sıralama yapar.


                        select new
                {
                    UrunAdi = guruplama.Key, // distinct ProductName'i getirecek.
                    SatisAdedi = guruplama.Count(),// distinct productName'e göre sipariş adedi
                    SatisTutari = guruplama.Sum(total => total.Quantity * total.UnitPrice),// distict ProductName'e göre toplam satış tutarı
                    ToplamSatisMiktari = guruplama.Sum(total => total.Quantity)//distinct ProductName'e göre toplam satış miktarı
                   
                };

            //satısları urun ismine göre grupla
            //-------------------------------- 1.YOL-----------------------------------------------------
            //var sonuc =
            //            from prod in dtx.Products
            //            join od in dtx.Order_Details on prod.ProductID equals od.ProductID
            //            group od.UnitPrice * od.Quantity by prod.ProductName into Grup
            //            select new
            //            {
            //                Grup.Key,
            //                SatisAdedi = Grup.Count(),
            //                SatisTutari = Grup.Sum()

            //            };
            //-------------------------------------------------------------------------------------------
            //********************************2.YOL********************************************
            //var sonuc =
            //          from prod in ctx.Products
            //          join od in dtx.Order_Details on prod.ProductID equals od.ProductID
            //          group od by prod.ProductName into Grup
            //          select new
            //          {
            //              Grup.Key,
            //              SatisAdedi = Grup.Count(),
            //             ToplamSatisTutari = Grup.Sum(total=>total.Quantity*total.UnitPrice)

            //          };
            //**************************************************************************************************
            dataGridView1.DataSource = sonuc;


        }

       
    }
}
