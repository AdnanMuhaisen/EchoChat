using EchoChat.Core.Domain.MessageAggregate;

namespace EchoChat.Core.Application.Abstractions.Firestore;

public interface IFirebseStorageService
{
    Task<MessageFile?> UploadFileAsync(string fileName, string fileAsBase64String, string contentType);
}