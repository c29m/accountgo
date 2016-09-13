﻿import * as React from "react";
import {observer} from "mobx-react";

@observer
export default class SelectLineItem extends React.Component<any, {}>{
    onChangeItem(e) {
        if (this.props.row !== undefined)
            this.props.store.updateLineItem(this.props.row, "itemId", e.target.value);

        for (var i = 0; i < this.props.store.commonStore.items.length; i++) {
            if (this.props.store.commonStore.items[i].id === parseInt(e.target.value)) {


                if (this.props.row !== undefined) {
                    this.props.store.updateLineItem(this.props.row, "measurementId", this.props.store.commonStore.items[i].sellMeasurementId);
                    this.props.store.updateLineItem(this.props.row, "price", this.props.store.commonStore.items[i].price);
                    this.props.store.updateLineItem(this.props.row, "code", this.props.store.commonStore.items[i].code);


                }
                else {
                    (document.getElementById("optNewMeasurementId") as HTMLInputElement).value = this.props.store.commonStore.items[i].sellMeasurementId;
                    (document.getElementById("txtNewAmount") as HTMLInputElement).value = this.props.store.commonStore.items[i].price;
                    (document.getElementById("txtNewCode") as HTMLInputElement).value = this.props.store.commonStore.items[i].code;
                    
                }


            }
        }
    }

    render() {
        var options = [];
        this.props.store.commonStore.items.map(function (item) {
            return (
                options.push(<option key={ item.id } value={ item.id } > { item.description } </option>)
            );
        });

        return (
            <select value={this.props.selected} id={this.props.controlId} onChange={this.onChangeItem.bind(this) } className="form-control select2">
                <option key={ -1 }></option>
                {options}
            </select>
        );
    }
}