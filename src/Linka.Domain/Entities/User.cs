﻿using Linka.Domain.Common;
using Linka.Domain.Enums;

namespace Linka.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
    }
}
