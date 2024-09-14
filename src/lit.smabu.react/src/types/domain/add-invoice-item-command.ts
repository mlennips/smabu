/* tslint:disable */
/* eslint-disable */
/**
 * LIT.Smabu.API
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

import { InvoiceId } from './invoice-id';
import { InvoiceItemId } from './invoice-item-id';
import { Quantity } from './quantity';
 /**
 * 
 *
 * @export
 * @interface AddInvoiceItemCommand
 */
export interface AddInvoiceItemCommand {

    /**
     * @type {InvoiceItemId}
     * @memberof AddInvoiceItemCommand
     */
    id: InvoiceItemId;

    /**
     * @type {InvoiceId}
     * @memberof AddInvoiceItemCommand
     */
    invoiceId: InvoiceId;

    /**
     * @type {string}
     * @memberof AddInvoiceItemCommand
     */
    details: string | null;

    /**
     * @type {Quantity}
     * @memberof AddInvoiceItemCommand
     */
    quantity: Quantity;

    /**
     * @type {number}
     * @memberof AddInvoiceItemCommand
     */
    unitPrice: number;
}