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
    
    public partial class Shifts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shifts()
        {
            this.ReceiptsOfProducts = new HashSet<ReceiptsOfProducts>();
            this.SalesOfProducts = new HashSet<SalesOfProducts>();
            this.WritesOffOfProducts = new HashSet<WritesOffOfProducts>();
        }
    
        public int ShiftID { get; set; }
        public int WorkerID_FK { get; set; }
        public decimal ArriveOfMoney { get; set; }
        public decimal FlowOfNomey { get; set; }
        public decimal MoneyAtStartShift { get; set; }
        public System.DateTime ShiftDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceiptsOfProducts> ReceiptsOfProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOfProducts> SalesOfProducts { get; set; }
        public virtual Workers Workers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WritesOffOfProducts> WritesOffOfProducts { get; set; }
    }
}
