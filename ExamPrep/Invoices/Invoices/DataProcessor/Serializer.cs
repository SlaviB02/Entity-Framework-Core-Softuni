namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var clients = context.Clients
               .Where(c => c.Invoices.Any(i => i.IssueDate >= date))
               .OrderByDescending(c => c.Invoices.Count())
               .ThenBy(c => c.Name)
               .ToArray()      
               .Select(e => new ExportClientDto
               {
                   ClientName = e.Name,
                   VatNumber = e.NumberVat,
                   InvoicesCount = e.Invoices.Count(),
                   Invoices = e.Invoices
                    .OrderBy(i => i.IssueDate)
                   .ThenByDescending(i => i.DueDate)
                   .Select(i => new ExportinvoiceDto
                   {
                       InvoiceNumber = i.Number,
                       InvoiceAmount = i.Amount,
                       DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                       Currency = i.CurrencyType.ToString(),
                   })             
                   .ToArray(),
                  
               })
               
               .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<ExportClientDto>), new XmlRootAttribute("Clients"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, clients, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products
              .Where(c=>c.ProductsClients.Any(c => c.Client.Name.Length >= nameLength))
              .ToArray()
              .Select(e => new
              {
                  e.Name,
                  e.Price,
                  Category = e.CategoryType.ToString(),
                  Clients = e.ProductsClients
                  .Where(p=>p.Client.Name.Length >= nameLength)
                  .ToArray()
                  .Select(c => new
                  {
                      Name = c.Client.Name,
                      NumberVat = c.Client.NumberVat,
                  })
                  .OrderBy(n => n.Name)
                  .ToList()
              })
              .OrderByDescending(p => p.Clients.Count())
              .ThenBy(p => p.Name)
              .Take(5)
              .ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}