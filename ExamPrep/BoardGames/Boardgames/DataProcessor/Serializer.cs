namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .Where(c => context.Boardgames.Any(b => b.CreatorId == c.Id))
                .ToArray()
                .Select(e => new CreatorExport
                {
                    CreatorName = $"{e.FirstName} {e.LastName}",
                    BoardgamesCount=e.Boardgames.Count,
                    Boardgames = e.Boardgames
                    .Select(b => new BoardGameExport
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished
                    })
                    .OrderBy(b=>b.BoardgameName)
                    .ToArray()
                })
                .OrderByDescending(e=>e.Boardgames.Length)
                .ThenBy(e=>e.CreatorName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CreatorExport>), new XmlRootAttribute("Creators"));

            using StringWriter sw = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(sw, creators, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => context.BoardgamesSellers.Any(bs => bs.SellerId == s.Id)
                && s.BoardgamesSellers.Any(b => b.Boardgame.YearPublished >= year)
                && s.BoardgamesSellers.Any(b => b.Boardgame.Rating <= rating))
                .ToArray()
                .Select(e => new
                {
                    Name = e.Name,
                    Website = e.Website,
                    Boardgames = e.BoardgamesSellers
                    .Where(b => b.Boardgame.YearPublished >= year && b.Boardgame.Rating <= rating)
                    .ToArray()
                    .Select(bg => new
                    {
                        Name = bg.Boardgame.Name,
                        Rating = bg.Boardgame.Rating,
                        Mechanics = bg.Boardgame.Mechanics,
                        Category = bg.Boardgame.CategoryType.ToString(),
                    })
                    .OrderByDescending(bg => bg.Rating)
                    .ThenBy(bg => bg.Name)
                    .ToList()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToList();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);

        }
    }
}