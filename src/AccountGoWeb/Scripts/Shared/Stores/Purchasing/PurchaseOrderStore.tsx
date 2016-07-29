﻿import {observable, extendObservable, action} from 'mobx';
import * as axios from "axios";

import Config = require("Config");

import PurchaseOrder from './PurchaseOrder';
import PurchaseOrderLine from './PurchaseOrderLine';

import CommonStore from "../Common/CommonStore";

let baseUrl = location.protocol
    + "//" + location.hostname
    + (location.port && ":" + location.port)
    + "/";

export default class PurchaseOrderStore {
    purchaseOrder;
    commonStore;

    constructor() {
        this.purchaseOrder = new PurchaseOrder();
        extendObservable(this.purchaseOrder, {
            vendorId: this.purchaseOrder.vendorId,
            orderDate: this.purchaseOrder.orderDate,
            paymentTermId: this.purchaseOrder.paymentTermId,
            referenceNo: this.purchaseOrder.referenceNo,
            purchaseOrderLines: []
        });

        this.commonStore = new CommonStore();
    }
    
    changedVendor(vendorId) {
        this.purchaseOrder.vendorId = vendorId;
    }

    changedOrderDate(date) {
        this.purchaseOrder.orderDate = date;
    }

    addLineItem(itemId, measurementId, quantity, amount, discount) {
        var newLineItem = new PurchaseOrderLine(itemId, measurementId, quantity, amount, discount);
        this.purchaseOrder.purchaseOrderLines.push(extendObservable(newLineItem, newLineItem));        
    }

    removeLineItem(row) {
        this.purchaseOrder.purchaseOrderLines.splice(row, 1);
    }

    updateLineItem(row, targetProperty, value) {
        if (this.purchaseOrder.purchaseOrderLines.length > 0)
            this.purchaseOrder.purchaseOrderLines[row][targetProperty] = value;
    }

    grandTotal() {
        var sum = 0;
        for (var i = 0; i < this.purchaseOrder.purchaseOrderLines.length; i++) {
            var lineSum = this.purchaseOrder.purchaseOrderLines[i].quantity * this.purchaseOrder.purchaseOrderLines[i].amount;
            sum = sum + lineSum;
        }
        return sum;
    }

    lineTotal(row) {
        var lineSum = this.purchaseOrder.purchaseOrderLines[row].quantity * this.purchaseOrder.purchaseOrderLines[row].amount;
        return lineSum;
    }
}