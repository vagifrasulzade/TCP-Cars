using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

Command command = null!;
string response = null!;

while (true)
{
    Console.WriteLine(@"------Commands list------
GET - Show all cars
POST - Add new car
Put - Update car
Delete - Delete car
EXIT - Exit the program

");
    Console.Write("Enter command name:");
    string input = Console.ReadLine()!.ToUpper();

    if (input == "EXIT")
    {
        Console.WriteLine("Exiting the program...");
        break; 
    }

    
    switch (input)
    {
        case Command.Get:
            command = new Command { Text = Command.Get };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            var cars = JsonSerializer.Deserialize<List<Car>>(response);
            Console.WriteLine("Cars in database:");
            cars!.ForEach(c => Console.WriteLine($@"
Id: {c.Id}
Brand: {c.Brand}
Model: {c.Model}
Year: {c.Year}
Color: {c.Colors}
Price: {c.Price}
Currency: {c.Currency}
"));
            break;

        case Command.Post:
            Console.Write("Enter the brand of the car: ");
            string? brand = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(brand) || brand.Length > 20 || brand.Length < 3)
            {
                Console.WriteLine("Enter the size of Brand correctly!");
                return;
            }
            brand = char.ToUpper(brand[0]) + brand.Substring(1);

            Console.Write("Enter the model of the car: ");
            string? model = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(model) || model.Length > 20 || model.Length < 2)
            {
                Console.WriteLine("Enter the size of Model correctly!");
                return;
            }
            model = char.ToUpper(model[0]) + model.Substring(1);





            Console.Write("Enter the year of the car: ");
            int year;
            int.TryParse(Console.ReadLine(), out year);

            Console.Write(@"Choice Color
1. Black
2. White
3. Blue
4. Red
5. Grey
6. Yellow
");
            Console.Write("Enter the color of the car: ");
            int colorInput;
            int.TryParse(Console.ReadLine(), out colorInput);

            Console.Write("Enter the price of the car: ");
            decimal price;
            decimal.TryParse(Console.ReadLine(), out price);

            Console.Write(@"Choice Currency
1. USD
2. EUR
3. AZN
4. GBP
5. TRY
");
            Console.Write("Enter the currency of the car: ");
            int currencyInput;
            int.TryParse(Console.ReadLine(), out currencyInput);
            Currency currency = (Currency)currencyInput;


            Car newCar = new Car
            {
                Brand = brand,
                Model = model,
                Price = price,
                Year = year,
                Colors = (Color)colorInput,
                Currency = (Currency)currencyInput

            };

            command = new Command
            {
                Text = Command.Post,
                Param = JsonSerializer.Serialize(newCar)
            };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            break;

        case Command.Put:
            
            Console.Write("Enter the id of the car: ");
            int updateId;
            int.TryParse(Console.ReadLine(), out updateId);

            Console.Write("Enter the new brand of the car: ");
            string? newBrand = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newBrand) || newBrand.Length > 20 || newBrand.Length < 3)
            {
                Console.WriteLine("Enter the size of Brand correctly!");
                return;
            }
            newBrand = char.ToUpper(newBrand[0]) + newBrand.Substring(1);

            Console.Write("Enter the new model of the car: ");
            string? newModel = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newModel) || newModel.Length > 20 || newModel.Length < 2)
            {
                Console.WriteLine("Enter the size of Model correctly!");
                return;
            }
            newModel = char.ToUpper(newModel[0]) + newModel.Substring(1);

            Console.Write("Enter the new year of the car: ");
            int newYear;

            int.TryParse(Console.ReadLine(), out newYear);

            Console.Write(@"Choice New Color
1. Black
2. White
3. Blue
4. Red
5. Grey
6. Yellow
");
            Console.Write("Enter the new color of the car: ");
            int newColorInput;
            int.TryParse(Console.ReadLine(), out newColorInput);

            Console.Write("Enter the new price of the car: ");
            decimal newPrice;
            decimal.TryParse(Console.ReadLine(), out newPrice);

            Console.Write(@"Choice New Currency
1. USD
2. EUR
3. AZN
4. GBP
5. TRY
");
            Console.Write("Enter the new currency of the car: ");
            int newCurrencyInput;
            int.TryParse(Console.ReadLine(), out newCurrencyInput);

            Car updatedCar = new Car
            {
                Id = updateId,
                Brand = newBrand,
                Model = newModel,
                Price = newPrice,
                Year = newYear,
                Colors = (Color)newColorInput,
                Currency = (Currency)newCurrencyInput

            };

            command = new Command
            {
                Text = Command.Put,
                Param = JsonSerializer.Serialize(updatedCar)
            };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            break;

        case Command.Delete:
            Console.Write("Enter the ID of the car to delete: ");
            int deleteId;
            int.TryParse(Console.ReadLine(), out deleteId);

            Car deleteCar = new Car
            {
                Id = deleteId
            };

            command = new Command
            {
                Text = Command.Delete,
                Param = JsonSerializer.Serialize(deleteCar)
            };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            break;

        default:
            Console.WriteLine("Invalid choice");
            break;
    }
}
