using ABXExchangeClient.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new AbxClient();
        Console.WriteLine("Connecting to ABX Exchange Server...");

        var packets = await client.FetchAllPacketsAsync();
        Console.WriteLine($"Fetched {packets.Count} packets.");

        string outputFile = "output.json";
        await client.SaveToJsonAsync(packets, outputFile);

        Console.WriteLine($"Saved to {outputFile}");
    }
}
