using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FieldEngineerApi.Models;

namespace FieldEngineerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public AppointmentsController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
          if (_context.Appointments == null)
          {
              return NotFound();
          }
            return await _context.Appointments
                .Include(c => c.Customer)
                .Include(e => e.Engineer)
                .Include(s => s.AppointmentStatus)
                .ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(long id)
        {
          if (_context.Appointments == null)
          {
              return NotFound();
          }
            // var appointment = await _context.Appointments.FindAsync(id);

            // if (appointment == null)
            // {
            //     return NotFound();
            // }

            // return appointment;
            var appointment = _context.Appointments
                .Where(a => a.Id == id)
                .Include(c => c.Customer)
                .Include(e => e.Engineer)
                .Include(s => s.AppointmentStatus);

            var appData = await appointment.FirstOrDefaultAsync();

            if (appData == null)
            {
                return NotFound();
            }

            return appData;
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        // public async Task<IActionResult> PutAppointment(long id, Appointment appointment)
        // {
        //     if (id != appointment.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(appointment).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!AppointmentExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }
        public async Task<IActionResult> PutAppointment(long id, string problemDetails, string statusName, string notes, string imageUrl)
        {
            if (_context.AppointmentStatuses == null)
            {
                return NotFound();
            }
            var statusId = _context.AppointmentStatuses.First(s => s.StatusName == statusName).Id;
            
            if (_context.Appointments
                == null)
            {
                return NotFound();
            }
            var appointment =  _context.Appointments.First(e => e.Id == id);
            
            if (appointment == null)
            {
                return BadRequest();
            }

            appointment.ProblemDetails = problemDetails;
            appointment.AppointmentStatusId = statusId;
            appointment.Notes = notes;
            appointment.ImageUrl = imageUrl;
            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
          if (_context.Appointments == null)
          {
              return Problem("Entity set 'ScheduleContext.Appointments'  is null.");
          }
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(long id)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(long id)
        {
            return (_context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}