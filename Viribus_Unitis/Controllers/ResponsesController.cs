using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Viribus_Unitis;
using Viribus_Unitis.Models;
using Viribus_Unitis.ViewModel;

namespace Viribus_Unitis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        EmailService _emailService;

        public ResponsesController(ApplicationContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/Responses
        [HttpGet("ofrequest/{id}")]
        public async Task<ActionResult<IEnumerable<Response>>> GetResponsesOfRequest(int id)
        {
            return await _context.Responses.Include(u => u.Request).Where(u => u.RequestId == id).ToListAsync();
        }
        [HttpGet("ofuser/{id}")]
        public async Task<ActionResult<IEnumerable<Response>>> GetResponsesOfUser(int id)
        {
            return await _context.Responses.Include(u => u.Request).Where(u => u.UserId == id).ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Response>>> GetResponses()
        {
            return await _context.Responses.Include(u => u.Request).ToListAsync();
        }

        // GET: api/Responses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetResponse(int id)
        {
            var response = _context.Responses.Include(u => u.Request).Where(u => u.Id==id).FirstOrDefault();

            if (response == null)
            {
                return NotFound();
            }

            return response;
        }

        // PUT: api/Responses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponse(int id, Response response)
        {
            if (id != response.Id)
            {
                return BadRequest();
            }

            _context.Entry(response).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(id))
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

        // POST: api/Responses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Response>> PostResponse(ResponseViewModel responseModel)
        {
            Response response = new Response { Comment = responseModel.Comment, Phone=responseModel.Phone, UserId= responseModel.UserId };
            var request = await _context.Requests.FindAsync(responseModel.RequestId);
            if (request != null)
            {
                response.Request = request;
            }

            _context.Responses.Add(response);
            request.Responses.Add(response);
            await _context.SaveChangesAsync();
            await _emailService.SendEmailAsync("yorkshirelove@yandex.ru", "Отклик на просьбу", $"<div> Поступил отклик на вашу просьбу от {request.Name}!<br/><br/>{response.Comment} <br/><br/> Подробности на сайте. </div>");
            return CreatedAtAction("GetResponse", new { id = response.Id }, response);
        }

        // DELETE: api/Responses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse(int id)
        {
            var response = await _context.Responses.FindAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            _context.Responses.Remove(response);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponseExists(int id)
        {
            return _context.Responses.Any(e => e.Id == id);
        }

        public async Task<ActionResult<string>> GetEmail(RequestViewModel request)
        {

            string responseString ="";
            using (var client = new HttpClient())

            {
                client.DefaultRequestHeaders.Add("Authorization", request.Token);
                var obj = new
                {
                    requestMessage = request.UserId
                };

                var response = await client.PostAsJsonAsync("https://api.test.profcomff.com/auth/me", obj);

                Me? me = await response.Content.ReadFromJsonAsync<Me>();
                responseString = me?.Email;

            }

            return responseString;
        }
    }
}
