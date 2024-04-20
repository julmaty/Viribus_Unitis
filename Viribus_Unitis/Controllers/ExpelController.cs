using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ExpelController(ApplicationContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            await _emailService.SendEmailAsync("yorkshirelove@yandex.ru", "Отчисление из вуза", $"<div>Уважаемый декан! <br/><br/> Поступило обращение от студента.<br/><br/>Он хочет отчислиться. </div>");
            return Ok();
        }
    }
}
