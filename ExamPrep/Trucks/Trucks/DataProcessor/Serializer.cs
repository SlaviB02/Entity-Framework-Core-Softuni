namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = context.Despatchers
                .Where(d => d.Trucks.Any())
                .ToArray()
                .Select(d => new ExportDespatcherDto
                {
                    DespatcherName = d.Name,
                    Trucks = d.Trucks
                    .Select(t => new TruckExportDto
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray(),

                    TrucksCount = d.Trucks.Count()

                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DespatcherName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<ExportDespatcherDto>), new XmlRootAttribute("Despatchers"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, despatchers, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Select(t => t.Truck).Any(t => t.TankCapacity >= capacity))
                .ToArray()
                .Select(e => new
                {
                    Name = e.Name,
                    Trucks = e.ClientsTrucks
                    .Select(t => t.Truck)
                    .Where(t => t.TankCapacity >= capacity)
                
                    .ToArray()
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.RegistrationNumber,
                        VinNumber = t.VinNumber,
                        TankCapacity = t.TankCapacity,
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = t.CategoryType.ToString(),
                        MakeType = t.MakeType.ToString(),
                    })
                   .OrderBy(t => t.MakeType)
                    .ThenByDescending(t => t.CargoCapacity)
                    .ToList()
                })
                .OrderByDescending(t => t.Trucks.Count())
                .ThenBy(t => t.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }
    }
}
