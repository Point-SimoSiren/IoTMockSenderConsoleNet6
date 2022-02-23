// See https://aka.ms/new-console-template for more information
using IoTMockSender;
using Newtonsoft.Json;
using System.Text;

//"deviceId":4,"time":"2022-02-20T00:00:00","temperature":-4.32,"humidity":43.32,"pressure":1008.22,"command":null,"optional":null

Console.WriteLine("Anna lämpötila");
double temp = double.Parse(Console.ReadLine());

Console.WriteLine("Anna kosteus%");
double hum = double.Parse(Console.ReadLine());

Console.WriteLine("Anna ilmanpaine");
double press = double.Parse(Console.ReadLine());


Console.WriteLine("Anna komentona laitenumero <space> on tai off esim. *1 on* tai *1 off*");
Console.WriteLine("Jos haluat ohittaa paina x");

string? command = null;
string c = Console.ReadLine().ToLower();

if (c == "x") {
        Console.WriteLine("No command this time.");
    }
else {
    command = c;
}

Measurement obj = new Measurement()
{
    DeviceId = 2,
    Temperature = temp,
    Humidity = hum,
    Pressure = press,
    Time = DateTime.Now,
    Command = command
};


HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://iottemperature1001.azurewebsites.net/");


// Muutetaan em. data objekti Jsoniksi
string input = JsonConvert.SerializeObject(obj);
StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

// Lähetetään serialisoitu objekti back-endiin Post pyyntönä
HttpResponseMessage message = await client.PostAsync("/api/measurements", content);


// Otetaan vastaan palvelimen vastaus
string reply = await message.Content.ReadAsStringAsync();

Console.WriteLine(reply);

// Stop
Console.Read();
