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

import { CustomerDTO } from './customer-dto';
import { InvoiceIdOrderReferenceItem } from './invoice-id-order-reference-item';
import { OfferIdOrderReferenceItem } from './offer-id-order-reference-item';
import { OrderId } from './order-id';
import { OrderNumber } from './order-number';
import { OrderStatus } from './order-status';
 /**
 * 
 *
 * @export
 * @interface OrderDTO
 */
export interface OrderDTO {

    /**
     * @type {string}
     * @memberof OrderDTO
     */
    displayName?: string | null;

    /**
     * @type {OrderId}
     * @memberof OrderDTO
     */
    id?: OrderId;

    /**
     * @type {OrderNumber}
     * @memberof OrderDTO
     */
    number?: OrderNumber;

    /**
     * @type {Date}
     * @memberof OrderDTO
     */
    createdOn?: Date;

    /**
     * @type {CustomerDTO}
     * @memberof OrderDTO
     */
    customer?: CustomerDTO;

    /**
     * @type {string}
     * @memberof OrderDTO
     */
    name?: string | null;

    /**
     * @type {string}
     * @memberof OrderDTO
     */
    description?: string | null;

    /**
     * @type {string}
     * @memberof OrderDTO
     */
    orderDate?: string;

    /**
     * @type {Date}
     * @memberof OrderDTO
     */
    deadline?: Date | null;

    /**
     * @type {string}
     * @memberof OrderDTO
     */
    orderGroup?: string | null;

    /**
     * @type {OrderStatus}
     * @memberof OrderDTO
     */
    status?: OrderStatus;

    /**
     * @type {Array<OfferIdOrderReferenceItem>}
     * @memberof OrderDTO
     */
    offers?: Array<OfferIdOrderReferenceItem> | null;

    /**
     * @type {Array<InvoiceIdOrderReferenceItem>}
     * @memberof OrderDTO
     */
    invoices?: Array<InvoiceIdOrderReferenceItem> | null;
}
