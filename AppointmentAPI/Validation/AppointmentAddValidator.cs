using AppointmentAPI.Dto;
using FluentValidation;

namespace AppointmentAPI.Validation;

public class AppointmentAddValidator : AbstractValidator<AppointmentAddDto>
{
    public AppointmentAddValidator()
    {
        var startHour = new DateTime(2000, 01, 01, 08, 00, 00);
        var endHour = new DateTime(2000, 01, 01, 17, 00, 00);
        RuleFor(p => p.Date.Value.Hour).InclusiveBetween(startHour.Hour, endHour.Hour)
            .WithMessage("Yalnızca belirlenen saatler arasında randevu alınabilir. {From}-{To}");
    }
}
