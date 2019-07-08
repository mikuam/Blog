using System;

namespace MichalBialecki.com.NetCore.Web.Users
{
    [Serializable]
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public DateTime LastModified { get; set; }
    }
}
