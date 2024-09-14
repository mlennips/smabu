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
import { ProductId } from './product-id';
import { Quantity } from './quantity';
 /**
 * 
 *
 * @export
 * @interface InvoiceItemDTO
 */
export interface InvoiceItemDTO {

    /**
     * @type {string}
     * @memberof InvoiceItemDTO
     */
    displayName?: string | null;

    /**
     * @type {InvoiceItemId}
     * @memberof InvoiceItemDTO
     */
    id?: InvoiceItemId;

    /**
     * @type {InvoiceId}
     * @memberof InvoiceItemDTO
     */
    invoiceId?: InvoiceId;

    /**
     * @type {number}
     * @memberof InvoiceItemDTO
     */
    position?: number;

    /**
     * @type {string}
     * @memberof InvoiceItemDTO
     */
    details?: string | null;

    /**
     * @type {Quantity}
     * @memberof InvoiceItemDTO
     */
    quantity?: Quantity;

    /**
     * @type {number}
     * @memberof InvoiceItemDTO
     */
    unitPrice?: number;

    /**
     * @type {number}
     * @memberof InvoiceItemDTO
     */
    totalPrice?: number;

    /**
     * @type {ProductId}
     * @memberof InvoiceItemDTO
     */
    productId?: ProductId;
}