using CoreDisplay.Shared.DTOs;
using FluentValidation;

namespace CoreDisplay.Application.Validators;

public class DeviceRegisterValidator : AbstractValidator<DeviceRegisterDto>
{
    public DeviceRegisterValidator()
    {
        RuleFor(x => x.HardwareId)
            .NotEmpty().WithMessage("HardwareId is required.")
            .MaximumLength(100).WithMessage("HardwareId must not exceed 100 characters.")
            .Matches("^[a-zA-Z0-9-]+$").WithMessage("HardwareId can only contain alphanumeric characters and dashes.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Device Name is required.")
            .MaximumLength(100);

        RuleFor(x => x.OS)
            .NotEmpty()
            .MaximumLength(50);
            
        RuleFor(x => x.AppVersion)
            .NotEmpty()
            .Matches(@"^\d+\.\d+\.\d+$").WithMessage("AppVersion must be in format X.Y.Z");
    }
}
