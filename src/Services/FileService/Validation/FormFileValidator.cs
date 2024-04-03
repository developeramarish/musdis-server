using FluentValidation;

using Musdis.FileService.Utils;

namespace Musdis.FileService.Validation;

/// <summary>
///     The validator for the <see cref="IFormFile"/>.
/// </summary>
public sealed class FormFileValidator : AbstractValidator<IFormFile>
{
    public FormFileValidator()
    {
        RuleFor(x => x.FileName).Must(x =>
        {
            var ext = Path.GetExtension(x);
            return FileHelper.IsExtensionSupported(ext);
        }).WithMessage("File type is not supported.");
    }
}