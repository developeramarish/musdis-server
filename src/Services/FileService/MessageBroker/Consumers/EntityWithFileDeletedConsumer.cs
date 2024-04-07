using MassTransit;

using Musdis.FileService.Services.Storage;
using Musdis.MessageBrokerHelpers.Events;

namespace Musdis.FileService.MessageBroker.Consumers;

public sealed class EntityWithFileDeletedConsumer : IConsumer<EntityWithFileDeleted>
{
    private readonly IStorageService _storageService;

    public EntityWithFileDeletedConsumer(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task Consume(ConsumeContext<EntityWithFileDeleted> context)
    {
        await _storageService.DeleteFileAsync(context.Message.FileId);
    }
}