namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context
                .Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls &&
                        t.Tickets.Count >= 20)
                .Select(t => new ExportTheatreDTO()
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                                      .Where(tc => tc.RowNumber >= 1 && tc.RowNumber <= 5)
                                      .Sum(tc => tc.Price),
                    Tickets = t.Tickets
                                .Where(tc => tc.RowNumber >= 1 && tc.RowNumber <= 5)
                                .Select(tc => new ExportTicketDTO()
                                {
                                    Price = tc.Price,
                                    RowNumber = tc.RowNumber
                                })
                                .OrderByDescending(tc => tc.Price)
                                .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportPlayDTO[]), xmlRoot);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            using StringWriter sw = new StringWriter(sb);

            ExportPlayDTO[] playsDTOs = context
                        .Plays
                        .ToArray()
                        .Where(p => p.Rating <= rating)
                        .Select(p => new ExportPlayDTO()
                        {
                            Title = p.Title,
                            Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),
                            Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                            Genre = p.Genre.ToString(),
                            Actors = p.Casts
                                        .Where(a => a.IsMainCharacter == true)
                                        .Select(a => new ExportCastDTO()
                                        {
                                            FullName = a.FullName,
                                            MainCharacter = a.IsMainCharacter ? $"Plays main character in '{p.Title}'." : ""
                                        })
                                        .OrderByDescending(p => p.FullName)
                                        .ToArray()
                        })
                        .OrderBy(p => p.Title)
                        .ThenByDescending(p => p.Genre)
                        .ToArray();

            serializer.Serialize(sw, playsDTOs, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
