//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TP_OH_AdminControlPanel
{
    using System;
    using System.Collections.Generic;
    
    public partial class EarnedCreditsFromEventTable
    {
        public int creditEventID { get; set; }
        public int eventIDFK { get; set; }
        public int userIDFK { get; set; }
    
        public virtual EventsTable EventsTable { get; set; }
        public virtual User User { get; set; }
    }
}
