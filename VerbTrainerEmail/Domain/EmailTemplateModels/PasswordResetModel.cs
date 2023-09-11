using System;
using VerbTrainerEmail.Domain.Base;

namespace VerbTrainerEmail.Domain.EmailTemplateModels
{
	public struct PasswordResetModel : IEmailTemplateModel
    {
		public string Name { get; private set; }
		public string Link { get; private set; }

        private PasswordResetModel(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public static PasswordResetModel CreateNew(
            string name, string link)
        {
            return new PasswordResetModel(name, link);
        }
    }
}

