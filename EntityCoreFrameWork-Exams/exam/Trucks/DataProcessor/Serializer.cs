namespace Trucks.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Despatchers");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportDespatchersDTO[]), xmlRoot);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            using StringWriter stringWriter = new StringWriter(sb);

            ExportDespatchersDTO[] despatchers = context.Despatchers
                                .ToArray()
                                .Where(d => d.Trucks.Any())
                                .Select(d => new ExportDespatchersDTO()
                                {
                                    DespatcherName = d.Name,
                                    TrucksCount = d.Trucks.Count,
                                    Trucks = d.Trucks
                                             .Select(t => new ExportDespatchersTrucksDTO()
                                             {
                                                 RegistrationNumber = t.RegistrationNumber,
                                                 Make = t.MakeType.ToString()
                                             })
                                             .OrderBy(t => t.RegistrationNumber)
                                             .ToArray()
                                })
                                .OrderByDescending(d => d.TrucksCount)
                                .ThenBy(d => d.DespatcherName)
                                .ToArray();

            serializer.Serialize(stringWriter, despatchers, namespaces);

            return sb.ToString().TrimEnd();
                            
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clientsWithMostTrucks = context
                            .Clients
                            .ToArray()
                            .Where(c => c.ClientsTrucks.Any(c => c.Truck.TankCapacity >= capacity))
                            .Select(c => new ExportClientDTO()
                            {
                                Name = c.Name,
                                Trucks = c.ClientsTrucks
                                         .Where(c => c.Truck.TankCapacity >= capacity)
                                         .Select(t => new ExportClientTrucksDTO()
                                         {
                                             TruckRegistrationNumber = t.Truck.RegistrationNumber,
                                             VinNumber = t.Truck.VinNumber,
                                             TankCapacity = t.Truck.TankCapacity,
                                             CargoCapacity = t.Truck.CargoCapacity,
                                             CategoryType = t.Truck.CategoryType.ToString(),
                                             MakeType = t.Truck.MakeType.ToString()
                                         })
                                         .OrderBy(t => t.MakeType)
                                         .ThenByDescending(t => t.CargoCapacity)
                                         .ToArray()
                            })
                            .OrderByDescending(c => c.Trucks.Count())
                            .ThenBy(c => c.Name)
                            .Take(10)
                            .ToArray();

            return JsonConvert.SerializeObject(clientsWithMostTrucks, Formatting.Indented);
        }
    }
}
