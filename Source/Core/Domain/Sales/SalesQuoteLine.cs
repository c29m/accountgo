//-----------------------------------------------------------------------
// <copyright file="SalesQuoteLine.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{
    [Table("SalesQuoteLine")]
    public partial class SalesQuoteLine : BaseEntity
    {
        public int SalesQuoteHeaderId { get; set; }
        public SalesQuoteHeader SalesQuoteHeader { get; set; }
    }
}
