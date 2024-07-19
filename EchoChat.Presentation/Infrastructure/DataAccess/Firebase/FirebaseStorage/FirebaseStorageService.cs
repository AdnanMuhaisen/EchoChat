using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.Common.Requirements;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.CodeAnalysis;
using System.Text;

namespace EchoChat.Infrastructure.DataAccess.Firebase.FirebaseStorage;

public class FirebaseStorageService : IFirebseStorageService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public FirebaseStorageService(IConfiguration configuration)
    {
        var credentials = GoogleCredential.FromFile(Path.Combine(Environment.CurrentDirectory, configuration["FirebaseSettings:FilePath"]!));
        _storageClient = StorageClient.Create(credentials);
        _bucketName = configuration["FirebaseSettings:FirebaseStorage:bucketName"]!;
    }

    public async Task UploadFileAsync(string fileName, string fileAsBase64String, string contentType)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileAsBase64String));

        var bucket = _storageClient.CreateBucket(FirestoreRequirements.ProjectId, _bucketName);

        var uploadedObject = await _storageClient.UploadObjectAsync(_bucketName, fileName, contentType, stream);
        var dbg = uploadedObject.Id;
    }
}