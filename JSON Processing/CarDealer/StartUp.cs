using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var dbContext =new CarDealerContext();

            //var input = File.ReadAllText("../../../Datasets/suppliers.json");

            //var res = ImportSuppliers(dbContext, input);
            //Console.WriteLine(res);

            //var input = File.ReadAllText("../../../Datasets/parts.json");

            //var res = ImportParts(dbContext, input);
            //Console.WriteLine(res);

            //var input = File.ReadAllText("../../../Datasets/cars.json");

            //var res = ImportCars(dbContext, input);
            //Console.WriteLine(res);

            //var input = File.ReadAllText("../../../Datasets/customers.json");

            //var res = ImportCustomers(dbContext, input);
            //Console.WriteLine(res);

            //var input = File.ReadAllText("../../../Datasets/sales.json");

            //var res = ImportSales(dbContext, input);
            //Console.WriteLine(res);

            Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));

        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            List<Supplier> suppliers=JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var validParts=parts.Where(p=>context.Suppliers.Select(i=>i.Id).ToList().Contains(p.SupplierId)).ToList(); 
            
            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            List<CarPartDto> cars = JsonConvert.DeserializeObject<List<CarPartDto>>(inputJson);

            foreach (var car in cars)
            {
                Car currentCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TraveledDistance = car.TravelledDistance
                };

                foreach (var part in car.PartsId)
                {
                    bool isValid = currentCar.PartsCars.FirstOrDefault(x => x.PartId == part) == null;
                    bool isPartValid = context.Parts.FirstOrDefault(p => p.Id == part) != null;

                    if (isValid && isPartValid)
                    {
                        currentCar.PartsCars.Add(new PartCar()
                        {
                            PartId = part
                        });
                    }
                }

                context.Cars.Add(currentCar);
            }

            context.SaveChanges();

            return $"Successfully imported {context.Cars.Count()}.";

        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            List<Customer> customers=JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers=context.Customers
                .OrderBy(b => b.BirthDate)
                .ThenBy(b => b.IsYoungDriver)
                .Select(e => new
                {
                    Name=e.Name,
                    BirthDate=e.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver=e.IsYoungDriver,
                })  
                .ToList();

            return JsonConvert.SerializeObject(customers,Formatting.Indented);
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars=context.Cars
                .Where(c=>c.Make=="Toyota")
                .Select(e => new
                {
                    e.Id,
                    e.Make,
                    e.Model,
                    e.TraveledDistance,
                })
                .OrderBy(c=>c.Model)
                .ThenByDescending(c=>c.TraveledDistance)
                .ToList();

            return JsonConvert.SerializeObject (cars,Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    PartsCount = e.Parts.Count
                })
                .ToList();

            return JsonConvert.SerializeObject(suppliers,Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars= context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance,
                    },
                    parts=c.PartsCars
                    .Select(p => new
                    {
                        Name=p.Part.Name,
                        Price=p.Part.Price.ToString("F2")
                    })
                })
                .ToList();

            

            return JsonConvert.SerializeObject(cars,Formatting.Indented);

        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
               .Where(c => context.Sales.Select(s => s.CustomerId).ToList().Contains(c.Id))
               .Select(e => new
               {
                   fullName = e.Name,
                   boughtCars = e.Sales.Count(),
                   spentMoney = e.Sales.Sum(pc => pc.Car.PartsCars.Sum(p => p.Part.Price))

               })
               .ToList()
               .OrderByDescending(sp => sp.spentMoney)
               .ThenByDescending(x => x.boughtCars)
               .ToList();
               

            return JsonConvert.SerializeObject (customers,Formatting.Indented);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(e => new
                {
                    car = new
                    {
                        e.Car.Make,
                        e.Car.Model,
                        e.Car.TraveledDistance
                    },
                    customerName = e.Customer.Name,
                    discount = e.Discount.ToString("F2"),
                    price = e.Car.PartsCars.Sum(e => e.Part.Price).ToString("F2"),
                    priceWithDiscount = (e.Car.PartsCars.Sum(e => e.Part.Price) - e.Car.PartsCars.Sum(e => e.Part.Price) * (e.Discount / 100)).ToString("F2")

                })
                .ToList();



            return JsonConvert.SerializeObject(sales,Formatting.Indented);
        }
    }
}