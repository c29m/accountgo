﻿import SalesOrderLine from "./SalesOrderLine";

interface ISalesOrder {
    id;
    customerId;
    orderDate;
    paymentTermId;
    referenceNo;
    salesOrderLines: SalesOrderLine[];
}

export default class SalesOrder implements ISalesOrder {
    id: number;
    customerId: number;
    orderDate: Date;
    paymentTermId: number;
    referenceNo: string;
    salesOrderLines: SalesOrderLine[] = [];
    //constructor(customerId, orderDate, paymentTermId, referenceNo) {
    //    this.customerId = customerId;
    //    this.orderDate = orderDate;
    //    this.paymentTermid = paymentTermId;
    //    this.referenceNo = referenceNo;
    //}
}