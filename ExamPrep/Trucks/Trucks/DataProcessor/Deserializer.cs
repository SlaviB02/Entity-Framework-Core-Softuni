namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DespatcherDtoImport>), new XmlRootAttribute("Despatchers"));
            var DespatcherDtos = (List<DespatcherDtoImport>)serializer.Deserialize(new StringReader(xmlString));

            var validDespatchers = new List<Despatcher>();

            StringBuilder sb =new StringBuilder();

            foreach(var despatchDto in DespatcherDtos)
            {
                if(!IsValid(despatchDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(despatchDto.Position==null || despatchDto.Position ==string.Empty)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher
                {
                    Name = despatchDto.Name,
                    Position = despatchDto.Position,
                };

                foreach(var truckDto in despatchDto.Trucks)
                {
                    if(!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = Enum.Parse<CategoryType>(truckDto.CategoryType),
                        MakeType = Enum.Parse<MakeType>(truckDto.MakeType)
                    };
                    despatcher.Trucks.Add(truck);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count()));
                validDespatchers.Add(despatcher);
            }

            context.Despatchers.AddRange(validDespatchers);

            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var ClientDtos=JsonConvert.DeserializeObject<List<ImportClientDto>>(jsonString);

            var validClients = new List<Client>();

            StringBuilder sb=new StringBuilder();

            foreach(var clientDto in ClientDtos)
            {
                if(!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(clientDto.Type=="usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type,
                };

                foreach(var truckId in  clientDto.Trucks.Distinct())
                {
                    if(!context.Trucks.Any(t=>t.Id== truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    ClientTruck clientTruck = new ClientTruck
                    {
                        Client = client,
                        TruckId = truckId,
                    };
                    client.ClientsTrucks.Add(clientTruck);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name,client.ClientsTrucks.Count()));
                validClients.Add(client);
            }

            context.Clients.AddRange(validClients);

            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}