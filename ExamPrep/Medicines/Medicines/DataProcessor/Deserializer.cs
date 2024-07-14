namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using System.Xml;
    using Medicines.DataProcessor.ImportDtos;
    using System.Xml.Linq;
    using Medicines.Data.Models;
    using System.Globalization;
    using Medicines.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            var patients=JsonConvert.DeserializeObject<List<ImportPatientsDto>>(jsonString);

            var validPatients=new List<Patient>();

            StringBuilder sb=new StringBuilder();

            foreach(var patient in patients)
            {
                if (!IsValid(patient))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Patient validPatient = new Patient
                {
                    FullName = patient.FullName,
                    AgeGroup = (AgeGroup)Enum.Parse(typeof(AgeGroup), patient.AgeGroup),
                    Gender = (Gender)Enum.Parse(typeof(Gender), patient.Gender),
                };
                foreach(var medicineId in patient.Medicines)
                {
                    if(validPatient.PatientsMedicines.Any(m=>m.MedicineId == medicineId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    PatientMedicine patientMedicine = new PatientMedicine
                    {
                        MedicineId = medicineId,
                        Patient = validPatient
                    };
                    validPatient.PatientsMedicines.Add(patientMedicine);
                }
                validPatients.Add(validPatient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient,validPatient.FullName,validPatient.PatientsMedicines.Count()));
            }

            context.Patients.AddRange(validPatients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PharmacyImportDto>), new XmlRootAttribute("Pharmacies"));
            var PharmacyDtos = (List<PharmacyImportDto>)serializer.Deserialize(new StringReader(xmlString));

            var validPharmacies = new List<Pharmacy>();

            StringBuilder sb = new StringBuilder();

            foreach(var pharmacydto in PharmacyDtos)
            {
                if(!IsValid(pharmacydto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Pharmacy pharmacy = new Pharmacy
                {
                    IsNonStop =bool.Parse(pharmacydto.IsNonStop),
                    Name = pharmacydto.Name,
                    PhoneNumber = pharmacydto.PhoneNumber,
                };

                foreach(var medic in  pharmacydto.Medicines)
                {
                    if(!IsValid(medic))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime dateProduction=DateTime
                        .ParseExact(medic.ProductionDate,"yyyy-MM-dd",CultureInfo.InvariantCulture);

                    DateTime dateExpiry = DateTime
                        .ParseExact(medic.ExpiryDate,"yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if(dateProduction>=dateExpiry)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (pharmacy.Medicines.Any(m => m.Name == medic.Name && m.Producer == medic.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new Medicine
                    {
                        Category =(Category)Enum.Parse(typeof(Category),medic.Category),
                        Name = medic.Name,
                        Price = medic.Price,
                        ProductionDate = dateProduction,
                        ExpiryDate = dateExpiry,
                        Producer = medic.Producer,
                    };

                    pharmacy.Medicines.Add(medicine);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count()));

                validPharmacies.Add(pharmacy);
            }

            context.Pharmacies.AddRange(validPharmacies);
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
