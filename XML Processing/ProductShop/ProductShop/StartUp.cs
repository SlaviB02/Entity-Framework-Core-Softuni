using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //string input = File.ReadAllText("../../../Datasets/users.xml");
            //var res = ImportUsers(context, input);

            //string input = File.ReadAllText("../../../Datasets/products.xml");
            //var res = ImportProducts(context, input);

            //string input= File.ReadAllText("../../../Datasets/categories.xml");
            //var res=ImportCategories(context, input);

            //string input= File.ReadAllText("../../../Datasets/categories-products.xml");

            //var res=ImportCategoryProducts(context, input);

            Console.WriteLine();
            
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<UserDto>), new XmlRootAttribute("Users"));
            var users = (List<UserDto>)serializer.Deserialize(new StringReader(inputXml));

            var validUsers=users
                .Select(e=>new User
                {
                    FirstName= e.FirstName,
                    LastName= e.LastName,
                    Age= e.Age,
                })
                .ToList();


            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ProductDto>), new XmlRootAttribute("Products"));

            var productsDtos = (List<ProductDto>)serializer.Deserialize(new StringReader(inputXml));

            var products = productsDtos
                .Select(e => new Product
                {
                    Name = e.Name,
                    Price = e.Price,
                    SellerId = e.SellerId,
                    BuyerId = e.BuyerId,
                })
                .ToList();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoriesDto>), new XmlRootAttribute("Categories"));

            var categoriesDtos = (List<CategoriesDto>)serializer.Deserialize(new StringReader(inputXml));

            var categories = categoriesDtos
                .Where(c=>c.Name!=null)
                .Select(e=>new Category
                {
                    Name=e.Name,
                })
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryProductDto>), new XmlRootAttribute("CategoryProducts"));

            var categoriesProductsDtos = (List<CategoryProductDto>)serializer.Deserialize(new StringReader(inputXml));

            var categoriesProducts = categoriesProductsDtos
                .Where(cp=>cp.ProductId!=null && cp.CategoryId!=null)
                .Select(e=>new CategoryProduct
                {
                    ProductId=e.ProductId,
                    CategoryId=e.CategoryId
                }
                )
                .ToList();

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products=context.Products
                .Where(p=>p.Price>=500 && p.Price<=1000)
                .OrderBy(p=>p.Price)
                .Select(p=>new ProductExportDto
                {
                    Name = p.Name,
                    Price=p.Price.ToString(),
                    BuyerName=$"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<ProductExportDto>), new XmlRootAttribute("Products"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, products, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
             .Where(u=>u.ProductsSold.Any())  
             .Select(e => new SoldProductsDto
             {
                 FirstName = e.FirstName,
                 LastName = e.LastName,
                 Products = e.ProductsSold
                 .Select(p => new ProductExportDto
                 {
                     Name = p.Name,
                     Price = p.Price.ToString(),
                 })
                 .ToArray()
             })
              .OrderBy(u => u.LastName)
             .ThenBy(u => u.FirstName)
             .Take(5)
             .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SoldProductsDto>), new XmlRootAttribute("Users"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, users, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(e => new CategoryDtoExport
                {
                    Name= e.Name,
                    Count=e.CategoryProducts.Count(),
                    AveragePrice= e.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = e.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(pc => pc.Count)
                .ThenBy(pc=>pc.TotalRevenue)
                .ToList();


            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryDtoExport>), new XmlRootAttribute("Categories"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, categories, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersAndproducts = context
                .Users
                .ToArray()
                .Where(p => p.ProductsSold.Any())
                .Select(u => new UserDtoExport
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age =u.Age,
                    SoldProduct = new ExportProductCountDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new ExportProductModel
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                })
                .OrderByDescending(x => x.SoldProduct.Count)
                .Take(10)
                .ToArray();

            var resultDto = new ExportUserCountDto
            {
                Count = context.Users.Count(p => p.ProductsSold.Any()),
                Users = usersAndproducts
            };

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportUserCountDto), new XmlRootAttribute("Users"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, resultDto, namespaces);

            return sb.ToString().TrimEnd();

        }


    }
}