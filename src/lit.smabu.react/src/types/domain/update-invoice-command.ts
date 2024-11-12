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

import { DatePeriod } from './date-period';
import { InvoiceId } from './invoice-id';
import { TaxRate } from './tax-rate';
 /**
 * 
 *
 * @export
 * @interface UpdateInvoiceCommand
 */
export interface UpdateInvoiceCommand {

    /**
     * @type {InvoiceId}
     * @memberof UpdateInvoiceCommand
     */
    invoiceId?: InvoiceId;

    /**
     * @type {DatePeriod}
     * @memberof UpdateInvoiceCommand
     */
    performancePeriod?: DatePeriod;

    /**
     * @type {TaxRate}
     * @memberof UpdateInvoiceCommand
     */
    taxRate?: TaxRate;

    /**
     * @type {string}
     * @memberof UpdateInvoiceCommand
     */
    invoiceDate?: string | null;
}
