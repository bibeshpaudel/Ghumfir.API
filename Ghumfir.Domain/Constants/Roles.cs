﻿namespace Ghumfir.Domain.Constants;

public static class Roles
{
    public const string Admin = "admin";
    public const string Customer = "customer";
    
    public static readonly HashSet<string> AllRoles =
    [
        Admin,
        Customer
    ];
}