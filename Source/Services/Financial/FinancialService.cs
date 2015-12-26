//-----------------------------------------------------------------------
// <copyright file="FinancialService.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using Core.Data;
using Core.Domain;
using Core.Domain.Financials;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Domain.Items;
using Services.Inventory;

namespace Services.Financial
{
    public partial class FinancialService : BaseService, IFinancialService
    {
        private readonly IInventoryService _inventoryService;

        private readonly IRepository<GeneralLedgerHeader> _generalLedgerRepository;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<Tax> _taxRepo;
        private readonly IRepository<JournalEntryHeader> _journalEntryRepo;
        private readonly IRepository<GeneralLedgerLine> _generalLedgerLineRepository;
        private readonly IRepository<FiscalYear> _fiscalYearRepo;
        private readonly IRepository<TaxGroup> _taxGroupRepo;
        private readonly IRepository<ItemTaxGroup> _itemTaxGroupRepo;
        private readonly IRepository<PaymentTerm> _paymentTermRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<GeneralLedgerSetting> _glSettingRepo;

        public FinancialService(IInventoryService inventoryService, 
            IRepository<GeneralLedgerHeader> generalLedgerRepository,
            IRepository<GeneralLedgerLine> generalLedgerLineRepository,
            IRepository<Account> accountRepo,
            IRepository<Tax> taxRepo,
            IRepository<JournalEntryHeader> journalEntryRepo,
            IRepository<FiscalYear> fiscalYearRepo,
            IRepository<TaxGroup> taxGroupRepo,
            IRepository<ItemTaxGroup> itemTaxGroupRepo,
            IRepository<PaymentTerm> paymentTermRepo,
            IRepository<Bank> bankRepo,
            IRepository<Item> itemRepo,
            IRepository<GeneralLedgerSetting> glSettingRepo
            )
            :base(null, null, paymentTermRepo, bankRepo)
        {
            _inventoryService = inventoryService;
            _generalLedgerRepository = generalLedgerRepository;
            _accountRepo = accountRepo;
            _taxRepo = taxRepo;
            _journalEntryRepo = journalEntryRepo;
            _generalLedgerLineRepository = generalLedgerLineRepository;
            _fiscalYearRepo = fiscalYearRepo;
            _taxGroupRepo = taxGroupRepo;
            _itemTaxGroupRepo = itemTaxGroupRepo;
            _paymentTermRepo = paymentTermRepo;
            _bankRepo = bankRepo;
            _itemRepo = itemRepo;
            _glSettingRepo = glSettingRepo;
        }

        public FiscalYear CurrentFiscalYear()
        {
            var query = (from fy in _fiscalYearRepo.Table
                        where fy.IsActive == true
                        select fy).FirstOrDefault();

            return query;
        }

        public GeneralLedgerHeader CreateGeneralLedgerHeader(DocumentTypes documentType, DateTime date, string description)
        {
            var entry = new GeneralLedgerHeader()
            {
                DocumentType = documentType,
                Date = date,
                Description = description,
                CreatedBy = Thread.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.Now,
                ModifiedBy = Thread.CurrentPrincipal.Identity.Name,
                ModifiedOn = DateTime.Now
            };
            return entry;
        }

        public GeneralLedgerLine CreateGeneralLedgerLine(TransactionTypes DrCr, int accountId, decimal amount)
        {
            var line = new GeneralLedgerLine()
            {
                DrCr = DrCr,
                AccountId = accountId,
                Amount = amount,
                CreatedBy = Thread.CurrentPrincipal.Identity.Name,
                CreatedOn = DateTime.Now,
                ModifiedBy = Thread.CurrentPrincipal.Identity.Name,
                ModifiedOn = DateTime.Now
            };
            return line;
        }

        public IEnumerable<Account> GetAccounts()
        {
            var query = from f in _accountRepo.Table
                        select f;
            return query.AsEnumerable();
        }

        public IEnumerable<Tax> GetTaxes()
        {
            var query = from f in _taxRepo.Table
                        select f;
            return query.AsEnumerable();
        }

        public IEnumerable<ItemTaxGroup> GetItemTaxGroups()
        {
            var query = from f in _itemTaxGroupRepo.Table
                        select f;
            return query;
        }

        public IEnumerable<TaxGroup> GetTaxGroups()
        {
            var query = from f in _taxGroupRepo.Table
                        select f;
            return query;
        }

