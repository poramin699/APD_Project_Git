//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APD_Project
{
    using System;
    using System.Collections.Generic;
    
    public partial class P_Promotion
    {
        public int pro_id { get; set; }
        public Nullable<double> discount { get; set; }
        public string product_id_1 { get; set; }
        public string product_id_2 { get; set; }
    
        public virtual P_Product P_Product { get; set; }
        public virtual P_Product P_Product1 { get; set; }
    }
}