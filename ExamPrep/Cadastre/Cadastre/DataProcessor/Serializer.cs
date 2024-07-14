using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.DataProcessor.ExportDtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            var properties = dbContext.Properties
                .Where(p => p.DateOfAcquisition >= new DateTime(2000, 1, 1))
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new
                {
                    p.PropertyIdentifier,
                    p.Area,
                    p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture),
                    Owners = p.PropertiesCitizens
                    .OrderBy(c=>c.Citizen.LastName)
                   .Select(c => new
                   {
                       LastName = c.Citizen.LastName,
                       MaritalStatus = c.Citizen.MaritalStatus.ToString(),
                   })
                   .ToList()
                })
                .ToList();

            return JsonConvert.SerializeObject(properties,Formatting.Indented);

        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var properties = dbContext.Properties
                .Where(p => p.Area >= 100)
                 .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportPropertyDto
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                })
               
                .ToList();



            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<ExportPropertyDto>), new XmlRootAttribute("Properties"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, properties, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
