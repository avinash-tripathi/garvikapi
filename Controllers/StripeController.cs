using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GARVIKService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace GARVIKService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private IConfiguration _config;
        public StripeController(IConfiguration config)
        {
            _config = config;
            StripeConfiguration.ApiKey = "sk_test_51Nc8teSCPyqIiJIsUPOqn28jfqPAtSEtZy5KkO3FBtmAR7ROmFFSdcm9OJEYVnTiq8XMj03bxlB90codDXuXHmC000Vn5JlzFu";
       }

        [HttpGet("/order/success")]
        public ActionResult OrderSuccess([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            var customerService = new CustomerService();
            Stripe.Customer customer = customerService.Get(session.CustomerId);

            return Content($"<html><body><h1>Thanks for your order, {customer.Name}!</h1></body></html>");
        }

    }
}
