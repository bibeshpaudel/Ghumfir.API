﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghumfir.Application.DTOs.UserDTO
{
    public class ChangePasswordDto
    {
        public string? OldPassword { get; set; } = string.Empty;
        public string? NewPassword { get; set; } = string.Empty;
        public string? ConfirmNewPassword { get; set;} = string.Empty;
    }
}