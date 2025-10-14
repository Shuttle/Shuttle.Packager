using Microsoft.Extensions.Options;

namespace Shuttle.Packager.WebApi;

public class PackagerOptionsValidator : IValidateOptions<PackagerOptions>
{
    public ValidateOptionsResult Validate(string? name, PackagerOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseFolder))
        {
            return ValidateOptionsResult.Fail("Option 'BaseFolder' may not be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.VisualStudioPath))
        {
            return ValidateOptionsResult.Fail("Option 'VisualStudioPath' may not be empty.");
        }

        return ValidateOptionsResult.Success;
    }
}