using LINQSamples.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQSamples
{
    class CustomerModel
    {
        public CustomerModel()
        {
            this.Orders = new List<OrderModel>();
        }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int OrderCount { get; set; }
        public List<OrderModel> Orders { get; set; }
    }
    class OrderModel
    {

        public int OrderId { get; set; }
        public decimal Total { get; set; }
    }

    class ProductModel
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }




    class Program  
    {
        static void Main(string[] args)
        {

            using (var db = new NorthWindContext())
            {
                //var sonuc = db.Database.ExecuteSqlRaw("delete from products  where productId=107");
                //Console.WriteLine(sonuc);

                //var sonuc = db.Database.ExecuteSqlRaw("update products set unitprice=unitprice*1.2 where categoryId=4");
                //Console.WriteLine(sonuc);

                //var products = db.Products.FromSqlRaw("select *from products where categoryId=4").ToList();
                //foreach (var item in products)
                //{
                //    Console.WriteLine(item.ProductName);    
                //}



            }
            Console.ReadLine();
        }

        private static void OneMoreTable5(NorthWindContext db)
        {
            //query expressions
            //var products = (from p in db.Products
            //                where p.UnitPrice > 10
            //                select p).ToList();

            var products = (from p in db.Products
                            join s in db.Suppliers on p.SupplierId equals s.SupplierId
                            select new
                            {
                                p.ProductName,
                                contactName = s.ContactName,
                                companyName = s.CompanyName
                            }).ToList();//inner join

            foreach (var item in products)
            {
                Console.WriteLine(item.ProductName + " " + item.companyName + " " + item.contactName);
            }
        }

        private static void OneMoreTable4(NorthWindContext db)
        {
            var products = db.Products
            .Select(p =>
            new
            {
                companyName = p.Supplier.CompanyName,
                contactName = p.Supplier.ContactName,
                p.ProductName
            }).ToList();//leftjoin

            foreach (var item in products)
            {
                Console.WriteLine(item.ProductName + " " + item.companyName + " " + item.contactName);
            }
        }

        private static void OneMoretable3(NorthWindContext db)
        {
            //var categories = db.Categories.Where(p=>p.Products.Count()==0).ToList();
            var categories = db.Categories.Where(p => p.Products.Any()).ToList();


            foreach (var item in categories)
            {
                Console.WriteLine(item.CategoryName);
            }
        }

        private static void OneMoreTable2(NorthWindContext db)
        {

            //var products = db.Products.Include(p=>p.Category).Where(p => p.Category.CategoryName == "Beverages");

            //foreach (var item in products)
            //{
            //    Console.WriteLine(item.ProductName + " " + item.CategoryId + "" + item.Category.CategoryName//include sayesinde categpry namelere ulastık);
            //}
            var products = db.Products
                .Where(p => p.Category.CategoryName == "Beverages")
                .Select(p => new
                {
                    name = p.ProductName,
                    id = p.CategoryId,
                    categoryname = p.Category.CategoryName
                })
                .ToList();


            foreach (var item in products)
            {
                Console.WriteLine(item.name + " " + item.id + " " + item.categoryname);
            }
        }

        private static void OneMoreTable(NorthWindContext db)
        {
            //var products = db.Products.Where(p => p.CategoryId == 1).ToList();
            var products = db.Products.Where(p => p.Category.CategoryName == "Beverages");


            foreach (var item in products)
            {
                Console.WriteLine(item.ProductName + " " + item.CategoryId/*+""+item.CategoryName --hata verir*/);
            }
        }

      
        private static void Delete3(NorthWindContext db)
        {
            var p1 = new Product() { ProductId = 82 };
            var p2 = new Product() { ProductId = 83 };

            var products = new List<Product>() { p1, p2 };

            db.Products.RemoveRange(products);
        }

        private static void Delete2(NorthWindContext db)
        {
            var p = new Product() { ProductId = 80 };//select sorgusu olusmadan silme

            //db.Entry(p).State = EntityState.Deleted;
            db.Products.Remove(p);
            db.SaveChanges();
        }

        private static void Delete1(NorthWindContext db)
        {
            var p = db.Products.Find(97);

            if (p != null)
            {
                db.Products.Remove(p);
                db.SaveChanges();

            }
        }

        private static void Guncelleme3(NorthWindContext db)
        {
            var product = db.Products.Find(1);

            if (product != null)
            {
                product.UnitPrice = 28;

                db.Products.Update(product);
                db.SaveChanges();
            }
        }

        private static void Guncellleme2(NorthWindContext db)
        {
            var p = new Product() { ProductId = 1 };

            db.Products.Attach(p);

            p.UnitsInStock = 50;
            db.SaveChanges();
        }

        private static void Guncelleme(NorthWindContext db)
        {
            var product = db.Products
                /*AsNoTracking()*/
                .Where(p => p.ProductId == 1).FirstOrDefault();
            if (product != null)
            {
                product.UnitsInStock += 10;
                db.SaveChanges();
                Console.WriteLine(" Veri Guncellendi");
            }
        }

        private static void Ekleme(NorthWindContext db)
        {
            var category = db.Categories.Where(i => i.CategoryName == "Beverages").FirstOrDefault();
            var p1 = new Product() { ProductName = "yeni ürün 11" };
            var p2 = new Product() { ProductName = "yeni ürün 12" };


            //var products = new List<Product>()
            //{
            //    p1,p2
            //};

            category.Products.Add(p1);
            category.Products.Add(p2);

            db.SaveChanges();

            Console.WriteLine("Veriler Eklendi");
            Console.WriteLine(p1.ProductId);
            Console.WriteLine(p2.ProductId);

            //var p1 = new Product() { ProductName = "yeni ürün 7", CategoryId = 1 };
            //var p2 = new Product() { ProductName = "yeni ürün 8", CategoryId = 1 };

            //var p1 = new Product() { ProductName = "yeni ürün 7", Category = category };
            //var p2 = new Product() { ProductName = "yeni ürün 8", Category = category };

            //var p1 = new Product() { ProductName = "yeni ürün 7", Category = new Category() { CategoryName="Yeni kategori 1"} };
            //var p2 = new Product() { ProductName = "yeni ürün 8", Category = new Category() { CategoryName = "Yeni kategori 2" } };
        }

        private static void Sıralama(NorthWindContext db)
        {
            //var result = db.Products.Count();
            //var result = db.Products.Count(i=>i.UnitPrice>10 &&i.UnitPrice<30);
            //var result = db.Products.Count(i => i.Discontinued);
            //var result = db.Products.Count(i => i.Discontinued==true);

            //var result = db.Products.Max(p=>p.UnitPrice);
            //var result = db.Products.Where(p=>p.CategoryId==2).Min(p=>p.UnitPrice);

            //var result = db.Products.Where(p => !p.Discontinued).Average(k => k.UnitPrice);//ortalama
            //var result = db.Products.Where(p => !p.Discontinued).Sum(k => k.UnitPrice);//toplam

            //var result = db.Products.OrderBy(k=>k.UnitPrice).ToList();//ürün fiyat artan şekilde
            /*var result = db.Products.OrderBy(k=>k.UnitPrice).ToList();*///ürün fiyat artan şekilde

            //var result = db.Products.OrderByDescending(k=>k.UnitPrice).ToList();//ürün fiyat azalan şekilde liste
            //var result = db.Products.OrderByDescending(k => k.UnitPrice).FirstOrDefault();//ürün fiyat azalan şekilde tek
            //Console.WriteLine(result.ProductName + " " + result.UnitPrice);

            //foreach (var item in result)
            //{
            //    Console.WriteLine(item.ProductName+" "+item.UnitPrice);
            //}



            //Console.WriteLine(result);
        }

        private static void Uygulama(NorthWindContext db)
        {
            ////tüm müşteri kayıtlarını getiriniz.(Customers)---------------------------
            //var customers = db.Customers.ToList();
            //foreach (var customer in customers)
            //{
            //    Console.WriteLine(customer.ContactName);
            //}

            ////tüm müsterilerin sadece customerId ve ContactName kolonlarını getirin----------------------------
            //var customers = db.Customers.Select(c => new { c.CustomerId, c.ContactName }).ToList();
            //foreach (var item in customers)
            //{
            //    Console.WriteLine(item.CustomerId+" "+item.ContactName);
            //}

            ////almanyada yasayan müsterilerin adlarını getirin-------
            //var customers = db.Customers.Select(c => new { c.ContactName, c.Country }).Where(c => c.Country == "Germany").ToList();
            //foreach (var item in customers)
            //{
            //    Console.WriteLine(item.Country + " " + item.ContactName);
            //}

            ////Diego roel isimli müsteri nerede yasamaktadır------------------------------
            //var customers = db.Customers.Where(p => p.ContactName == "Diego Roel").FirstOrDefault();
            //Console.WriteLine(customers.ContactName+" "+customers.CompanyName);

            ////stokta olmayan ürünler hangileridir--------------------------------
            //var products = db.Products.Where(i => i.UnitsInStock == 0).ToList();
            //var products = db.Products.Select(i=> new { i.ProductName,i.UnitsInStock}).Where(i => i.UnitsInStock == 0).ToList();
            //foreach (var item in products)
            //{
            //    Console.WriteLine(item.ProductName+" "+item.UnitsInStock);
            //}

            //Tüm calısanların ad ve soyadını tek kolon halinde getirin-----------------------------
            //var employes = db.Employees.Select(i => new { FullName = i.FirstName + " " + i.LastName }).ToList();
            //foreach (var item in employes)
            //{
            //    Console.WriteLine(item.FullName);
            //}

            ////ürünler tablosundaki ilk 5 kaydı alınız.(TAKE)-------------------------------------
            //var products = db.Products.Take(5).ToList();
            //foreach (var item in products)
            //{
            //    Console.WriteLine(item.ProductId+" "+item.ProductName);
            //}

            ////ürünler tablosundaki ikinci 5 kaydı alınız.(SKİP)--------------------------------
            //var products = db.Products.Skip(5).Take(5).ToList();
            //foreach (var item in products)
            //{
            //    Console.WriteLine(item.ProductId+" "+item.ProductName);
            //}
        }

        private static void Where(NorthWindContext db)
        {
            //var product = db.Products.Where(p => p.UnitPrice > 18).ToList();
            //var product = db.Products.Select(p => new { p.ProductName, p.UnitPrice }).Where(p => p.UnitPrice > 20);
            //var product = db.Products.Select(p => new { p.UnitPrice,p.ProductName }).Where(p => p.UnitPrice > 18 && p.UnitPrice < 30).ToList();
            //var product = db.Products.Where(p => p.CategoryId == 1).ToList();
            //var product = db.Products.Where(p => p.CategoryId == 1 || p.CategoryId == 5);
            //var product = db.Products.Where(p=>p.CategoryId==1).Select(p => new { p.ProductName, p.UnitPrice });
            //var product = db.Products.Where(i => i.ProductName == "Chai").ToList();
            var product = db.Products.Where(i => i.ProductName.Contains("Ch")).ToList();



            foreach (var item in product)
            {
                Console.WriteLine(item.UnitPrice + " " + item.ProductName);
            }
        }

        private static void Select(NorthWindContext db) 
        {
            //var products = db.Products.ToList();
            //var products = db.Products.Select(p => new { p.ProductName,p.UnitPrice }).ToList();
            //var products = db.Products.Select(p =>
            //new ProductModel {
            //    Name=p.ProductName,
            //    Price=p.UnitPrice 
            //}).ToList();

            var product = db.Products.Select(p => new { p.ProductName, p.UnitPrice }).FirstOrDefault();
            Console.WriteLine(product.ProductName + ' ' + product.UnitPrice);

            //foreach (var p in products)
            //{
            //    Console.WriteLine(p.Name + ' ' +p.Price);
            //}
        }
    }

}





//class Program
//{
//    static void Main(string[] args)
//    {

//        using (var db = new NorthWindContext())
//        {
//            //müşterilerin verdiği siparis toplamı

//            var customers = db.Customers
//                .Where(cus => cus.Orders.Any())
//                .Select(cus => new CustomerModel
//                {
//                    CustomerId = cus.CustomerId,
//                    CustomerName = cus.ContactName,
//                    OrderCount = cus.Orders.Count

//                })
//                .OrderBy(c => c.OrderCount)
//                .ToList();

//            foreach (var customer in customers)
//            {
//                Console.WriteLine(customer.CustomerId + "=>" + customer.CustomerName + " " + customer.OrderCount);
//            }

//        }
//        Console.ReadLine();
//    }