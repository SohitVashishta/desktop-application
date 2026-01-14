using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SchoolManagementSystem.Business.Notification
{
    public class SmsService
    {
        public void Send(string phone, string message)
        {
            TwilioClient.Init("ACCOUNT_SID", "AUTH_TOKEN");

            MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber("+123456789"),
                to: new Twilio.Types.PhoneNumber(phone)
            );
        }
    }
}