        public void SaveGeneralLedgerEntry(GeneralLedgerHeader entry)
        {
            if (ValidateGeneralLedgerEntry(entry))
            {
                try
                {
                    _generalLedgerRepository.Insert(entry);
                }
                catch { }
            }
        }

        public bool ValidateGeneralLedgerEntry(GeneralLedgerHeader glEntry)
        {
            if (!FinancialHelper.DrCrEqualityValidated(glEntry))
                throw new InvalidOperationException("Debit/Credit are not equal.");

            if (!FinancialHelper.NoLineAmountIsEqualToZero(glEntry))
                throw new InvalidOperationException("One or more line(s) amount is zero.");

            var fiscalYear = CurrentFiscalYear();
            if (fiscalYear == null || !FinancialHelper.InRange(glEntry.Date, fiscalYear.StartDate, fiscalYear.EndDate))
                throw new InvalidOperationException("Date is out of range. Must within financial year.");

            //var duplicateAccounts = glEntry.GeneralLedgerLines.GroupBy(gl => gl.AccountId).Where(gl => gl.Count() > 1);
            //if (duplicateAccounts.Count() > 0)
            //    throw new InvalidOperationException("Duplicate account id in a collection.");

            foreach (var account in glEntry.GeneralLedgerLines)
                if (!_accountRepo.GetById(account.AccountId).CanPost())
                    throw new InvalidOperationException("Account is not valid for posting");
        
            return true;
        }

        public IEnumerable<JournalEntryHeader> GetJournalEntries()
        {
            var query = from je in _journalEntryRepo.Table
                        select je;
            return query.ToList();
        }

        public void AddJournalEntry(JournalEntryHeader journalEntry)
        {
            var glEntry = new GeneralLedgerHeader()
            {
                Date = journalEntry.Date,
                DocumentType = Core.Domain.DocumentTypes.JournalEntry,
                Description = journalEntry.Memo,
                CreatedBy = journalEntry.CreatedBy,
                CreatedOn = journalEntry.CreatedOn,
                ModifiedBy = journalEntry.ModifiedBy,
                ModifiedOn = journalEntry.ModifiedOn,
            };

            foreach (var je in journalEntry.JournalEntryLines)
            {
                glEntry.GeneralLedgerLines.Add(new GeneralLedgerLine()
                {
                    AccountId = je.AccountId,
                    DrCr = je.DrCr,
                    Amount = je.Amount,
                    CreatedBy = journalEntry.CreatedBy,
                    CreatedOn = journalEntry.CreatedOn,
                    ModifiedBy = journalEntry.ModifiedBy,
                    ModifiedOn = journalEntry.ModifiedOn,
                });
            }

            if (ValidateGeneralLedgerEntry(glEntry))
            {
                journalEntry.GeneralLedgerHeader = glEntry;
                _journalEntryRepo.Insert(journalEntry);
            }
        }

        public ICollection<TrialBalance> TrialBalance(DateTime? from = default(DateTime?), DateTime? to = default(DateTime?))
        {            
            var allDr = (from dr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where dr.DrCr == TransactionTypes.Dr
                         //&& IsDateBetweenFinancialYearStartDateAndEndDate(dr.GLHeader.Date)
                         group dr by new { dr.Account.AccountCode, dr.Account.AccountName, dr.Amount } into tb
                         select new
                         {
                             AccountCode = tb.Key.AccountCode,
                             AccountName = tb.Key.AccountName,
                             Debit = tb.Sum(d => d.Amount),
                         });

            var allCr = (from cr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where cr.DrCr == TransactionTypes.Cr
                         //&& IsDateBetweenFinancialYearStartDateAndEndDate(cr.GLHeader.Date)
                         group cr by new { cr.Account.AccountCode, cr.Account.AccountName, cr.Amount } into tb
                         select new
                         {
                             AccountCode = tb.Key.AccountCode,
                             AccountName = tb.Key.AccountName,
                             Credit = tb.Sum(c => c.Amount),
                         });

            var allDrcr = (from x in allDr
                           select new TrialBalance
                           {
                               AccountCode = x.AccountCode,
                               AccountName = x.AccountName,
                               Debit = x.Debit,
                               Credit = (decimal)0,
                           }
                          ).Concat(from y in allCr
                                   select new TrialBalance
                                   {
                                       AccountCode = y.AccountCode,
                                       AccountName = y.AccountName,
                                       Debit = (decimal)0,
                                       Credit = y.Credit,
                                   });

            var sortedList = allDrcr
                .OrderBy(tb => tb.AccountCode)
                .ToList()
                .Reverse<TrialBalance>();

            var accounts = sortedList.ToList().GroupBy(a => a.AccountCode)
                .Select(tb => new TrialBalance
                {
                    AccountCode = tb.First().AccountCode,
                    AccountName = tb.First().AccountName,
                    Credit = tb.Sum(x => x.Credit),
                    Debit = tb.Sum(y => y.Debit)
                }).ToList();
            return accounts;
        }

