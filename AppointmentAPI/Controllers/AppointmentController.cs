using AppointmentAPI.Contexts;
using AppointmentAPI.Dto;
using AppointmentAPI.Entities;
using AppointmentAPI.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly AppointmentDbContext _dbContext;

    public AppointmentController(AppointmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{appointmentId}")]
    public async Task<IActionResult> GetById(Guid appointmentId)
    {
        var appointment = await _dbContext.Appointments.Where(p => !p.IsDeleted && p.Id == appointmentId)
            .Select(p => new
            {
                p.Id,
                p.FullName,
                p.Date,
                p.Email,
                p.Phone,
                p.Description,
                p.CreatedDate,
                p.ModifiedDate
            }).FirstOrDefaultAsync();
        return Ok(appointment);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var appointment = await _dbContext.Appointments.Where(p => !p.IsDeleted).Select(p =>
        new
        {
            p.Id,
            p.FullName,
            p.Date,
            p.Email,
            p.Phone,
            p.Description,
            p.CreatedDate,
            p.ModifiedDate
        }).ToListAsync();
        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AppointmentAddDto appointmentAddDto)
    {
        var validator = new AppointmentAddValidator();
        var validationResult = await validator.ValidateAsync(appointmentAddDto);
        var list = new Lazy<List<string>>();
        if (!validationResult.IsValid)
        {
            foreach (var validationErrors in validationResult.Errors)
            {
                list.Value.Add(validationErrors.ErrorMessage);
                return Ok(new { code = StatusCode(1002), message = list.Value, type = "error" });
            }
        }

        var newAppointment = new Appointment
        {
            FullName = appointmentAddDto.FullName,
            Date = appointmentAddDto.Date,
            Description = appointmentAddDto.Description,
            Phone = appointmentAddDto.Phone,
            Email = appointmentAddDto.Email
        };
        await _dbContext.Appointments.AddAsync(newAppointment);
        var result = await _dbContext.SaveChangesAsync();
        switch (result)
        {
            case > 0:
                list.Value.Add("Ekleme işlemi başarılı.");
                return Ok(new { code = StatusCode(1000), message = list.Value, type = "success" });
            case 0:
                list.Value.Add("Ekleme işlemi başarısız.");
                return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
            default:
                list.Value.Add("İşlem sırasında hata meydana geldi.");
                return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(AppointmentUpdateDto appointmentUpdateDto)
    {
        var validator = new AppointmentUpdateValidator();
        var validationResult = await validator.ValidateAsync(appointmentUpdateDto);
        var list = new Lazy<List<string>>();
        if (!validationResult.IsValid)
        {
            foreach (var validationErrors in validationResult.Errors)
            {
                list.Value.Add(validationErrors.ErrorMessage);
                return Ok(new { code = StatusCode(1002), message = list.Value, type = "error" });
            }
        }
        var currentAppointment = await _dbContext.Appointments.Where(p => !p.IsDeleted && p.Id == appointmentUpdateDto.Id).FirstOrDefaultAsync();
        if (currentAppointment is null)
        {
            list.Value.Add("Müracaat bulunamadı.");
            return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
        }
        _dbContext.Appointments.Attach(currentAppointment);
        currentAppointment.FullName = appointmentUpdateDto.FullName;
        currentAppointment.Date = appointmentUpdateDto.Date;
        currentAppointment.Description = appointmentUpdateDto.Description;
        currentAppointment.Email = appointmentUpdateDto.Email;
        currentAppointment.Phone = appointmentUpdateDto.Phone;
        currentAppointment.ModifiedDate = DateTime.Now;
        var result = await _dbContext.SaveChangesAsync();
        switch (result)
        {
            case > 0:
                list.Value.Add("Güncelleme işlemi başarılı.");
                return Ok(new { code = StatusCode(1000), message = list.Value, type = "success" });
            case 0:
                list.Value.Add("Güncelleme işlemi başarısız.");
                return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
            default:
                list.Value.Add("İşlem sırasında hata meydana geldi.");
                return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var currentAppointment = await _dbContext.Appointments.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
        var list = new Lazy<List<string>>();
        if (currentAppointment is null)
        {
            list.Value.Add("Müracaat bulunamadı.");
            return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
        }
        _dbContext.Appointments.Attach(currentAppointment);
        currentAppointment.IsDeleted = true;
        currentAppointment.ModifiedDate = DateTime.Now;
        var result = await _dbContext.SaveChangesAsync();
        switch (result)
        {
            case > 0:
                list.Value.Add("Silme işlemi başarılı.");
                return Ok(new { code = StatusCode(1000), message = list.Value, type = "success" });
            case 0:
                list.Value.Add("Silme işlemi başarısız.");
                return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
            default:
                list.Value.Add("İşlem sırasında hata meydana geldi.");
                return Ok(new { code = StatusCode(1001), message = list.Value, type = "error" });
        }
    }
}