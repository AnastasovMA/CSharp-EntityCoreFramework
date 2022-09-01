namespace Trucks.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Despatchers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportDespatcherDTO[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportDespatcherDTO[] despatcherDTOs = (ImportDespatcherDTO[])xmlSerializer.Deserialize(sr);

            HashSet<Despatcher> despatchers = new HashSet<Despatcher>();

            foreach (var despatcherDTO in despatcherDTOs)
            {
                if (!IsValid(despatcherDTO) || string.IsNullOrEmpty(despatcherDTO.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher currentDespatcher = new Despatcher()
                {
                    Name = despatcherDTO.Name,
                    Position = despatcherDTO.Position
                };

                HashSet<Truck> trucks = new HashSet<Truck>();

                foreach (var truckDTO in despatcherDTO.Trucks)
                {
                    if (!IsValid(truckDTO))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck currentTruck = new Truck()
                    {
                        RegistrationNumber = truckDTO.RegistrationNumber,
                        VinNumber = truckDTO.VinNumber,
                        TankCapacity = truckDTO.TankCapacity,
                        CargoCapacity = truckDTO.CargoCapacity,
                        CategoryType = (CategoryType)truckDTO.CategoryType,
                        MakeType = (MakeType)truckDTO.MakeType
                    };

                    trucks.Add(currentTruck);
                }

                currentDespatcher.Trucks = trucks;
                despatchers.Add(currentDespatcher);
                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, currentDespatcher.Name, trucks.Count));
            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportClientDTO[] clientDTOs = JsonConvert.DeserializeObject<ImportClientDTO[]>(jsonString);

            var existingTrucksIds = context.Trucks.Select(p => p.Id).ToHashSet();

            HashSet<Client> clients = new HashSet<Client>();

            foreach (var clientDTO in clientDTOs)
            {
                if (!IsValid(clientDTO) || clientDTO.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client currentClient = new Client()
                {
                    Name = clientDTO.Name,
                    Nationality = clientDTO.Nationality,
                    Type = clientDTO.Type
                };

                HashSet<ClientTruck> clientTrucks = new HashSet<ClientTruck>();

                foreach (var truckId in clientDTO.Trucks.Distinct())
                {
                    Truck truck = context.Trucks.Find(truckId);

                    if (truck == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck currentClientTruck = new ClientTruck()
                    {
                        TruckId = truckId,
                        Client = currentClient
                    };

                    currentClient.ClientsTrucks.Add(currentClientTruck);
                }

                clients.Add(currentClient);

                sb.AppendLine(String.Format(SuccessfullyImportedClient, currentClient.Name, currentClient.ClientsTrucks.Count()));
            }

            context.Clients.AddRange(clients);
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
