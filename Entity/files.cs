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
    
    public partial class files
    {
        public int id_files { get; set; }
        public int id_document { get; set; }
        public int id_worker { get; set; }
        public string contents { get; set; }
        public System.DateTime when_uploaded { get; set; }
    
        public virtual documents documents { get; set; }
        public virtual workers workers { get; set; }
    }
}
