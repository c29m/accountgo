﻿using System.Linq;
using System.Collections.Generic;
using Core.Data;
using Core.Domain;
using Core.Domain.Financials;

namespace Services.Administration
{
    public class AdministrationService : BaseService, IAdministrationService
    {
        private readonly IRepository<FiscalYear> _fiscalYearRepo;
        private readonly IRepository<TaxGroup> _taxGroupRepo;
        private readonly IRepository<ItemTaxGroup> _itemTaxGroupRepo;
        private readonly IRepository<PaymentTerm> _paymentTermRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<GeneralLedgerSetting> _genetalLedgerSetting;
        private readonly IRepository<Tax> _taxRepo;

        public AdministrationService(IRepository<FiscalYear> fiscalYearRepo,
            IRepository<TaxGroup> taxGroupRepo,
            IRepository<ItemTaxGroup> itemTaxGroupRepo,
            IRepository<PaymentTerm> paymentTermRepo,
            IRepository<Bank> bankRepo,
            IRepository<Tax> taxRepo,
            IRepository<GeneralLedgerSetting> generalLedgerSetting)
            : base(null, generalLedgerSetting, paymentTermRepo, bankRepo)
        {
            _fiscalYearRepo = fiscalYearRepo;
            _taxGroupRepo = taxGroupRepo;
            _itemTaxGroupRepo = itemTaxGroupRepo;
            _paymentTermRepo = paymentTermRepo;
            _bankRepo = bankRepo;
            _genetalLedgerSetting = generalLedgerSetting;
            _taxRepo = taxRepo;
        }

        public ICollection<Tax> GetAllTaxes(bool includeInActive)
        {
            var query = from f in _taxRepo.Table
                        select f;
            return query.ToList();
        }

        public void AddNewTax(Tax tax)
        {
            _taxRepo.Insert(tax);
        }

        public void UpdateTax(Tax tax)
        {
            _taxRepo.Update(tax);
        }

        public void DeleteTax(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<ItemTaxGroup> GetItemTaxGroups(bool includeInActive)
        {
            var query = from f in _itemTaxGroupRepo.Table
                        select f;
            return query.ToList();
        }

        public ICollection<TaxGroup> GetTaxGroups(bool includeInActive)
        {
            var query = from f in _taxGroupRepo.Table
                        select f;
            return query.ToList();
        }
    }
}
