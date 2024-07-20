namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportClientDto>), new XmlRootAttribute("Clients"));
            var clientsDtos = (List<ImportClientDto>)serializer.Deserialize(new StringReader(xmlString));

            var validClients= new List<Client>();

            StringBuilder sb=new StringBuilder();

            foreach(var clientDto in clientsDtos)
            {
                if(!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat
                };

                foreach(var add in clientDto.Addresses)
                {
                    if(!IsValid(add))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = new Address
                    {
                        StreetName = add.StreetName,
                        StreetNumber = add.StreetNumber,
                        PostCode = add.PostCode,
                        City = add.City,
                        Country = add.Country,
                    };
                    client.Addresses.Add(address);
                }
                validClients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClients,client.Name));
            }

            context.Clients.AddRange(validClients);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            var InvoicesDtos=JsonConvert.DeserializeObject<List<ImportInvoicesDto>>(jsonString);

            var validInvoices = new List<Invoice>();

            StringBuilder sb = new StringBuilder();

            foreach(var invoiceDto in InvoicesDtos)
            {
                if(!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime IssueDate= DateTime
                    .Parse(invoiceDto.IssueDate,CultureInfo.InvariantCulture);

                DateTime DueDate= DateTime
                    .Parse(invoiceDto.DueDate,CultureInfo.InvariantCulture);

                if(DueDate<=IssueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice = new Invoice
                {
                    Number = invoiceDto.Number,
                    IssueDate = IssueDate,
                    DueDate = DueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = Enum.Parse<CurrencyType>(invoiceDto.CurrencyType),
                    ClientId = invoiceDto.ClientId
                };

                validInvoices.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }

            context.Invoices.AddRange(validInvoices);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            var ProductsDtos = JsonConvert.DeserializeObject<List<ImportProductDto>>(jsonString);

            var validProducts = new List<Product>();

            StringBuilder sb = new StringBuilder();

            foreach(var productDto in ProductsDtos)
            {
                if(!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product product = new Product
                {
                    Name= productDto.Name,
                    Price= productDto.Price,
                    CategoryType=Enum.Parse<CategoryType>(productDto.CategoryType)
                };

                foreach(var clientId in  productDto.Clients.Distinct())
                {
                    if(!context.Clients.Any(c=>c.Id == clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient = new ProductClient
                    {
                        Product = product,
                        ClientId = clientId,
                    };
                    product.ProductsClients.Add(productClient);
                }
                validProducts.Add(product);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts,product.Name,product.ProductsClients.Count()));
            }

            context.Products.AddRange(validProducts);

            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
