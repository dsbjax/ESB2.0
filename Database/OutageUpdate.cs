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
    using System.Collections.Generic;
    
    public partial class OutageUpdate
    {
        public int Id { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Update { get; set; }
        public int OutageId { get; set; }
    
        public virtual User UpdateBy { get; set; }
    }
}
