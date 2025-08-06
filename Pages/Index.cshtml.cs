using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace KiafricaLLC.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public ContactFormModel ContactForm { get; set; }

    public string ResultMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ResultMessage = "Please fill all fields correctly.";
            return Page();
        }

        try
        {
            var toAddress = "mahabubsikder751975@gmail.com"; // Replace with your email

            var subject = $"New Contact Message from {ContactForm.Name}";

            var body = $"Name: {ContactForm.Name}\n" +
            $"Email: {ContactForm.Email}\n" +
            $"Phone: {ContactForm.Phone}\n" +
            $"Service: {ContactForm.ServiceItem}\n\n" +
            $"Message:\n{ContactForm.Message}";

            var mail = new MailMessage
            {
                From = new MailAddress(ContactForm.Email),
                Subject = subject,
                Body = body
            };
            
            mail.To.Add(toAddress);

            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("mahabubsikder751975@gmail.com", "abpi clnj tvav rgzg");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }

            ResultMessage = "Thank you! Your message has been sent.";
            ModelState.Clear();
            ContactForm = new ContactFormModel(); // Clear form
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email.");
            ResultMessage = "Sorry, something went wrong. Please try again.";
        }

        return Page();
    }

    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string ServiceItem { get; set; }
    }
}


// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;

// namespace KiafricaLLC.Pages;

// public class IndexModel : PageModel
// {
//     private readonly ILogger<IndexModel> _logger;

//     public IndexModel(ILogger<IndexModel> logger)
//     {
//         _logger = logger;
//     }

//     public void OnGet()
//     {

//     }
// }