        public ICollection<BalanceSheet> BalanceSheet(DateTime? from = default(DateTime?), DateTime? to = default(DateTime?))
        {
            var assets = from a in _accountRepo.Table
                         where a.AccountClassId == 1 && a.ParentAccountId != null
                         select a;
            var liabilities = from a in _accountRepo.Table
                              where a.AccountClassId == 2 && a.ParentAccountId != null
                              select a;
            var equities = from a in _accountRepo.Table
                           where a.AccountClassId == 3 && a.ParentAccountId != null
                           select a;

            var balanceSheet = new HashSet<BalanceSheet>();
            foreach (var asset in assets)
            {
                balanceSheet.Add(new BalanceSheet()
                {
                    AccountClassId = asset.AccountClassId,
                    AccountCode = asset.AccountCode,
                    AccountName = asset.AccountName,
                    Amount = asset.Balance
                });
            }
            foreach (var liability in liabilities)
            {
                balanceSheet.Add(new BalanceSheet()
                {
                    AccountClassId = liability.AccountClassId,
                    AccountCode = liability.AccountCode,
                    AccountName = liability.AccountName,
                    Amount = liability.Balance
                });
            }
            foreach (var equity in equities)
            {
                balanceSheet.Add(new BalanceSheet()
                {
                    AccountClassId = equity.AccountClassId,
                    AccountCode = equity.AccountCode,
                    AccountName = equity.AccountName,
                    Amount = equity.Balance
                });
            }
            return balanceSheet;
        }

        public ICollection<IncomeStatement> IncomeStatement(DateTime? from = default(DateTime?), DateTime? to = default(DateTime?))
        {
            var revenues = from r in _accountRepo.Table
                           where r.AccountClassId == 4 && r.ParentAccountId != null
                           select r;

            var expenses = from e in _accountRepo.Table
                           where e.AccountClassId == 5 && e.ParentAccountId != null
                           select e;

            var revenues_expenses = new HashSet<IncomeStatement>();
            foreach (var revenue in revenues)
            {
                revenues_expenses.Add(new IncomeStatement()
                {
                    AccountCode = revenue.AccountCode,
                    AccountName = revenue.AccountName,
                    Amount = revenue.Balance,
                    IsExpense = false
                });
            }
            foreach (var expense in expenses)
            {
                revenues_expenses.Add(new IncomeStatement()
                {
                    AccountCode = expense.AccountCode,
                    AccountName = expense.AccountName,
                    Amount = expense.Balance,
                    IsExpense = true
                });
            }
            return revenues_expenses;
        }

