using FluentValidation;

namespace LabEquipmentApi.Contracts.Equipment;

public class CreateEquipmentRequestValidator : AbstractValidator<CreateEquipmentRequest>
{
    public CreateEquipmentRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().MaximumLength(100);
        RuleFor(request => request.Room).NotEmpty().MaximumLength(20);
    }
}
