﻿using Core.Domain.Financials;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace Core.Domain.Sales
{
    [Table("Customer")]
    public partial class Customer : Party
    {
        public Customer()
        {
            SalesInvoices = new HashSet<SalesInvoiceHeader>();
            SalesReceipts = new HashSet<SalesReceiptHeader>();
            SalesOrders = new HashSet<SalesOrderHeader>();
            //Contacts = new HashSet<Contact>();
        }

        public int? TaxGroupId { get; set; }
        [Display(Name = "Accounts Receivable")]
        public int? AccountsReceivableAccountId { get; set; }
        public int? SalesAccountId { get; set; }
        public int? SalesDiscountAccountId { get; set; }
        public int? PromptPaymentDiscountAccountId { get; set; }
        public string No { get; set; }
        public int? PrimaryContactId { get; set; }
        public int? PaymentTermId { get; set; }

        public virtual TaxGroup TaxGroup { get; set; }
        public virtual Account AccountsReceivableAccount { get; set; }
        public virtual Account SalesAccount { get; set; }
        public virtual Account SalesDiscountAccount { get; set; }
        public virtual Account PromptPaymentDiscountAccount { get; set; }
        public virtual Contact PrimaryContact { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }

        public virtual ICollection<SalesInvoiceHeader> SalesInvoices { get; set; }
        public virtual ICollection<SalesReceiptHeader> SalesReceipts { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrders { get; set; }
        //public virtual ICollection<Contact> Contacts { get; set; }


        [NotMapped]
        public decimal Balance { get { return GetBalance(); } }

        private decimal GetBalance()
        {
            decimal balance = 0;
            decimal totalInvoiceAmount = 0;
            decimal totalReceiptAmount = 0;
            decimal totalAllocation = 0;

            foreach (var header in SalesInvoices)
            {
                totalInvoiceAmount += header.SalesInvoiceLines.Sum(d => d.Amount);
                totalAllocation += header.CustomerAllocations.Sum(a => a.Amount);

                foreach (var receipt in header.SalesReceipts)
                    foreach(var receiptLine in receipt.SalesReceiptLines)
                        totalReceiptAmount += receiptLine.AmountPaid;
            }

            balance = (totalInvoiceAmount - totalReceiptAmount) - totalAllocation;

            return balance;
        }
    }
}
