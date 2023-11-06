using StocksMarketEntitiesLibrary.EmailEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace SharedServices.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailsAsync(Emailer emailer);
        Task SendEmailWithFilesAsync(Emailer emailer, string[] filePaths);
    }
}
