using EchoChat.Application.Abstractions.Firestore;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore.V1;
using Microsoft.Extensions.Configuration;

namespace EchoChat.Infrastructure.Services.Firestore;

public class FirestoreClientFactory(IConfiguration configuration) : IFirestoreClientFactory
{
    public FirestoreClient GetClient()
    {
        var filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, $@"EchoChat.Infrastructure\DataAccess\Firebase\{configuration["FirebaseSettings:FileName"]}");
        
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        GoogleCredential credential = GoogleCredential.FromStream(stream);
        FirestoreClientBuilder builder = new()
        {
            CredentialsPath = filePath
        };

        return builder.Build();
    }
}