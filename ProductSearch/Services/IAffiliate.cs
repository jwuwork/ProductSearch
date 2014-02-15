using ProductSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSearch.Services
{
    interface IAffiliate
    {
        ICollection<Product> Find(string keywords, int page);
        Affiliate GetAffiliate();
    }
}
