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

import { Currency } from './currency';
import { Dataset } from './dataset';
import { SalesAmountItem } from './sales-amount-item';
 /**
 * 
 *
 * @export
 * @interface GetSalesDashboardReadModel
 */
export interface GetSalesDashboardReadModel {

    /**
     * @type {Date}
     * @memberof GetSalesDashboardReadModel
     */
    version?: Date;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    thisYear?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    lastYear?: number;

    /**
     * @type {Currency}
     * @memberof GetSalesDashboardReadModel
     */
    currency?: Currency;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    totalSales?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    salesThisYear?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    salesLastYear?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    salesLast12Month?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    salesLast24Month?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    salesLast36Month?: number;

    /**
     * @type {Array<SalesAmountItem>}
     * @memberof GetSalesDashboardReadModel
     */
    top3InvoicesEver?: Array<SalesAmountItem> | null;

    /**
     * @type {Array<SalesAmountItem>}
     * @memberof GetSalesDashboardReadModel
     */
    top3InvoicesLast12Month?: Array<SalesAmountItem> | null;

    /**
     * @type {Array<SalesAmountItem>}
     * @memberof GetSalesDashboardReadModel
     */
    top3CustomersEver?: Array<SalesAmountItem> | null;

    /**
     * @type {Array<SalesAmountItem>}
     * @memberof GetSalesDashboardReadModel
     */
    top3CustomersLast12Month?: Array<SalesAmountItem> | null;

    /**
     * @type {Dataset}
     * @memberof GetSalesDashboardReadModel
     */
    salesByYear?: Dataset;

    /**
     * @type {Array<SalesAmountItem>}
     * @memberof GetSalesDashboardReadModel
     */
    salesByCustomer?: Array<SalesAmountItem> | null;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    invoiceCount?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    customerCount?: number;

    /**
     * @type {number}
     * @memberof GetSalesDashboardReadModel
     */
    orderCount?: number;
}
