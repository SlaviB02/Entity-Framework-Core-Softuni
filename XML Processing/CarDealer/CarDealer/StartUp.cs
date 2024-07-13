using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            //var input = File.ReadAllText("../../../Datasets/suppliers.xml");

            //Console.Write(ImportSuppliers(context, input));

            //var input = File.ReadAllText("../../../Datasets/parts.xml");

            //Console.WriteLine(ImportParts(context, input));

            //var input = File.ReadAllText("../../../Datasets/cars.xml");

            //Console.WriteLine(ImportCars(context, input));

            //var input = File.ReadAllText("../../../Datasets/customers.xml");

            //Console.WriteLine(ImportCustomers(context, input));

            //var input = File.ReadAllText("../../../Datasets/sales.xml");

            //Console.WriteLine(ImportSales(context, input));

            Console.WriteLine(GetTotalSalesByCustomer(context));

        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SuppliersDto>), new XmlRootAttribute("Suppliers"));
            var suppliersDtos = (List<SuppliersDto>)serializer.Deserialize(new StringReader(inputXml));

            var suppliers = suppliersDtos
                .Select(x => new Supplier
                {
                    Name = x.Name,
                    IsImporter = x.IsImporter,
                })
                .ToList();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";


        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PartDto>), new XmlRootAttribute("Parts"));
            var partsDtos = (List<PartDto>)serializer.Deserialize(new StringReader(inputXml));

            var parts = partsDtos
                .Select(e=>new Part
                {
                    Name=e.Name,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    SupplierId = e.SupplierId,
                })
                .Where(p=>context.Suppliers.Select(i=>i.Id).Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarPartDto>), new XmlRootAttribute("Cars"));
            var CarPartsDtos = (List<CarPartDto>)serializer.Deserialize(new StringReader(inputXml));

            var CarParts=CarPartsDtos
                .Select(e=>new CarPartDto
                {
                    Make=e.Make,
                    Model=e.Model,
                    TraveledDistance=e.TraveledDistance,
                    Parts=e.Parts
                })
                .ToList();

            var cars = new List<Car>();
            
            var allParts=context.Parts.Select(p=>p.Id).ToList();

            foreach(var CurrentCar in CarParts)
            {
                var distinctParts=CurrentCar.Parts.Select(e=>e.Id).Distinct();
                var parts = distinctParts.Intersect(allParts);

                var car = new Car
                {
                    Make = CurrentCar.Make,
                    Model = CurrentCar.Model,
                    TraveledDistance = CurrentCar.TraveledDistance,
                };
                foreach(var part in parts)
                {
                    var PartCar = new PartCar
                    {
                        PartId = part
                    };
                    car.PartsCars.Add(PartCar);
                }
                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {context.Cars.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomerDto>), new XmlRootAttribute("Customers"));
            var customerDtos = (List<CustomerDto>)serializer.Deserialize(new StringReader(inputXml));

            var customers=customerDtos
                .Select(e=>new Customer
                {
                    Name = e.Name,
                    BirthDate = e.BirthDate,
                    IsYoungDriver = e.IsYoungDriver,
                })
                .ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges() ;

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SalesDto>), new XmlRootAttribute("Sales"));
            var salesDtos = (List<SalesDto>)serializer.Deserialize(new StringReader(inputXml));

            var sales = salesDtos
                .Select(e=>new Sale
                {
                    CarId = e.CarId,
                    CustomerId  = e.CustomerId,
                    Discount = e.Discount,
                })
                .ToList();

            var validSales=sales.Where(p=>context.Cars.Select(i=>i.Id).Contains(p.CarId)).ToList();

            context.Sales.AddRange(validSales);
            context.SaveChanges() ;
            return $"Successfully imported {validSales.Count}";


        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .Select(e => new CarDto
                {
                    Make = e.Make,
                    Model = e.Model,
                    TraveledDistance = e.TraveledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            StringBuilder sb=new StringBuilder();

            XmlSerializer serializer=new XmlSerializer(typeof(List<CarDto>),new XmlRootAttribute("cars"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, cars,namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(e => new CarBMWDto
                {
                    Id=e.Id,
                    Model = e.Model,
                    TraveledDistance = e.TraveledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CarBMWDto>), new XmlRootAttribute("cars"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(e => new SupplierImporterDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    PartsCount=e.Parts.Count,
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SupplierImporterDto>), new XmlRootAttribute("suppliers"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, suppliers, namespaces);

            return sb.ToString().TrimEnd();
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {

            var cars= context.Cars
                .Select(e=>new CarPartExportDto
                {
                    Make=e.Make,
                    Model=e.Model,
                    TraveledDistance=e.TraveledDistance,
                    Parts=e.PartsCars
                    .Select(e=>e.Part)
                    .Select(pc=>new PartExportDto
                    {
                        Name=pc.Name,
                        Price=pc.Price,
                    })
                    .OrderByDescending(pc=>pc.Price)
                    .ToArray()
                })
                .OrderByDescending(c=>c.TraveledDistance)
                .ThenBy(c=>c.Model)
                .Take(5)
                .ToList();


            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CarPartExportDto>), new XmlRootAttribute("cars"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {

            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new
                {
                    Name = x.Name,
                    BoughtCars = x.Sales.Count().ToString(),
                    SpentMoney = x.Sales.Select(e => new
                    {
                        Prices = x.IsYoungDriver
                        ? e.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price * 0.95, 2))
                        : e.Car.PartsCars.Sum(pc => (double)pc.Part.Price)
                    })
                    .ToArray()
                })   
                .ToList();

            var validCustomer=customers
                .OrderByDescending(x=>x.SpentMoney.Sum(y=>y.Prices))
                .Select(e => new CustomerExportDto
                {
                    Name=e.Name,
                    BoughtCars=e.BoughtCars,
                    SpentMoney=e.SpentMoney.Sum(b=>(decimal)b.Prices)
                })
                .ToList();

        
           

          

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomerExportDto>), new XmlRootAttribute("customers"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, validCustomer, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var dtos = context
                .Sales
                .Select(s => new SalesExportDto()
                {
                    Car = new CarExportModel()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance.ToString()
                    },
                    Discount = s.Discount.ToString(CultureInfo.InvariantCulture),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(pc => pc.Part.Price).ToString(CultureInfo.InvariantCulture),
                    PriceWithDiscount =
                        (s.Car.PartsCars.Sum(pc => pc.Part.Price) -
                         (s.Car.PartsCars.Sum(pc => pc.Part.Price) * s.Discount / 100)).ToString(CultureInfo.InvariantCulture)
                })
                .ToList();


            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SalesExportDto>), new XmlRootAttribute("sales"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }

    }
}