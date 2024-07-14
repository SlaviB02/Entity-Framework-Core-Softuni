namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
       
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DistrictDto>), new XmlRootAttribute("Districts"));
            var districtsDtos = (List<DistrictDto>)serializer.Deserialize(new StringReader(xmlDocument));

            var validDistricts= new List<District>();

            StringBuilder sb=new StringBuilder();


            foreach(var districtDto in districtsDtos)
            {
                if(!IsValid(districtDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (dbContext.Districts.Any(d => d.Name == districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District district = new District
                {
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                    Region = (Region)Enum.Parse(typeof(Region), districtDto.Region)
                };
                foreach(var propDto in districtDto.Properties)
                {
                    if(!IsValid(propDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime date = DateTime
                        .ParseExact(propDto.DateOfAcquisition, "dd/MM/yyyy", CultureInfo
                        .InvariantCulture);

                    if (dbContext.Properties.Any(p=>p.PropertyIdentifier==propDto.PropertyIdentifier) ||district.Properties.Any(p => p.PropertyIdentifier == propDto.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(dbContext.Properties.Any(p=>p.Address==propDto.Address) || district.Properties.Any(p => p.Address == propDto.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property prop = new Property
                    {
                        PropertyIdentifier = propDto.PropertyIdentifier,
                        Area = propDto.Area,
                        Details = propDto.Details,
                        Address = propDto.Address,
                        DateOfAcquisition = date,
                    };

                    district.Properties.Add(prop);
                }

                validDistricts.Add(district);
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict,district.Name,district.Properties.Count));
            }

            dbContext.AddRange(validDistricts);
            dbContext.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            var citizensDtos=JsonConvert.DeserializeObject<List<CitizenDto>>(jsonDocument);

            List<Citizen>validCitizens=new List<Citizen>();

            StringBuilder sb = new StringBuilder();

            foreach(var c in citizensDtos) 
            {
                   if(!IsValid(c))
                   {
                    sb.AppendLine(ErrorMessage); 
                    continue;
                   }

                   DateTime date= DateTime
                    .ParseExact(c.BirthDate, "dd-MM-yyyy",CultureInfo.InvariantCulture);

                Citizen citizen = new Citizen
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    BirthDate = date,
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), c.MaritalStatus),
                };

                foreach(var prop in c.Properties)
                {
                    PropertyCitizen pc = new PropertyCitizen
                    {
                        Citizen = citizen,
                        PropertyId = prop
                    };
                    citizen.PropertiesCitizens.Add(pc);
                }

                validCitizens.Add(citizen);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen,citizen.FirstName, c.LastName,c.Properties.Count()));

            }

            dbContext.Citizens.AddRange(validCitizens);
            dbContext.SaveChanges();

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
