//-----------------------------------------------------------------------
// <copyright file="SalesOrders.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:50:13 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels.Sales
{
    public partial class SalesOrders
    {
        public SalesOrders()
        {
            SalesOrdersViewModel = new HashSet<SalesOrderViewModel>();
        }
        public virtual ICollection<SalesOrderViewModel> SalesOrdersViewModel { get; set; }
    }

    public partial class SalesOrderViewModel
    {
        public SalesOrderViewModel()
        {
            Date = DateTime.Now;
        }

        public int Id { get; set; }
        public string Customer { get; set; }
        public DateTime Date { get; set; }
        public int? PaymentTermId { get; set; }
        public decimal Amount { get; set; }
    }
}
