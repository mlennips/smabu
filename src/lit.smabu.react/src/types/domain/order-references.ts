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

import { IEntityId } from './ientity-id';
import { InvoiceId } from './invoice-id';
import { OfferId } from './offer-id';
 /**
 * 
 *
 * @export
 * @interface OrderReferences
 */
export interface OrderReferences {

    /**
     * @type {Array<OfferId>}
     * @memberof OrderReferences
     */
    offerIds?: Array<OfferId> | null;

    /**
     * @type {Array<InvoiceId>}
     * @memberof OrderReferences
     */
    invoiceIds?: Array<InvoiceId> | null;

    /**
     * @type {Array<IEntityId>}
     * @memberof OrderReferences
     */
    allReferenceIds?: Array<IEntityId> | null;
}
