using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Text.Json;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext dbContext= new ProductShopContext();

            //string inputJson = File.ReadAllText("../../../Datasets/users.json");
            //var res = ImportUsers(dbContext, inputJson);
            //Console.WriteLine(res);

            //string inputJson = File.ReadAllText("../../../Datasets/products.json");
            //var res=ImportProducts(dbContext, inputJson);
            //Console.WriteLine(res);

            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            //var res=ImportCategories(dbContext, inputJson);
            //Console.WriteLine(res);

            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //var res=ImportCategoryProducts(dbContext, inputJson);
            //Console.WriteLine(res);

            Console.WriteLine(GetUsersWithProducts(dbContext));

        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
           List<User>users=JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product>products=JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson).Where(e=>e.Name!=null).ToList();
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct>categoryProducts=JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(e => e.Price >= 500 && e.Price <= 1000)
                .Select(e => new
                {
                    name = e.Name,
                    price=e.Price,
                    seller = $"{e.Seller.FirstName} {e.Seller.LastName}"
                })
                .OrderBy(e => e.price)
                .ToList();

            return JsonConvert.SerializeObject(products,Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(e => new
                {
                    firstName = e.FirstName,
                    lastName = e.LastName,
                    soldProducts = e.ProductsSold
                   .Select(ps => new
                   {
                       name = ps.Name,
                       price = ps.Price,
                       buyerFirstName = ps.Buyer.FirstName,
                       buyerLastName = ps.Buyer.LastName,
                   })
                })
                .OrderBy(b => b.lastName)
                .ThenBy(b => b.firstName)
                .ToList();

            return JsonConvert.SerializeObject(products,Formatting.Indented);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(e => new
                {
                    category = e.Name,
                    productsCount = e.CategoriesProducts.Count(),
                    averagePrice = e.CategoriesProducts.Average(p => p.Product.Price).ToString("F2"),
                    totalRevenue = e.CategoriesProducts.Sum(p => p.Product.Price).ToString("F2")
                })
                .OrderByDescending(pc => pc.productsCount)
                .ToList();

            return JsonConvert.SerializeObject (categories,Formatting.Indented);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(u => u.ProductsSold)
               .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
               .Select(e => new
               {
                   firstName = e.FirstName,
                   lastName = e.LastName,
                   age = e.Age,
                   soldProducts = new
                   {

                       count = e.ProductsSold.Count(ps => ps.BuyerId != null),
                       products = e.ProductsSold.Where(q => q.BuyerId != null)
                       .Select(pro => new
                       {
                           name = pro.Name,
                           price = pro.Price
                       })
                       .ToList()

                   }

               })
               .OrderByDescending(u => u.soldProducts.count)
               .ToList();

            var resultObject = new
            {
                usersCount = users.Count(),
                users = users
            };

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(resultObject,Formatting.Indented,settings);
        }
    }
}