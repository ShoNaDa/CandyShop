//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Candy_shop
{
    using System;
    using System.Collections.Generic;
    
    public partial class WritesOffOfProducts
    {
        public int WriteOffOfProducts { get; set; }
        public int ProductID_FK { get; set; }
        public int ShiftID_FK { get; set; }
        public int CountOfProducts { get; set; }
        public string Info { get; set; }
    
        public virtual Products Products { get; set; }
        public virtual Shifts Shifts { get; set; }
    }
}
