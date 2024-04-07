using MassTransit;

using Microsoft.EntityFrameworkCore;

using Musdis.FileService.Data;
using Musdis.FileService.Exceptions;
using Musdis.MessageBrokerHelpers.Events;

namespace Musdis.FileService.MessageBroker.Consumers;

/// <summary>
///     Represents a consumer for the <see cref="FileUsed"/> event.
/// </summary>
public sealed class FileUsedConsumer : IConsumer<FileUsed>
{
    private readonly FileServiceDbContext _dbContext;

    public FileUsedConsumer(FileServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<FileUsed> context)
    {
        var file = await _dbContext.FilesMetadata
            .FirstOrDefaultAsync(x => x.Id == context.Message.Id) 
            ?? throw new InvalidMethodCallException("File is not found. Cannot change its usage status.");
        
        file.IsUsed = true;
        await _dbContext.SaveChangesAsync();
    }
}