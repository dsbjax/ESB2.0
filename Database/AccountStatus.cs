//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    
    [Flags]
    public enum AccountStatus : int
    {
        AccountDisabled = 1,
        AccountLocked = 2,
        UserMustChangePassword = 4,
        AccountValid = 0
    }
}