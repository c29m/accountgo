﻿using Core.Domain;
using Core.Domain.Financials;
using System;
using System.Collections.Generic;

namespace Services.Financial
{
    public partial interface IFinancialService
    {
        bool ValidateGeneralLedgerEntry(GeneralLedgerHeader glEntry);
        void SaveGeneralLedgerEntry(GeneralLedgerHeader entry);
        GeneralLedgerLine CreateGeneralLedgerLine(TransactionTypes DrCr, int accountId, decimal amount);
        GeneralLedgerHeader CreateGeneralLedgerHeader(DocumentTypes documentType, DateTime Date, string description);
        IEnumerable<Account> GetAccounts();
        IEnumerable<JournalEntryHeader> GetJournalEntries();
        void AddJournalEntry(JournalEntryHeader journalEntry);
        ICollection<TrialBalance> TrialBalance(DateTime? from = null, DateTime? to = null);
        ICollection<BalanceSheet> BalanceSheet(DateTime? from = null, DateTime? to = null);
        ICollection<IncomeStatement> IncomeStatement(DateTime? from = null, DateTime? to = null);
        ICollection<MasterGeneralLedger> MasterGeneralLedger(DateTime? from = null, DateTime? to = null);
        FiscalYear CurrentFiscalYear();
        IEnumerable<Tax> GetTaxes();
        IEnumerable<ItemTaxGroup> GetItemTaxGroups();
        IEnumerable<TaxGroup> GetTaxGroups();
        IEnumerable<Bank> GetCashAndBanks();
        List<KeyValuePair<int, decimal>> ComputeInputTax(int itemId, decimal quantity, decimal amount);
        List<KeyValuePair<int, decimal>> ComputeOutputTax(int itemId, decimal quantity, decimal amount, decimal discount);
        GeneralLedgerSetting GetGeneralLedgerSetting(int? companyId = null);
        void UpdateGeneralLedgerSetting(GeneralLedgerSetting setting);
    }
}
