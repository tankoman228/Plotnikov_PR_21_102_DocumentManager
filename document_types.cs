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
    
    public partial class document_types
    {
        public document_types()
        {
            this.documents = new HashSet<documents>();
        }
    
        public int id_document_type { get; set; }
        public string doctype_name { get; set; }
    
        public virtual ICollection<documents> documents { get; set; }
    }
}
