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

import { PaymentTerms } from './payment-terms';
 /**
 * 
 *
 * @export
 * @interface PaymentCondition
 */
export interface PaymentCondition {

    /**
     * @type {string}
     * @memberof PaymentCondition
     */
    name?: string | null;

    /**
     * @type {Array<PaymentTerms>}
     * @memberof PaymentCondition
     */
    terms?: Array<PaymentTerms> | null;
}