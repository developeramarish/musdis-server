using MassTransit;

using Musdis.FileService.MessageBroker.Commands;
using Musdis.FileService.Services.Storage;

namespace Musdis.FileService.MessageBroker.Consumers;

/// <summary>
///     Represents a consumer for the <see cref="DeleteFileScheduled"/> command.
/// </summary>
public sealed class DeleteFileScheduledConsumer : IConsumer<DeleteFileScheduled>
{
    private readonly IStorageService _storageService;

    public DeleteFileScheduledConsumer(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task Consume(ConsumeContext<DeleteFileScheduled> context)
    {
        var id = context.Message.Id;
        var fileResult = await _storageService.GetFileMetadataAsync(id);
        if (fileResult.IsFailure || fileResult.Value.IsUsed)
        {
            return;
        }

        await _storageService.DeleteFileAsync(id);
    }
}