        public ICollection<MasterGeneralLedger> MasterGeneralLedger(DateTime? from = default(DateTime?), DateTime? to = default(DateTime?))
        {
            var allDr = (from dr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where dr.DrCr == TransactionTypes.Dr
                         //&& GeneralLedgerHelper.IsBetween(dr.GLHeader.Date, (DateTime)fromDate, (DateTime)toDate) == true
                         select new MasterGeneralLedger
                         {
                             Id = dr.Id,
                             TransactionNo = dr.GeneralLedgerHeader.Id,
                             AccountId = dr.AccountId,
                             AccountCode = dr.Account.AccountCode,
                             AccountName = dr.Account.AccountName,
                             Date = dr.GeneralLedgerHeader.Date,
                             Debit = dr.Amount,
                             Credit = (decimal)0,
                             //CurrencyId = dr.CurrencyId,
                             DocumentType = Enum.GetName(typeof(DocumentTypes), dr.GeneralLedgerHeader.DocumentType)
                         });

            var allCr = (from cr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where cr.DrCr == TransactionTypes.Cr
                         //&& GeneralLedgerHelper.IsBetween(cr.GLHeader.Date, (DateTime)fromDate, (DateTime)toDate) == true
                         select new MasterGeneralLedger
                         {
                             Id = cr.Id,
                             TransactionNo = cr.GeneralLedgerHeader.Id,
                             AccountId = cr.AccountId,
                             AccountCode = cr.Account.AccountCode,
                             AccountName = cr.Account.AccountName,
                             Date = cr.GeneralLedgerHeader.Date,
                             Debit = (decimal)0,
                             Credit = cr.Amount,
                             //CurrencyId = cr.CurrencyId,
                             DocumentType = Enum.GetName(typeof(DocumentTypes), cr.GeneralLedgerHeader.DocumentType)
                         });

            var allDrcr = (from x in allDr
                           select new MasterGeneralLedger
                           {
                               Id = x.Id,
                               TransactionNo = x.TransactionNo,
                               AccountId = x.AccountId,
                               AccountCode = x.AccountCode,
                               AccountName = x.AccountName,
                               Date = x.Date,
                               Debit = x.Debit,
                               Credit = (decimal)0,
                               CurrencyId = x.CurrencyId,
                               DocumentType = x.DocumentType
                           }
                          ).Concat(from y in allCr
                                   select new MasterGeneralLedger
                                   {
                                       Id = y.Id,
                                       TransactionNo = y.TransactionNo,
                                       AccountId = y.AccountId,
                                       AccountCode = y.AccountCode,
                                       AccountName = y.AccountName,
                                       Date = y.Date,
                                       Debit = (decimal)0,
                                       Credit = y.Credit,
                                       CurrencyId = y.CurrencyId,
                                       DocumentType = y.DocumentType
                                   });

            var sortedList = allDrcr.OrderBy(gl => gl.Id).ToList().Reverse<MasterGeneralLedger>();
            return sortedList.ToList();
        }

        public new IEnumerable<Bank> GetCashAndBanks()
        {
            var query = from b in _bankRepo.Table
                        select b;
            return query;
        }

        /// <summary>
        /// Input VAT is the value added tax added to the price when you purchase goods or services liable to VAT. If the buyer is registered in the VAT Register, the buyer can deduct the amount of VAT paid from his/her settlement with the tax authorities. 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, decimal>> ComputeInputTax(int itemId, decimal quantity, decimal amount)
        {
            var taxes = new List<KeyValuePair<int, decimal>>();
            decimal taxAmount = 0;
            var item = _inventoryService.GetItemById(itemId);
            var lineAmount = quantity * amount;

            if (item.ItemTaxGroup != null)
            {
                foreach (var tax in item.ItemTaxGroup.ItemTaxGroupTax)
                {
                    if (!tax.IsExempt)
                    {
                        var lineTaxAmount = (tax.Tax.Rate / 100) * lineAmount;
                        taxAmount += lineTaxAmount;
                        taxes.Add(new KeyValuePair<int, decimal>(tax.Id, lineTaxAmount));
                    }
                }
            }
            return taxes;
        }

        /// <summary>
        /// Output VAT is the value added tax you calculate and charge on your own sales of goods and services
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, decimal>> ComputeOutputTax(int itemId, decimal quantity, decimal amount, decimal discount)
        {
            decimal taxAmount = 0, amountXquantity = 0, discountAmount = 0, subTotalAmount = 0;
            var item = _itemRepo.GetById(itemId);
            var taxes = new List<KeyValuePair<int, decimal>>();
            amountXquantity = amount * quantity;

            if(discount > 0)
                discountAmount = (discount / 100) * amountXquantity;

            subTotalAmount = amountXquantity - discountAmount;

            if (item.ItemTaxGroup != null)
            {
                foreach (var tax in item.ItemTaxGroup.ItemTaxGroupTax)
                {
                    if (!tax.IsExempt)
                    {
                        taxAmount = subTotalAmount - (subTotalAmount / (1 + (tax.Tax.Rate / 100)));
                        taxes.Add(new KeyValuePair<int, decimal>(tax.Id, taxAmount));
                    }
                }
            }
            return taxes;
        }

        public GeneralLedgerSetting GetGeneralLedgerSetting(int? companyId = null)
        {
            GeneralLedgerSetting glSetting = null;
            if (companyId == null)
                glSetting = _glSettingRepo.Table.FirstOrDefault();
            else
                glSetting = _glSettingRepo.Table.Where(s => s.CompanyId == companyId).FirstOrDefault();

            return glSetting;
        }

        public void UpdateGeneralLedgerSetting(GeneralLedgerSetting setting)
        {
            _glSettingRepo.Update(setting);
        }
    }
}
