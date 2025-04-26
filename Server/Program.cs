using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.IO;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var listener = new TcpListener(ip, port);
listener.Start();
Console.WriteLine("Server Started...");

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var bw = new BinaryWriter(stream);
    var br = new BinaryReader(stream);

    using var db = new CarContext();

    while (true)
    {
        var input = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(input);

        switch (command!.Text)
        {
            case Command.Get:
                var allCars = db.Cars.ToList();
                bw.Write(JsonSerializer.Serialize(allCars));
                break;

            case Command.Post:
                var newCar = JsonSerializer.Deserialize<Car>(command.Param);
                db.Cars.Add(newCar!);
                db.SaveChanges();
                bw.Write("Car added successfully.");
                break;

            case Command.Put:
                var updatedCar = JsonSerializer.Deserialize<Car>(command.Param);
                var carToUpdate = db.Cars.FirstOrDefault(c => c.Id == updatedCar!.Id);

                if (carToUpdate != null)
                {
                    carToUpdate.Brand = updatedCar.Brand;
                    carToUpdate.Model = updatedCar.Model;
                    carToUpdate.Year = updatedCar.Year;
                    carToUpdate.Price = updatedCar.Price;
                    carToUpdate.Colors = updatedCar.Colors;
                    carToUpdate.Currency = updatedCar.Currency;

                    db.SaveChanges();
                    bw.Write("Car updated successfully.");
                }
                else
                {
                    bw.Write("Car not found.");
                }
                break;

            case Command.Delete:
                var deleteCar = JsonSerializer.Deserialize<Car>(command.Param);
                var carToDelete = db.Cars.FirstOrDefault(c => c.Id == deleteCar!.Id);

                if (carToDelete != null)
                {
                    db.Cars.Remove(carToDelete);
                    db.SaveChanges();
                    bw.Write("Car deleted successfully.");
                }
                else
                {
                    bw.Write("Car not found.");
                }
                break;

            default:
                bw.Write("Invalid command.");
                break;
        }
    }

}
