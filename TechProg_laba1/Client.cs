using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechProg_laba1
{
    internal class Client
    {

        private string fullName;
        private string contactInfo;
        private bool isLoyaltyMember;

        public string FullName
        {
            get { return fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ошибка: ФИО не может быть пустым");
                }
                fullName = value;
            }
        }

        public string ContactInfo
        {
            get { return contactInfo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ошибка: контакная информация не может быть пустой");
                }
                contactInfo = value;
            }
        }

        public bool IsLoyaltyMember
        {
            get { return isLoyaltyMember; }
            set { isLoyaltyMember = value; }
        }


        public Client(string fullName, string contactInfo, bool isLoyaltyMember)
        {
            FullName = fullName;
            ContactInfo = contactInfo;
            IsLoyaltyMember = isLoyaltyMember;
        }


    }
}
