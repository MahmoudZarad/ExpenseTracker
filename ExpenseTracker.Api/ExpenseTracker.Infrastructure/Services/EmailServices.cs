using ExpenseTracker.Application.Common.Email;
using ExpenseTracker.Application.Interfaces.ExternalServices;
using Hangfire;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ExpenseTracker.Infrastructure.Services;

public class EmailServices : IEmailService
{
    private readonly EmailSettings _settings;
    public EmailServices(IOptions<EmailSettings> options) => _settings = options.Value;

    public async Task SendEmailAsync(EmailDto EmailDto)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_settings.SenderName, _settings.DomainEmail));
        email.To.Add(MailboxAddress.Parse(EmailDto.To));
        email.Subject = EmailDto.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = EmailDto.Body };

        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        // Accept the server certificate if only the revocation check failed
        smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
        {
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                return true;

            // If the only issue is revocation status unknown, accept it
            if (chain is not null)
            {
                foreach (var status in chain.ChainStatus)
                {
                    if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown &&
                        status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.OfflineRevocation &&
                        status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                        return false;
                }
                return true;
            }

            return false;
        };

        await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.username, _settings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendEmailverificationAsync(string email, string verificationToken, string Endpoint, string subject, string FullName = "Dear customer")
    {
        var verificationLink = $"{_settings.BaseUrl}/api/{Endpoint}?" +
                               $"email={Uri.EscapeDataString(email)}" +
                               $"&Token={Uri.EscapeDataString(verificationToken)}";

        var body = $@"
        <!DOCTYPE html>
        <html>w
        <body style='font-family:Arial, sans-serif; background:#f4f6f8; padding:30px;'>
        
        <div style='max-width:500px; margin:auto; background:#ffffff; 
                    padding:30px; border-radius:10px; text-align:center;'>
        
            <h2 style='color:#2c3e50; margin-bottom:20px;'>
                Welcome {FullName} 👋
            </h2>
        
            <p style='color:#555; font-size:14px;'>
                Please {subject} by clicking the button below:
            </p>
        
            <div style='margin:25px 0;'>
                <a href='{verificationLink}' 
                   style='background:#2563eb; color:#ffffff; 
                          padding:12px 25px; 
                          text-decoration:none; 
                          border-radius:6px; 
                          font-weight:bold; 
                          display:inline-block;'>
                  {subject}
                </a>
            </div>
        
            <p style='font-size:12px; color:#777;'>
                This link expires in 24 hours.
            </p>
        
            <hr style='border:none; border-top:1px solid #eee; margin:25px 0;' />
        
            <p style='font-size:11px; color:#aaa;'>
                If you didn’t request this, you can safely ignore this email.
            </p>
        </div>
        </body>
        </html>";

        BackgroundJob.Enqueue(() =>
            SendEmailAsync(new EmailDto
            {
                To = email,
                Subject = subject,
                Body = body
            }));
    }

    public async Task SendCodeverificationAsync(string email, string Codeverification, string subject, string FullName = "Dear customer")
    {
        var body = $@"<!DOCTYPE html>
                      <html>
                      <head>
                      <meta charset='UTF-8'>
                      <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                      <title>Email Verification</title>
                      </head>
                      <body style='margin:0; padding:0; background-color:#f4f6f8; font-family:Arial, sans-serif;'>
                      
                      <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f6f8; padding:40px 0;'>
                      <tr>
                      <td align='center'>
                      
                          <table width='500' cellpadding='0' cellspacing='0' 
                                 style='background:#ffffff; border-radius:12px; padding:40px; text-align:center;'>
                      
                              <!-- Logo -->
                              <tr>
                                  <td>
                                      <h1 style='margin:0; color:#2c3e50;'>{_settings.SenderName}</h1>
                                  </td>
                              </tr>
                      
                              <!-- Spacer -->
                              <tr><td style='height:20px;'></td></tr>
                      
                              <!-- Greeting -->
                              <tr>
                                  <td>
                                      <h2 style='margin:0; color:#333;'>Welcome {FullName} 👋</h2>
                                  </td>
                              </tr>
                      
                              <!-- Spacer -->
                              <tr><td style='height:20px;'></td></tr>
                      
                              <!-- Message -->
                              <tr>
                                  <td style='color:#555; font-size:15px;'>
                                      Your verification code is:
                                  </td>
                              </tr>
                      
                              <!-- Code Box -->
                              <tr>
                                  <td style='padding:25px 0;'>
                                      <div style='
                                          display:inline-block;
                                          background:#f0f3f7;
                                          padding:15px 30px;
                                          font-size:28px;
                                          font-weight:bold;
                                          letter-spacing:6px;
                                          border-radius:8px;
                                          color:#2c3e50;'>
                                          {Codeverification}
                                      </div>
                                  </td>
                              </tr>
                      
                              <!-- Expiry -->
                              <tr>
                                  <td style='color:#777; font-size:13px;'>
                                      This code expires in {_settings.DurationInMinutes} minutes.
                                  </td>
                              </tr>
                      
                              <!-- Spacer -->
                              <tr><td style='height:30px;'></td></tr>
                      
                              <!-- Divider -->
                              <tr>
                                  <td style='border-top:1px solid #eaeaea; padding-top:20px; font-size:12px; color:#999;'>
                                      If you didn’t request this email, you can safely ignore it.
                                  </td>
                              </tr>
                      
                              <!-- Footer -->
                              <tr>
                                  <td style='padding-top:15px; font-size:11px; color:#bbb;'>
                                      © {DateTime.UtcNow.Year} {_settings.SenderName}. All rights reserved.
                                  </td>
                              </tr>
                          </table>
                      </td>
                      </tr>
                      </table>
                      </body>
                      </html>";

        BackgroundJob.Enqueue(() =>
            SendEmailAsync(new EmailDto
            {
                To = email,
                Subject = subject,
                Body = body
            }));
    }
}