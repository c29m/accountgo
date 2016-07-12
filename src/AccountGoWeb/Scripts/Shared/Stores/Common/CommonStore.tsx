﻿import {observable, extendObservable, action} from 'mobx';
import * as axios from "axios";
import * as d3 from "d3";

import Config = require("Config");

export default class CommonStore {
    @observable customers = [];
    @observable paymentTerms = [];
    @observable items = [];
    @observable measurements = [];
    @observable vendors = [];

    constructor() {
        this.loadCustomersLookup();
        this.loadPaymentTermsLookup();
        this.loadItemsLookup();
        this.loadMeasurementsLookup();
        this.loadVendorsLookup();
    }

    loadCustomersLookup() {
        let customers = this.customers;
        axios.get(Config.apiUrl + "api/common/customers")
            .then(function (result) {
                const data = result.data as [];
                for (var i = 0; i < data.length; i++) {
                    customers.push(data[i]);
                }
            });
    }

    loadPaymentTermsLookup() {
        let paymentTerms = this.paymentTerms;
        axios.get(Config.apiUrl + "api/common/paymentterms")
            .then(function (result) {
                const data = result.data as [];
                for (var i = 0; i < data.length; i++) {
                    paymentTerms.push(data[i]);
                }
            });
    }

    loadVendorsLookup() {
        let vendors = this.vendors;
        axios.get(Config.apiUrl + "api/common/vendors")
            .then(function (result) {
                const data = result.data as [];
                for (var i = 0; i < data.length; i++) {
                    vendors.push(data[i]);
                }
            });
    }

    loadItemsLookup() {
        let items = this.items;
        axios.get(Config.apiUrl + "api/common/items")
            .then(function (result) {
                const data = result.data as [];
                for (var i = 0; i < data.length; i++) {
                    items.push(data[i]);
                }
            });
    }

    loadMeasurementsLookup() {
        let measurements = this.measurements;
        axios.get(Config.apiUrl + "api/common/measurements")
            .then(function (result) {
                const data = result.data as [];
                for (var i = 0; i < data.length; i++) {
                    measurements.push(data[i]);
                }
            });
    }
}