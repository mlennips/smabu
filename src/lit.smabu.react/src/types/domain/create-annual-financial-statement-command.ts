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
 /**
 * 
 *
 * @export
 * @interface CreateAnnualFinancialStatementCommand
 */
export interface CreateAnnualFinancialStatementCommand {

    /**
     * @type {AnnualFinancialStatementId}
     * @memberof CreateAnnualFinancialStatementCommand
     */
    annualFinancialStatementId?: AnnualFinancialStatementId;

    /**
     * @type {number}
     * @memberof CreateAnnualFinancialStatementCommand
     */
    fiscalYear?: number;
}
