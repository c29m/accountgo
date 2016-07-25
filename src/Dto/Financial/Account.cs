﻿namespace Dto.Financial
{
    public class Account : BaseDto
    {
        public int? ParentAccountId { get; set; }
        public int CompanyId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public bool IsCash { get; set; }
        public bool IsContraAccount { get; set; }
        public decimal Balance { get; set; }
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
    }

    public class SelectAccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
    }
}
