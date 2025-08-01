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
