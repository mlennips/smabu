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

import { OfferId } from './offer-id';
 /**
 * 
 *
 * @export
 * @interface OfferIdOrderReferenceItem
 */
export interface OfferIdOrderReferenceItem {

    /**
     * @type {OfferId}
     * @memberof OfferIdOrderReferenceItem
     */
    id?: OfferId;

    /**
     * @type {string}
     * @memberof OfferIdOrderReferenceItem
     */
    name?: string | null;

    /**
     * @type {string}
     * @memberof OfferIdOrderReferenceItem
     */
    date?: string | null;

    /**
     * @type {number}
     * @memberof OfferIdOrderReferenceItem
     */
    amount?: number | null;
}
