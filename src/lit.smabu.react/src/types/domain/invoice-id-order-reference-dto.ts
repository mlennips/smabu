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
 /**
 * 
 *
 * @export
 * @interface InvoiceIdOrderReferenceDTO
 */
export interface InvoiceIdOrderReferenceDTO {

    /**
     * @type {InvoiceId}
     * @memberof InvoiceIdOrderReferenceDTO
     */
    id?: InvoiceId;

    /**
     * @type {string}
     * @memberof InvoiceIdOrderReferenceDTO
     */
    name?: string | null;

    /**
     * @type {boolean}
     * @memberof InvoiceIdOrderReferenceDTO
     */
    isSelected?: boolean | null;

    /**
     * @type {string}
     * @memberof InvoiceIdOrderReferenceDTO
     */
    date?: string | null;

    /**
     * @type {number}
     * @memberof InvoiceIdOrderReferenceDTO
     */
    amount?: number | null;
}
