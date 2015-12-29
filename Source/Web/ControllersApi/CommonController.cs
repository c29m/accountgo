﻿using Services.Administration;
using Services.Financial;
using Services.Inventory;
using Services.Purchasing;
using Services.Sales;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Web.ModelsApi.Financial;
using Web.ModelsApi.Inventory;
using Web.ModelsApi.Purchasing;
using Web.ModelsApi.Sales;

namespace Web.ControllersApi
{
    public class CommonController : ApiController
    {
        private readonly IFinancialService _financialService = null;
        private readonly IInventoryService _inventoryService = null;
        private readonly ISalesService _salesService = null;
        private readonly IAdministrationService _administrationService = null;
        private readonly IPurchasingService _purchasingService = null;
        public CommonController(IInventoryService inventoryService,
            IFinancialService financialService,
            IPurchasingService purchasingService,
            ISalesService salesService,
            IAdministrationService administrationService)
        {
            _inventoryService = inventoryService;
            _financialService = financialService;
            _purchasingService = purchasingService;
            _salesService = salesService;
            _administrationService = administrationService;
        }

        [HttpGet]
        [Route("api/common/vendors")]
        public IHttpActionResult Vendors()
        {
            var vendors = _purchasingService.GetVendors();
            ICollection<VendorDto> vendorsDto = new HashSet<VendorDto>();

            foreach (var vendor in vendors)
                vendorsDto.Add(new VendorDto() { Id = vendor.Id, Name = vendor.Name });

            return Ok(vendorsDto.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/customers")]
        public IHttpActionResult Customers()
        {
            var customers = _salesService.GetCustomers();
            ICollection<CustomerDto> customersDto = new HashSet<CustomerDto>();
            
            foreach (var customer in customers)
                customersDto.Add(new CustomerDto() { Id = customer.Id, Name = customer.Name });

            return Ok(customersDto.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/contacts")]
        public IHttpActionResult Contacts()
        {
            var contacts = _salesService.GetContacts();
            return Ok(contacts.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/paymentterms")]
        public IHttpActionResult PaymentTerms()
        {
            var paymentterms = _salesService.GetPaymentTerms();
            return Ok(paymentterms.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/items")]
        public IHttpActionResult Items()
        {
            var items = _inventoryService.GetAllItems();
            ICollection<ItemDto> itemsDto = new HashSet<ItemDto>();
            
            foreach (var item in items)
                itemsDto.Add(new ItemDto() { No = item.No, Description = item.Description });

            return Ok(itemsDto.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/accounts")]
        public IHttpActionResult Accounts()
        {
            var accounts = _financialService.GetAccounts();
            ICollection<AccountDto> accountsDto = new HashSet<AccountDto>();

            foreach (var account in accounts)
                accountsDto.Add(new AccountDto() { Id = account.Id, AccountName = account.AccountName });

            return Ok(accountsDto.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/measurements")]
        public IHttpActionResult Measurements()
        {
            return Ok(_inventoryService.GetMeasurements());
        }

        [HttpGet]
        [Route("api/common/itemcategories")]
        public IHttpActionResult ItemCategories()
        {
            var itemcategories = _inventoryService.GetItemCategories();
            ICollection<ItemCategoryDto> itemcategoriesDto = new HashSet<ItemCategoryDto>();

            foreach (var itemcategory in itemcategories)
                itemcategoriesDto.Add(new ItemCategoryDto() { Id = itemcategory.Id, Name = itemcategory.Name });

            return Ok(itemcategoriesDto.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/taxes")]
        public IHttpActionResult Taxes()
        {
            return Ok(_financialService.GetTaxes());
        }

        [HttpGet]
        [Route("api/common/itemtaxgroups")]
        public IHttpActionResult ItemTaxGroups()
        {
            var itemtaxgroups = _financialService.GetItemTaxGroups();
            ICollection<ItemTaxGroupDto> itemtaxgroupsDto = new HashSet<ItemTaxGroupDto>();

            foreach (var itemtaxgroup in itemtaxgroups)
                itemtaxgroupsDto.Add(new ItemTaxGroupDto() { Id = itemtaxgroup.Id, Name = itemtaxgroup.Name });

            return Ok(itemtaxgroupsDto.AsEnumerable());
        }

        [HttpGet]
        [Route("api/common/taxgroups")]
        public IHttpActionResult TaxGroups()
        {
            return Ok(_financialService.GetTaxGroups());
        }

        [HttpGet]
        [Route("api/common/banks")]
        public IHttpActionResult Banks()
        {
            return Ok(_financialService.GetCashAndBanks());
        }
    }
}
