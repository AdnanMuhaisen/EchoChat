using EchoChat.Core.Application.Abstractions.Firestore;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore.V1;

namespace EchoChat.Infrastructure.DataAccess.Firebase.Firestore.Factories;

public class FirestoreClientFactory(IConfiguration configuration) : IFirestoreClientFactory
{
    public FirestoreClient GetClient()
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, configuration["FirebaseSettings:FilePath"]!);
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        GoogleCredential credential = GoogleCredential.FromStream(stream);
        FirestoreClientBuilder builder = new()
        {
            CredentialsPath = filePath
        };

        return builder.Build();
    }
}