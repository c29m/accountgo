"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const mobx_react_1 = require("mobx-react");
let SelectVoucherType = class SelectVoucherType extends React.Component {
    onChangeVoucherType(e) {
        this.props.store.changedVoucherType(e.target.value);
    }
    render() {
        var options = [];
        options.push(React.createElement("option", { key: "1", value: "1" }, " Opening Balances"));
        options.push(React.createElement("option", { key: "2", value: "2" }, " Closing Entries "));
        options.push(React.createElement("option", { key: "3", value: "3" }, " Adjustment Entries "));
        options.push(React.createElement("option", { key: "4", value: "4" }, " Correction Entries "));
        options.push(React.createElement("option", { key: "5", value: "5" }, " Transfer Entries "));
        return (React.createElement("select", { id: this.props.controlId, value: this.props.selected, onChange: this.onChangeVoucherType.bind(this), className: "form-control select2" },
            React.createElement("option", { key: -1, value: "" }),
            options));
    }
};
SelectVoucherType = __decorate([
    mobx_react_1.observer
], SelectVoucherType);
exports.default = SelectVoucherType;
//# sourceMappingURL=SelectVoucherType.js.map