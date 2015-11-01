//-----------------------------------------------------------------------
// <copyright file="GeneralLedgerSetting.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:50:13 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Financials
{
    [Table("GeneralLedgerSetting")]
    public partial class GeneralLedgerSetting : BaseEntity
    {
        public GeneralLedgerSetting()
        {
        }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int? PayableAccountId { get; set; }
        public int? PurchaseDiscountAccountId { get; set; }
        public int? GoodsReceiptNoteClearingAccountId { get; set; }
        public int? SalesDiscountAccountId { get; set; }
        public int? ShippingChargeAccountId { get; set; }

        public virtual Account PayableAccount { get; set; }
        public virtual Account PurchaseDiscountAccount { get; set; }
        public virtual Account GoodsReceiptNoteClearingAccount { get; set; }
        public virtual Account SalesDiscountAccount { get; set; }
        public virtual Account ShippingChargeAccount { get; set; } 

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
