//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Plotnikov_PR_21_102_DocumentManager.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class test_case_results
    {
        public int id_test_case_result { get; set; }
        public int id_tester { get; set; }
        public int id_test_case { get; set; }
        public System.DateTime when_datetime { get; set; }
        public string real_result { get; set; }
        public bool contains_bug_report { get; set; }
        public string bug_report_description { get; set; }
        public string bug_report_reproducibility { get; set; }
        public string bug_report_attachments { get; set; }
    
        public virtual test_cases test_cases { get; set; }
        public virtual workers workers { get; set; }
    }
}