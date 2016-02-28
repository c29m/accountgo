//-----------------------------------------------------------------------
// <copyright file="GeneralLedgerSettingViewModel.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels.Financials
{
    public class GeneralLedgerSettingViewModel
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public int? PayableAccountId { get; set; }
        public int? PurchaseDiscountAccountId { get; set; }
        public int? GoodsReceiptNoteClearingAccountId { get; set; }
        public int? SalesDiscountAccountId { get; set; }
        public int? ShippingChargeAccountId { get; set; }
    }
}
