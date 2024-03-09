using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using GARVIKService.Model;
using GARVIKService.Model.stripe;

namespace GARVIKService.Controllers
{
    [Route("stripe-instance")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        [HttpGet]
        public ActionResult Get(String accountcode)
        {
            var domain = "http://localhost:5000";
            var options = new SessionCreateOptions
            {
                
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = "price_1NcBBSSCPyqIiJIsUY6WePiN",
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "#/success",
                CancelUrl = domain + "#/cancel"
               
            };
            var service = new SessionService();
            var requestOptions = new RequestOptions();
            requestOptions.StripeAccount = "acct_1Nc8teSCPyqIiJIs";

            Session session = service.Create(options);
            session.Metadata.Add("accountcode", accountcode);
            var sessionid = session.Id;
            Response.Headers.Add("Location", session.Url);
            return Ok(session.Url);
            ///return new StatusCodeResult(303);
        }

        [HttpGet("setupintent")]
        public ActionResult SetupIntent()
        {
            var domain = "http://localhost:4200";
            try
            {
                // Call the CreateCustomer function to create a customer
                Stripe.Customer customer = CreateCustomer();
                var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                "card",
            },
                Mode = "setup",
                //Customer = "{{CUSTOMER_ID}}",
                Customer = "cus_OWrOn294kzDKwG",
                
                //SuccessUrl = "https://example.com/success?session_id={CHECKOUT_SESSION_ID}",
                SuccessUrl = domain + "/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/cancel?session_id={CHECKOUT_SESSION_ID}",
            };

            var service = new SessionService();
            var requestOptions = new RequestOptions();
            requestOptions.StripeAccount = "acct_1Nc8teSCPyqIiJIs";
            Session session = service.Create(options);
           
            return Ok(session.Url);
            }
            catch (StripeException ex)
            {
                // Handle any Stripe API exceptions and return an appropriate response
                return BadRequest(new { error = ex.Message });
            }
            ///return new StatusCodeResult(303);
        }

        private Stripe.Customer CreateCustomer()
        {
            var options = new CustomerCreateOptions
            {
                Name="",
                Email="",
                Phone="",
                Metadata = new Dictionary<string, string>
                {
                    { "accountcode", "accountcode" }  // Set the "email" value in Metadata
                // Add more key-value pairs as needed
                },
                Description = "Customer for Advisor Portal",
            };
            var service = new CustomerService();

            try
            {
                Stripe.Customer customer = service.Create(options);
                // Return the customer object as JSON
                return customer;
            }
            catch (StripeException ex)
            {
                // Handle any Stripe API exceptions and return an appropriate response
                throw ex;
            }
        }
        [HttpPost("create-customer")]
        public ActionResult CreateCustomer([FromBody] AdvisorAccount obj)
        {
            try
            {
                var options = new CustomerCreateOptions
                {
                    Name = obj.name,
                    Email = obj.email,
                    Phone = obj.phone,

                    Metadata = new Dictionary<string, string>
                {
                    { "accountcode", obj.accountcode },
                    { "name", obj.name },
                    { "email", obj.email },
                    { "phone", obj.phone },
                },
                    Description = "Customer for Advisor Portal",
                };
                var service = new CustomerService();
                Stripe.Customer customer = service.Create(options);
                    // Return the customer object as JSON
                    return Ok(new { customerid = customer.Id });
                
                

            }
            catch (StripeException ex)
            {
                // Handle any Stripe API exceptions and return an appropriate response
                return BadRequest(new { error = ex.Message });
            }
            ///return new StatusCodeResult(303);
        }


        [HttpPost("create-setup-intent")]
        public ActionResult CreateSetupIntent([FromBody] SetupIntentInput obj)
        {
            try
            {
                var options = new SetupIntentCreateOptions
                {
                    Customer = "cus_OXVDzampHOP6Ul",
                    Metadata = new Dictionary<string, string>
                {
                    { "email", obj.Email }  // Set the "email" value in Metadata
                // Add more key-value pairs as needed
                },
                    AutomaticPaymentMethods = new SetupIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                };
                var service = new SetupIntentService();
               
                var setupIntent = service.Create(options);  // Capture the result

                return Ok(new { clientSecret = setupIntent.ClientSecret });

            }
            catch (StripeException ex)
            {
                // Handle any Stripe API exceptions and return an appropriate response
                return BadRequest(new { error = ex.Message });
            }
            ///return new StatusCodeResult(303);
        }
    }
}
