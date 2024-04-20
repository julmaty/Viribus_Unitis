using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Viribus_Unitis;
using Viribus_Unitis.Models;
using Viribus_Unitis.ViewModel;

namespace Viribus_Unitis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RequestsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.Include(u => u.Description).Include(u => u.Tags).ToListAsync();
        }
        [HttpGet("ofuser/{id}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsOfUser(int id)
        {
            return await _context.Requests.Include(u => u.Description).Include(u => u.Tags).Where(u => u.UserId == id).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = _context.Requests.Include(u => u.Description).Include(u => u.Tags).Where(u => u.Id == id).FirstOrDefault();

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(RequestViewModel requestModel)
        {
            Request request = new Request { Name = requestModel.Name, UserId = requestModel.UserId, CreatedDate = DateTime.Now };
            if (requestModel.Token != null && requestModel.Token != "")
            {
                string email = await GetEmail(requestModel);
                request.Email = email;
            }
            _context.Requests.Add(request);
            Description description = new Description { Text = requestModel.Description, Request=request };
            _context.Descriptions.Add(description);
            foreach (int id in requestModel.Tags)
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag != null)
                {
                    tag.Requests.Add(request);
                    request.Tags.Add(tag);
                }
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
        public async Task<string> GetEmail(RequestViewModel request)
        {

            string responseString = "";
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
