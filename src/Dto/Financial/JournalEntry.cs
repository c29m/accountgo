﻿using System;

namespace Dto.Financial
{
    public class JournalEntry
    {
        public DateTime Date { get; set; }
        public int VoucherType { get; set; }
        public string ReferenceNo { get; set; }
        public string Memo { get; set; }
    }

    public class JournalEntryLine
    {
        public int Id { get; set; }
        public int JournalHeaderId { get; set; }
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string DrCr { get; set; }
        public decimal Amount { get; set; }
        public int GeneralLedgerHeaderId { get; set; }
    }
}
