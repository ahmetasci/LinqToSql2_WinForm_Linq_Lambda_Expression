using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqToSql2_1
{
    public partial class Form1 : Form
    {
        NothWindDataContext ctx = new NothWindDataContext();
        //Facade'lara ulaşabilmemiz için gerekli

        //****JOIN İŞLEMLERİ LAMBDA EXPRESSION ile yapıldı*****
        private void dataGridDoldur()
        {
            //******************1.YOL*******************
            //var query = ctx.Products.Join(ctx.Categories,
            //         urun => urun.CategoryID,
            //         cat => cat.CategoryID,
            //         (urun, cat) => new
            //         {
            //             Urun = urun,
            //             Cat = cat,
            //         }).Select(x => new
            //         {
            //             x.Urun.ProductID,
            //             x.Urun.UnitPrice,
            //             x.Urun.UnitsInStock,
            //             x.Cat.CategoryName
            //         });
            //******************2.YOL*************************
            var sonuc = ctx.Products.Join(ctx.Categories,
                prd => prd.CategoryID,
                cat => cat.CategoryID,


                (prd, cat) => new
                {
                    UrunAdi = prd.ProductName,
                    BirimFiyat = prd.UnitPrice,
                    StokMiktari = prd.UnitsInStock,
                    KategoriAdi = cat.CategoryName
                });


            dataGridView1.DataSource = sonuc;


        }
            public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             dataGridDoldur();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Button1' tıklandığında ProductName,UnitPrice,UnitsInStock,CategoryName,CompanyName'i
            dataGridView1'de göstersin.
            */


            //*************************
            //***************LAMBDA EXPRESSION ile 3 Tabloyu Joinledik
            //*************************
            var sonuc = ctx.Products.Join(ctx.Categories,
               prd => prd.CategoryID,
               cat => cat.CategoryID,

               (prd, cat) => new
               {
                   prd.ProductName,
                   prd.UnitPrice,
                   prd.UnitsInStock,
                   cat.CategoryName,
                   prd.SupplierID
               }).Join(ctx.Suppliers,
               pro => pro.SupplierID,
               sup => sup.SupplierID,
               (pro, sup) => new
               {
                   pro.ProductName,
                   pro.UnitPrice,
                   pro.CategoryName,
                   pro.UnitsInStock,
                   sup.CompanyName

               });
                    dataGridView1.DataSource = sonuc;
        }
    }
}
