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
    
    public partial class project_workers
    {
        public int id_project_worker { get; set; }
        public int id_project { get; set; }
        public int id_worker { get; set; }
    
        public virtual projects projects { get; set; }
        public virtual workers workers { get; set; }
    }
}
