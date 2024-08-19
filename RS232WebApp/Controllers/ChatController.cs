using Microsoft.AspNetCore.Mvc;
using RS232WebApp.Models;


namespace RS232WebApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly RS232Service _rs232Service;

        public ChatController(RS232Service rs232Service)
        {
            _rs232Service = rs232Service;
        }

        public IActionResult Index()
        {
            var model = new ChatViewModel
            {
                Messages = new List<MessageViewModel>()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SendMessage(string messageContent)
        {
            // Veriyi COM1'e gönder
            _rs232Service.SendMessageToPort1(messageContent);

            // COM3'ten alınan veriyi al ve göster
            var receivedMessage = _rs232Service.ReceiveMessageFromPort2();

            var model = new ChatViewModel
            {
                Messages = new List<MessageViewModel>
                {
                    new MessageViewModel { Content = messageContent, IsSentByUser = true },
                    new MessageViewModel { Content = receivedMessage, IsSentByUser = false }
                }
            };
            return View("Index", model);
        }
    }
}
