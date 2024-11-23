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

import { AnnualFinancialStatementId } from './annual-financial-statement-id';
import { Currency } from './currency';
import { DatePeriod } from './date-period';
import { FinancialStatementStatus } from './financial-statement-status';
import { FinancialTransaction } from './financial-transaction';
 /**
 * 
 *
 * @export
 * @interface AnnualFinancialStatementDTO
 */
export interface AnnualFinancialStatementDTO {

    /**
     * @type {AnnualFinancialStatementId}
     * @memberof AnnualFinancialStatementDTO
     */
    id?: AnnualFinancialStatementId;

    /**
     * @type {number}
     * @memberof AnnualFinancialStatementDTO
     */
    fiscalYear?: number;

    /**
     * @type {string}
     * @memberof AnnualFinancialStatementDTO
     */
    displayName?: string;

    /**
     * @type {DatePeriod}
     * @memberof AnnualFinancialStatementDTO
     */
    period?: DatePeriod;

    /**
     * @type {Currency}
     * @memberof AnnualFinancialStatementDTO
     */
    currency?: Currency;

    /**
     * @type {Array<FinancialTransaction>}
     * @memberof AnnualFinancialStatementDTO
     */
    incomes?: Array<FinancialTransaction> | null;

    /**
     * @type {Array<FinancialTransaction>}
     * @memberof AnnualFinancialStatementDTO
     */
    expenditures?: Array<FinancialTransaction> | null;

    /**
     * @type {FinancialStatementStatus}
     * @memberof AnnualFinancialStatementDTO
     */
    status?: FinancialStatementStatus;

    /**
     * @type {number}
     * @memberof AnnualFinancialStatementDTO
     */
    totalIncome?: number;

    /**
     * @type {number}
     * @memberof AnnualFinancialStatementDTO
     */
    totalExpenditure?: number;

    /**
     * @type {number}
     * @memberof AnnualFinancialStatementDTO
     */
    netIncome?: number;
}