namespace EchoChat.Core.Application.Abstractions.Firestore;

public interface IFirebseStorageService
{
    Task UploadFileAsync(string fileName, string fileAsBase64String, string contentType);
}