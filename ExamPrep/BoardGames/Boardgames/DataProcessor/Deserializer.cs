namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using System.Xml;
    using Boardgames.Data;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Data.Models;
    using System.Text;
    using Boardgames.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CreatorImport>), new XmlRootAttribute("Creators"));
            var creatorDtos = (List<CreatorImport>)serializer.Deserialize(new StringReader(xmlString));

            var validCreators = new List<Creator>();

            StringBuilder sb=new StringBuilder();

            foreach(var creatorDto in creatorDtos)
            {
                if(!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                };

                foreach(var boardGameDto in creatorDto.Boardgames)
                {
                    if(!IsValid(boardGameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new Boardgame
                    {
                        Name = boardGameDto.Name,
                        Rating = boardGameDto.Rating,
                        YearPublished = boardGameDto.YearPublished,
                        CategoryType = (CategoryType)Enum.Parse(typeof(CategoryType), boardGameDto.CategoryType),
                        Mechanics = boardGameDto.Mechanics,
                    };
                      creator.Boardgames.Add(boardgame);  
                }

                validCreators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count()));
            }

            context.Creators.AddRange(validCreators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sellersDtos=JsonConvert.DeserializeObject<List<SellerImport>>(jsonString);

            var validSellers = new List<Seller>();

            StringBuilder sb = new StringBuilder();

            foreach (var sellerDto in sellersDtos)
            {
                if(!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website,
                };

                foreach(var bg in sellerDto.Boardgames.Distinct())
                {
                    if(!context.Boardgames.Select(b=>b.Id).ToList().Contains(bg))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    BoardgameSeller boardgame = new BoardgameSeller
                    {
                        BoardgameId = bg,
                        Seller = seller,
                    };
                    seller.BoardgamesSellers.Add(boardgame);
                }
                validSellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count()));
            }

            context.Sellers.AddRange(validSellers);
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
