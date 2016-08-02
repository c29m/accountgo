﻿using Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Administration;
using Services.Sales;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class SalesController : Controller
    {
        private readonly IAdministrationService _adminService;
        private readonly ISalesService _salesService;

        public SalesController(IAdministrationService adminService,
            ISalesService salesService)
        {
            _adminService = adminService;
            _salesService = salesService;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveCustomer([FromBody]Dto.Sales.Customer customerDto)
        {
            bool isNew = customerDto.Id == 0;
            Core.Domain.Sales.Customer customer = null;

            if (isNew)
            {
                customer = new Core.Domain.Sales.Customer();                

                customer.Party = new Core.Domain.Party()
                {
                    PartyType = Core.Domain.PartyTypes.Customer,
                };

                customer.PrimaryContact = new Core.Domain.Contact()
                {
                    ContactType = Core.Domain.ContactTypes.Customer,
                    Party = new Core.Domain.Party() {
                        PartyType = Core.Domain.PartyTypes.Contact
                    }                                   
                };                
            }
            else
            {
                customer = _salesService.GetCustomerById(customerDto.Id);
            }

            customer.No = customerDto.No;
            customer.Party.Name = customerDto.Name;
            customer.Party.Phone = customerDto.Phone;
            customer.Party.Email = customerDto.Email;
            customer.Party.Fax = customerDto.Fax;
            customer.Party.Website = customerDto.Website;
            customer.PrimaryContact.FirstName = customerDto.PrimaryContact.FirstName;
            customer.PrimaryContact.LastName = customerDto.PrimaryContact.LastName;
            customer.PrimaryContact.Party.Name = customerDto.PrimaryContact.Party.Name;
            customer.PrimaryContact.Party.Phone = customerDto.PrimaryContact.Party.Phone;
            customer.PrimaryContact.Party.Email = customerDto.PrimaryContact.Party.Email;
            customer.PrimaryContact.Party.Fax = customerDto.PrimaryContact.Party.Fax;
            customer.PrimaryContact.Party.Website = customerDto.PrimaryContact.Party.Website;
            customer.AccountsReceivableAccountId = customerDto.AccountsReceivableId;
            customer.SalesAccountId = customerDto.SalesAccountId;
            customer.CustomerAdvancesAccountId = customerDto.PrepaymentAccountId;
            customer.SalesDiscountAccountId = customerDto.SalesDiscountAccountId;
            customer.PaymentTermId = customerDto.PaymentTermId;
            customer.TaxGroupId = customerDto.TaxGroupId;

            if(isNew)
                _salesService.AddCustomer(customer);
            else
                _salesService.UpdateCustomer(customer);

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Customer(int id)
        {
            try
            {
                var customer = _salesService.GetCustomerById(id);

                var customerDto = new Dto.Sales.Customer()
                {
                    Id = customer.Id,
                    No = customer.No,                  
                    AccountsReceivableId = customer.AccountsReceivableAccountId.GetValueOrDefault(),
                    SalesAccountId = customer.SalesAccountId.GetValueOrDefault(),
                    PrepaymentAccountId = customer.CustomerAdvancesAccountId.GetValueOrDefault(),
                    SalesDiscountAccountId = customer.SalesDiscountAccountId.GetValueOrDefault(),
                    PaymentTermId = customer.PaymentTermId.GetValueOrDefault(),
                    TaxGroupId = customer.TaxGroupId.GetValueOrDefault()
                };
                customerDto.Name = customer.Party.Name;
                customerDto.Email = customer.Party.Email;
                customerDto.Website = customer.Party.Website;
                customerDto.Phone = customer.Party.Phone;
                customerDto.Fax = customer.Party.Fax;

                if (customer.PrimaryContact != null) {
                    customerDto.PrimaryContact.FirstName = customer.PrimaryContact.FirstName;
                    customerDto.PrimaryContact.LastName = customer.PrimaryContact.LastName;
                    customerDto.PrimaryContact.Party.Email = customer.PrimaryContact.Party.Email;
                    customerDto.PrimaryContact.Party.Phone = customer.PrimaryContact.Party.Phone;
                    customerDto.PrimaryContact.Party.Fax = customer.PrimaryContact.Party.Fax;
                    customerDto.PrimaryContact.Party.Website = customer.PrimaryContact.Party.Website;
                    customerDto.PrimaryContact.Party.Name = customer.PrimaryContact.Party.Name;
                }

                return new ObjectResult(customerDto);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateCustomer([FromBody]Dto.Sales.CreateCustomer Dto)
        {
            try
            {
                var customer = new Core.Domain.Sales.Customer()
                {
                    Party = new Core.Domain.Party()
                    {
                        PartyType = Core.Domain.PartyTypes.Customer,
                        Name = Dto.Name,
                        Phone = Dto.Phone,
                    },
                };

                _salesService.AddCustomer(customer);

                var customerDto = new Dto.Sales.Customer();
                customerDto.Id = customer.Id;
                customerDto.No = customer.No;
                customerDto.Name = customer.Party.Name;
                customerDto.Phone = customer.Party.Phone;

                return new ObjectResult(customerDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Customers()
        {
            IList<Dto.Sales.Customer> customersDto = new List<Dto.Sales.Customer>();
            try
            {
                var customers = _salesService.GetCustomers().Where(p => p.Party != null);
                foreach (var customer in customers)
                {
                    var customerDto = new Dto.Sales.Customer()
                    {
                        Id = customer.Id,
                        No = customer.No
                    };

                    customerDto.Name = customer.Party.Name;
                    customerDto.Email = customer.Party.Email;
                    customerDto.Website = customer.Party.Website;
                    customerDto.Phone = customer.Party.Phone;
                    customerDto.Fax = customer.Party.Fax;

                    customersDto.Add(customerDto);
                }

                return new ObjectResult(customersDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SalesOrders()
        {
            var salesOrders = _salesService.GetSalesOrders();
            IList<Dto.Sales.SalesOrder> salesOrdersDto = new List<Dto.Sales.SalesOrder>();

            foreach (var salesOrder in salesOrders)
            {
                var salesOrderDto = new Dto.Sales.SalesOrder()
                {
                    Id = salesOrder.Id,
                    CustomerId = salesOrder.CustomerId.Value,
                    CustomerNo = salesOrder.Customer.No,
                    CustomerName = salesOrder.Customer.Party.Name,
                    OrderDate = salesOrder.Date,
                    Amount = salesOrder.SalesOrderLines.Sum(l => l.Amount)
                };

                salesOrdersDto.Add(salesOrderDto);
            }

            return new ObjectResult(salesOrdersDto);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SalesOrder(int id)
        {
            try
            {
                var salesOrder = _salesService.GetSalesOrderById(id);

                var salesOrderDto = new Dto.Sales.SalesOrder()
                {
                    Id = salesOrder.Id,
                    CustomerId = salesOrder.CustomerId.Value,
                    CustomerNo = salesOrder.Customer.No,
                    CustomerName = _salesService.GetCustomerById(salesOrder.CustomerId.Value).Party.Name,
                    OrderDate = salesOrder.Date,
                    Amount = salesOrder.SalesOrderLines.Sum(l => l.Amount),
                    SalesOrderLines = new List<Dto.Sales.SalesOrderLine>()
                };

                foreach (var line in salesOrder.SalesOrderLines)
                {
                    var lineDto = new Dto.Sales.SalesOrderLine();
                    lineDto.Id = line.Id;
                    lineDto.Amount = line.Amount;
                    lineDto.Discount = line.Discount;
                    lineDto.Quantity = line.Quantity;
                    lineDto.ItemId = line.ItemId;
                    lineDto.ItemDescription = line.Item.Description;
                    lineDto.MeasurementId = line.MeasurementId;
                    lineDto.MeasurementDescription = line.Measurement.Description;

                    salesOrderDto.SalesOrderLines.Add(lineDto);
                }

                return new ObjectResult(salesOrderDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SalesInvoice(int id)
        {
            try
            {
                var salesInvoice = _salesService.GetSalesInvoiceById(id);

                var salesOrderDto = new Dto.Sales.SalesInvoice()
                {
                    Id = salesInvoice.Id,
                    CustomerId = salesInvoice.CustomerId,
                    CustomerName = salesInvoice.Customer.Party.Name,
                    InvoiceDate = salesInvoice.Date,
                    TotalAmount = salesInvoice.SalesInvoiceLines.Sum(l => l.Amount),
                    SalesInvoiceLines = new List<Dto.Sales.SalesInvoiceLine>()
                };

                foreach (var line in salesInvoice.SalesInvoiceLines)
                {
                    var lineDto = new Dto.Sales.SalesInvoiceLine();
                    lineDto.Id = line.Id;
                    lineDto.Amount = line.Amount;
                    lineDto.Discount = line.Discount;
                    lineDto.Quantity = line.Quantity;
                    lineDto.ItemId = line.ItemId;
                    lineDto.MeasurementId = line.MeasurementId;

                    salesOrderDto.SalesInvoiceLines.Add(lineDto);
                }

                return new ObjectResult(salesOrderDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddSalesOrder([FromBody]Dto.Sales.SalesOrder salesorderDto)
        {
            try
            {
                var salesOrder = new Core.Domain.Sales.SalesOrderHeader()
                {
                    CustomerId = salesorderDto.CustomerId,
                    Date = salesorderDto.OrderDate,
                };

                foreach (var line in salesorderDto.SalesOrderLines)
                {
                    var salesOrderLine = new Core.Domain.Sales.SalesOrderLine();
                    salesOrderLine.Amount = line.Amount;
                    salesOrderLine.Discount = line.Discount;
                    salesOrderLine.Quantity = line.Quantity;
                    salesOrderLine.ItemId = line.ItemId;
                    salesOrderLine.MeasurementId = line.MeasurementId;

                    salesOrder.SalesOrderLines.Add(salesOrderLine);
                }

                _salesService.AddSalesOrder(salesOrder, true);

                salesorderDto.Id = salesOrder.Id;

                return new ObjectResult(salesorderDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Quotations()
        {
            var quotes = _salesService.GetSalesQuotes();

            var quoteDtos = new List<Dto.Sales.SalesQuotation>();

            foreach (var quote in quotes) {
                var quoteDto = new Dto.Sales.SalesQuotation()
                {
                    Id = quote.Id,
                    CustomerId = quote.CustomerId,
                    CustomerName = quote.Customer.Party.Name,
                    PaymentTermId = quote.CustomerId,
                    QuotationDate = quote.Date,                 
                };

                foreach (var line in quote.SalesQuoteLines) {
                    var lineDto = new Dto.Sales.SalesQuotationLine()
                    {
                        ItemId = line.ItemId,
                        MeasurementId = line.MeasurementId,
                        Quantity = line.Quantity,
                        Amount = line.Amount,
                        Discount = line.Discount
                    };
                    quoteDto.SalesQuotationLines.Add(lineDto);
                }

                quoteDtos.Add(quoteDto);
            }

            return new ObjectResult(quoteDtos);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Quotation(int id)
        {
            var quote = _salesService.GetSalesQuotationById(id);

            var quoteDto = new Dto.Sales.SalesQuotation()
            {
                Id = quote.Id,
                CustomerId = quote.CustomerId,
                CustomerName = quote.Customer.Party.Name,                
                QuotationDate = quote.Date,
            };


            foreach (var line in quote.SalesQuoteLines)
            {
                var lineDto = new Dto.Sales.SalesQuotationLine()
                {
                    Id = line.Id,
                    ItemId = line.ItemId,
                    MeasurementId = line.MeasurementId,
                    Quantity = line.Quantity,
                    Amount = line.Amount,
                    Discount = line.Discount
                };
                quoteDto.SalesQuotationLines.Add(lineDto);
            }

            return new ObjectResult(quoteDto);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SalesInvoices()
        {
            var salesInvoices = _salesService.GetSalesInvoices();
            IList<Dto.Sales.SalesInvoice> salesInvoicesDto = new List<Dto.Sales.SalesInvoice>();

            foreach (var salesInvoice in salesInvoices)
            {
                var salesInvoiceDto = new Dto.Sales.SalesInvoice()
                {
                    Id = salesInvoice.Id,
                    CustomerId = salesInvoice.CustomerId,
                    CustomerName = salesInvoice.Customer.Party.Name,
                    InvoiceDate = salesInvoice.Date,
                    TotalAmount = salesInvoice.SalesInvoiceLines.Sum(l => l.Amount)
                };

                salesInvoicesDto.Add(salesInvoiceDto);
            }

            return new ObjectResult(salesInvoicesDto);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SalesReceipts()
        {
            var salesReceipts = _salesService.GetSalesReceipts();
            IList<Dto.Sales.SalesReceipt> salesReceiptsDto = new List<Dto.Sales.SalesReceipt>();

            foreach (var salesReceipt in salesReceipts)
            {
                var salesReceiptDto = new Dto.Sales.SalesReceipt()
                {
                    Id = salesReceipt.Id,
                    ReceiptNo = salesReceipt.No,
                    CustomerId = salesReceipt.CustomerId,
                    CustomerName = salesReceipt.Customer.Party.Name,
                    ReceiptDate = salesReceipt.Date,
                    Amount = salesReceipt.Amount,
                    RemainingAmountToAllocate = salesReceipt.AvailableAmountToAllocate
                };

                salesReceiptsDto.Add(salesReceiptDto);
            }

            return new ObjectResult(salesReceiptsDto);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SalesReceipt(int id)
        {
            var salesReceipt = _salesService.GetSalesReceiptById(id);
            var salesReceiptDto = new Dto.Sales.SalesReceipt()
            {
                Id = salesReceipt.Id,
                ReceiptNo = salesReceipt.No,
                CustomerId = salesReceipt.CustomerId,
                CustomerName = salesReceipt.Customer.Party.Name,
                ReceiptDate = salesReceipt.Date,
                Amount = salesReceipt.Amount,
                RemainingAmountToAllocate = salesReceipt.AvailableAmountToAllocate
            };

            return new ObjectResult(salesReceiptDto);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult CustomerInvoices(int id)
        {
            try
            {
                var invoices = _salesService.GetCustomerInvoices(id);

                var invoicesDto = new HashSet<Dto.Sales.SalesInvoice>();

                foreach (var invoice in invoices)
                {
                    var invoiceDto = new Dto.Sales.SalesInvoice()
                    {
                        Id = invoice.Id,
                        InvoiceDate = invoice.Date,
                        CustomerId = invoice.CustomerId,
                        TotalAmount = invoice.ComputeTotalAmount(),
                        TotalAllocatedAmount = (decimal)invoice.CustomerAllocations.Sum(i => i.Amount)
                    };

                    invoicesDto.Add(invoiceDto);
                }

                return new ObjectResult(invoicesDto);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveSalesOrder([FromBody]Dto.Sales.SalesOrder salesOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            try
            {
                bool isNew = salesOrderDto.Id == 0;
                Core.Domain.Sales.SalesOrderHeader salesOrder = null;

                if (isNew)
                {
                    salesOrder = new Core.Domain.Sales.SalesOrderHeader();
                }
                else
                {
                    salesOrder = _salesService.GetSalesOrderById(salesOrderDto.Id);
                }

                if (isNew)
                {
                    //_salesService.AddSalesOrder(salesOrder, true);
                }
                else
                {

                }
                return new OkObjectResult(Ok());
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveSalesInvoice([FromBody]Dto.Sales.SalesInvoice salesInvoiceDto)
        {
            ModelState.AddModelError("", "Testing - sample errors.");
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            bool isNew = salesInvoiceDto.Id == 0;
            Core.Domain.Sales.SalesInvoiceHeader salesInvoice = null;

            try
            {
                if (isNew)
                {
                    salesInvoice = new Core.Domain.Sales.SalesInvoiceHeader();
                }
                else
                {
                    salesInvoice = _salesService.GetSalesInvoiceById(salesInvoiceDto.Id);
                }

                if (isNew)
                {
                    //_salesService.AddSalesOrder(salesOrder, true);
                }
                else
                {

                }
                return new OkObjectResult(Ok());
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveQuotation([FromBody]Dto.Sales.SalesQuotation quotationDto)
        {
            if (!ModelState.IsValid) {
                return new ObjectResult(ModelState.Values);
            }

            try
            {
                var salesQuote = new Core.Domain.Sales.SalesQuoteHeader()
                {
                    CustomerId = quotationDto.CustomerId.GetValueOrDefault(),
                    Date = quotationDto.QuotationDate,
                };

                foreach (var line in quotationDto.SalesQuotationLines)
                {
                    var salesQuoteLine = new Core.Domain.Sales.SalesQuoteLine();
                    salesQuoteLine.Amount = line.Amount == null ? 0 : line.Amount.Value;
                    salesQuoteLine.Discount = line.Discount == null ? 0 : line.Discount.Value;
                    salesQuoteLine.Quantity = line.Quantity == null ? 0 : line.Quantity.Value;
                    salesQuoteLine.ItemId = line.ItemId.GetValueOrDefault();
                    salesQuoteLine.MeasurementId = line.MeasurementId.GetValueOrDefault();

                    salesQuote.SalesQuoteLines.Add(salesQuoteLine);
                }

                //_salesService.AddSalesQuote(salesQuote);

                return new OkObjectResult(Ok());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorMessage", ex.Message);
                return new ObjectResult(ModelState.Values);
            }
        }
    }
}