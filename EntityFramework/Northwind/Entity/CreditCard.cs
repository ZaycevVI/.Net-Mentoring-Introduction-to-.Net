using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Entity
{
    public class CreditCard
    {
        public int CreditCardID { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string CardHolder { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
