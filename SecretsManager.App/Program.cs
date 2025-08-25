using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace SecretsManager.App;

internal class Program
{
    static async Task Main(string[] args)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var client = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);

        var request = new GetSecretValueRequest
        {
            SecretId = "ApiKey2"
        };

        try
        {
            Console.WriteLine($"Getting the secret at {DateTime.Now}");

            var response = await client.GetSecretValueAsync(request, cts.Token);

            Console.WriteLine($"Secret retrieved: {response.SecretString} at {DateTime.Now}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error retrieving secret: " + e.Message);
        }
    }
}