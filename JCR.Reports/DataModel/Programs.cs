//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JCR.Reports.DataModel
{
    using System;
    
    public partial class Programs
    {
        public int ProgramID { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string AdvCertFlag { get; set; }
        public Nullable<int> AdvCertListTypeID { get; set; }
        public string CertProgramName { get; set; }
        public Nullable<long> SortOrder { get; set; }
        public int BaseProgramID { get; set; }
    }
}