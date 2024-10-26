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
 /**
 * 
 *
 * @export
 * @interface GetWelcomeDashboardReadModel
 */
export interface GetWelcomeDashboardReadModel {

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    thisYear?: number;

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    lastYear?: number;

    /**
     * @type {Currency}
     * @memberof GetWelcomeDashboardReadModel
     */
    currency?: Currency;

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    salesVolumeThisYear?: number;

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    salesVolumeLastYear?: number;

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    invoiceCount?: number;

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    offerCount?: number;

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    customerCount?: number;

    /**
     * @type {number}
     * @memberof GetWelcomeDashboardReadModel
     */
    totalSalesVolume?: number;

    /**
     * @type {Date}
     * @memberof GetWelcomeDashboardReadModel
     */
    version?: Date;
}
