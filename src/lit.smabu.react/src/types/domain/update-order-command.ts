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

import { OrderId } from './order-id';
import { OrderStatus } from './order-status';
 /**
 * 
 *
 * @export
 * @interface UpdateOrderCommand
 */
export interface UpdateOrderCommand {

    /**
     * @type {OrderId}
     * @memberof UpdateOrderCommand
     */
    id?: OrderId;

    /**
     * @type {string}
     * @memberof UpdateOrderCommand
     */
    name?: string | null;

    /**
     * @type {string}
     * @memberof UpdateOrderCommand
     */
    description?: string | null;

    /**
     * @type {string}
     * @memberof UpdateOrderCommand
     */
    orderDate?: string;

    /**
     * @type {string}
     * @memberof UpdateOrderCommand
     */
    orderGroup?: string | null;

    /**
     * @type {Date}
     * @memberof UpdateOrderCommand
     */
    deadline?: Date | null;

    /**
     * @type {OrderStatus}
     * @memberof UpdateOrderCommand
     */
    status?: OrderStatus;
}
