using RazorEngine.Templating;
using Newtonsoft.Json;
using VerbTrainerEmail.Domain.Entities.Email;
using VerbTrainerEmail.Domain.Entities.User;
using VerbTrainerEmail.Domain.ValueObjects;
using RazorEngine;
using Microsoft.Extensions.Hosting.Internal;

//TODO: Database table where each email request will be added immediately, and when email is sent -> marked sent
namespace VerbTrainerEmail.Application.SendEmail
{
    public abstract class SendEmailHandler<T> where T : EmailEntity
    {
        // Abstract methods must be implemented in the deriving classes
        protected abstract T CreateEmailEntity(
            Dictionary<string, object> emailData,
            User userEntity);

        protected abstract Dictionary<string, object>
            CreateEmailModel(T emailEntity);
          

        public T ParseJsonToEntity(string json)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (data is null ||
                   !data.ContainsKey("User") ||
                   !data.ContainsKey("Email"))
                {
                    //TODO: add handling
                    throw new ArgumentException("Invalid JSON format.");
                }

                Dictionary<string, object> userDict = (Dictionary<string, object>)data["User"];
                Dictionary<string, object> emailDict = (Dictionary<string, object>)data["Email"];

                User userEntity = User.CreateNew
                (
                    (int)userDict["Id"],
                    (string)userDict["Email"],
                    (string?)userDict["FirstName"],
                    (string?)userDict["LastName"],
                    (UserStatus)userDict["Status"],
                    (DateTime?)userDict["LastLogin"]
                );

                T emailEntity = CreateEmailEntity(emailDict, userEntity);

                return emailEntity;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error parsing JSON data.", ex);
            }

        }
        
        public string RenderEmailTemplate(T emailEntity)
        {
            var model = CreateEmailModel(emailEntity);
            var template = File.ReadAllText(emailEntity.Template);
            var result = Engine.Razor.RunCompile(template, null, model);
            return result;
        }

    }
}

