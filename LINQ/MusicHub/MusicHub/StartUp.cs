namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            Console.WriteLine(ExportAlbumsInfo(context,9));

        
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb=new StringBuilder();

            var albums=context.Producers
                .FirstOrDefault(x=>x.Id==producerId)
                .Albums
                .Select(e=>new
                {
                    e.Name,
                    e.ReleaseDate,
                   ProducerName=e.Producer.Name,
                   e.Songs,
                   e.Price
                })
               .OrderByDescending(x => x.Price)
                .ToList();

            

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");
               var songs = album.Songs
                    .Select(e => new
               {
                 SongName=e.Name,
                   e.Price,
                   SongWriter=e.Writer.Name
               })
                .OrderByDescending(x=>x.SongName)
                .ThenBy(x=>x.SongWriter)
                .ToList();
                int index = 1;
                foreach(var song in  songs)
                {
                    sb.AppendLine($"---#{index}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:F2}");
                    sb.AppendLine($"---Writer: {song.SongWriter}");
                    index++;
                }
                sb.AppendLine($"-AlbumPrice: {album.Price:F2}");
            }


            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb=new StringBuilder();

            var Songs=context.Songs
                 .ToList()
                .Where(x=>x.Duration.TotalSeconds>duration)
                .Select(e => new
                {
                   SongName=e.Name,
                   PerformerFullName=
                   e.SongPerformers.ToArray()
                   .Select(sp=>$"{sp.Performer.FirstName} {sp.Performer.LastName}")
                   .ToList(),
                  WriterName=e.Writer.Name,
                   AlbumProducer=e.Album.Producer.Name,
                   e.Duration
                })
                .OrderBy(e=>e.SongName)
                .ThenBy(e=>e.WriterName)
                .ThenBy(e=>e.PerformerFullName)
                .ToList();

            int index = 1;
            foreach (var song in Songs)
            {
                sb
                    .AppendLine($"-Song #{index}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WriterName}");
                   if(song.PerformerFullName!=null)
                {
                    if(song.PerformerFullName.Count==1)
                    {
                        sb.AppendLine($"---Performer: {song.PerformerFullName.First()}");
                    }
                    else
                    {
                        foreach(var name in song.PerformerFullName.OrderBy(e=>e))
                        {
                            sb.AppendLine($"---Performer: {name}");
                        }
                    }
                    
                }
                sb
                 .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                 .AppendLine($"---Duration: {song.Duration.ToString("c")}");
                index++;
            }


            return sb.ToString().TrimEnd();
        }
    }
}
