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
import { Transaction } from './transaction';
 /**
 * 
 *
 * @export
 * @interface UpdateAnnualFinancialStatementCommand
 */
export interface UpdateAnnualFinancialStatementCommand {

    /**
     * @type {AnnualFinancialStatementId}
     * @memberof UpdateAnnualFinancialStatementCommand
     */
    annualFinancialStatementId?: AnnualFinancialStatementId;

    /**
     * @type {number}
     * @memberof UpdateAnnualFinancialStatementCommand
     */
    fiscalYear?: number;

    /**
     * @type {Array<Transaction>}
     * @memberof UpdateAnnualFinancialStatementCommand
     */
    incomes?: Array<Transaction> | null;

    /**
     * @type {Array<Transaction>}
     * @memberof UpdateAnnualFinancialStatementCommand
     */
    expenditures?: Array<Transaction> | null;
}
