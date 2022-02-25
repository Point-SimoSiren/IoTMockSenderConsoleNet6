// See https://aka.ms/new-console-template for more information
using IoTMockSender;
using Newtonsoft.Json;
using System.Text;

HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://iottemperature1001.azurewebsites.net/");
//client.BaseAddress = new Uri("https://localhost:44302/");

SILMUKKA:
Console.WriteLine("Simuloidaksesi ilmadatan lähetystä kirjoita a.");
Console.WriteLine("Antaaksesi ohjauskomennon IoT laitteelle kirjoita b.");
Console.WriteLine("Simuloidaksesi IoT laitteen tekemää vallitsevan komennon tarkistusta kirjoita c.");
Console.WriteLine("Vahvista painamalla enter.");

string valinta = Console.ReadLine();

if (valinta.ToLower() == "a")
{

    Console.WriteLine("Anna lämpötila");
    double temp = double.Parse(Console.ReadLine());

    Console.WriteLine("Anna kosteus%");
    double hum = double.Parse(Console.ReadLine());

    Console.WriteLine("Anna ilmanpaine");
    double press = double.Parse(Console.ReadLine());

    Measurement obj = new Measurement()
    {
        DeviceId = 2,
        Temperature = temp,
        Humidity = hum,
        Pressure = press,
        Time = DateTime.Now
    };


    // Muutetaan em. data objekti Jsoniksi
    string input = JsonConvert.SerializeObject(obj);
    StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

    // Lähetetään serialisoitu objekti back-endiin Post pyyntönä
    HttpResponseMessage message = await client.PostAsync("/api/measurements", content);


    // Otetaan vastaan palvelimen vastaus
    string reply = await message.Content.ReadAsStringAsync();

    Console.WriteLine(reply);
}
else if (valinta.ToLower() == "b")
{
    Console.WriteLine("Anna komento");

    Command c = new Command();
    c.Cmd = Console.ReadLine().ToLower();

    // Muutetaan em. data objekti Jsoniksi
    string input = JsonConvert.SerializeObject(c);
    StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

    // Lähetetään serialisoitu objekti back-endiin Post pyyntönä
    HttpResponseMessage message = await client.PostAsync("/api/measurements/command", content);


    // Otetaan vastaan palvelimen vastaus
    string reply = await message.Content.ReadAsStringAsync();

    Console.WriteLine(reply);
}

else
{

    // Komennon lukeminen get pyynnöllä
    Console.WriteLine("Katsotaan onko IoT laitteelle komentoja");

    HttpResponseMessage commandMsg = await client.GetAsync("/api/measurements/command");

    // Otetaan vastaan palvelimen vastaus
    string com = await commandMsg.Content.ReadAsStringAsync();

    Console.WriteLine("Komentotieto vastaanotettu tunnisteella: " + com);

}

Console.WriteLine("Haluatko jatkaa ohjelman käyttämistä? y/n");
string jatko = Console.ReadLine();

if (jatko.ToLower() == "y")
{
    goto SILMUKKA;
}


