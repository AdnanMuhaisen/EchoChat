using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.MessageAggregate;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace EchoChat.Infrastructure.DataAccess.Firebase.FirebaseStorage;

public class FirebaseStorageService : IFirebseStorageService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly string _imagesFolderName;
    private readonly string _videosFolderName;
    private readonly string _audioFolderName;
    private readonly string _textFilesFolderName;

    public FirebaseStorageService(IConfiguration configuration)
    {
        var credentials = GoogleCredential.FromFile(Path.Combine(Environment.CurrentDirectory, configuration["FirebaseSettings:FilePath"]!));
        _storageClient = StorageClient.Create(credentials);
        _bucketName = configuration["FirebaseSettings:FirebaseStorage:BucketName"]!;
        _imagesFolderName = configuration["FirebaseSettings:FirebaseStorage:ImagesFolderName"]!;
        _videosFolderName = configuration["FirebaseSettings:FirebaseStorage:VideosFolderName"]!;
        _audioFolderName = configuration["FirebaseSettings:FirebaseStorage:AudiosFolderName"]!;
        _textFilesFolderName = configuration["FirebaseSettings:FirebaseStorage:TextFilesFolderName"]!;
    }

    public async Task<MessageFile?> UploadFileAsync(string fileName, string fileAsBase64String, string contentType)
    {
        using var stream = new MemoryStream(Convert.FromBase64String(fileAsBase64String));

        try
        {
            var folderName = contentType.Split('/')[0] switch
            {
                "image" => _imagesFolderName,
                "video" => _videosFolderName,
                "audio" => _audioFolderName,
                "text" => _textFilesFolderName,
                _ => throw new NotSupportedException()
            };

            var uploadedObject = await _storageClient.UploadObjectAsync(
                _bucketName,
                $"{folderName}/{fileName}",
                contentType,
                stream,
                new UploadObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead });

            var uploadedFileUrl = $"https://storage.googleapis.com/{_bucketName}/{folderName}/{fileName}";

            return new MessageFile { Url = uploadedFileUrl, ContentType = contentType };
        }
        catch
        {
            return null;
        }
    }
}