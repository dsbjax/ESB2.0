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
    public enum UserPermissions : int
    {
        ReportViewer = 1,
        StatusUpdates = 2,
        UserManager = 4,
        EquipmentManager = 8,
        StatusBoardManager = 16,
        DatabaseAdmin = 32
    }
}