//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Plotnikov_PR_21_102_DocumentManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class documents
    {
        public documents()
        {
            this.files = new HashSet<files>();
        }
    
        public int id_document { get; set; }
        public int id_project { get; set; }
        public int id_document_type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    
        public virtual document_types document_types { get; set; }
        public virtual projects projects { get; set; }
        public virtual ICollection<files> files { get; set; }
    }
}
