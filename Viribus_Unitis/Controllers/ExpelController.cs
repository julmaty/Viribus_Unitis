using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Viribus_Unitis.Models;
using Viribus_Unitis.ViewModel;

namespace Viribus_Unitis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpelController : ControllerBase
    {
        private readonly ApplicationContext _context;
        EmailService _emailService;
        private readonly IConfiguration Configuration;
        public ExpelController(ApplicationContext context, EmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            Configuration = configuration;
        }
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var mailSettings = Configuration.GetSection("mailSettings");
            await _emailService.SendEmailAsync(mailSettings["DeanEmail"], "Отчисление из вуза", $"<div>Уважаемый декан! <br/><br/> Поступило обращение от студента.<br/><br/>Он хочет отчислиться. </div>");
            return Ok();
        }
    }
}
