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
import { CustomerDTO } from './customer-dto';
import { OfferId } from './offer-id';
import { OfferItemDTO } from './offer-item-dto';
import { OfferNumber } from './offer-number';
import { TaxRate } from './tax-rate';
 /**
 * 
 *
 * @export
 * @interface OfferDTO
 */
export interface OfferDTO {

    /**
     * @type {string}
     * @memberof OfferDTO
     */
    displayName?: string | null;

    /**
     * @type {OfferId}
     * @memberof OfferDTO
     */
    id?: OfferId;

    /**
     * @type {Date}
     * @memberof OfferDTO
     */
    createdAt?: Date | null;

    /**
     * @type {CustomerDTO}
     * @memberof OfferDTO
     */
    customer?: CustomerDTO;

    /**
     * @type {OfferNumber}
     * @memberof OfferDTO
     */
    number?: OfferNumber;

    /**
     * @type {string}
     * @memberof OfferDTO
     */
    offerDate?: string;

    /**
     * @type {string}
     * @memberof OfferDTO
     */
    expiresOn?: string;

    /**
     * @type {number}
     * @memberof OfferDTO
     */
    amount?: number;

    /**
     * @type {Currency}
     * @memberof OfferDTO
     */
    currency?: Currency;

    /**
     * @type {TaxRate}
     * @memberof OfferDTO
     */
    taxRate?: TaxRate;

    /**
     * @type {Array<OfferItemDTO>}
     * @memberof OfferDTO
     */
    items?: Array<OfferItemDTO> | null;
}
