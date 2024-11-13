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

import { CustomerId } from './customer-id';
import { OrderId } from './order-id';
import { OrderNumber } from './order-number';
 /**
 * 
 *
 * @export
 * @interface CreateOrderCommand
 */
export interface CreateOrderCommand {

    /**
     * @type {OrderId}
     * @memberof CreateOrderCommand
     */
    orderId?: OrderId;

    /**
     * @type {CustomerId}
     * @memberof CreateOrderCommand
     */
    customerId?: CustomerId;

    /**
     * @type {string}
     * @memberof CreateOrderCommand
     */
    name?: string | null;

    /**
     * @type {Date}
     * @memberof CreateOrderCommand
     */
    orderDate?: Date;

    /**
     * @type {OrderNumber}
     * @memberof CreateOrderCommand
     */
    number?: OrderNumber;
}
