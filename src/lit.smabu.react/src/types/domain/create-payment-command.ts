import { CustomerId } from './customer-id';
import { InvoiceId } from './invoice-id';
import { PaymentCondition } from './payment-condition';
import { PaymentDirection } from './payment-direction';
import { PaymentId } from './payment-id';
import { PaymentMethod } from './payment-method';
 /**
 * 
 *
 * @export
 * @interface CreatePaymentCommand
 */
export interface CreatePaymentCommand {

    /**
     * @type {PaymentId}
     * @memberof CreatePaymentCommand
     */
    paymentId?: PaymentId;

    /**
     * @type {PaymentDirection}
     * @memberof CreatePaymentCommand
     */
    direction?: PaymentDirection;

    /**
     * @type {string}
     * @memberof CreatePaymentCommand
     */
    details?: string | null;

    /**
     * @type {string}
     * @memberof CreatePaymentCommand
     */
    payer?: string | null;

    /**
     * @type {string}
     * @memberof CreatePaymentCommand
     */
    payee?: string | null;

    /**
     * @type {CustomerId}
     * @memberof CreatePaymentCommand
     */
    customerId?: CustomerId;

    /**
     * @type {InvoiceId}
     * @memberof CreatePaymentCommand
     */
    invoiceId?: InvoiceId;

    /**
     * @type {string}
     * @memberof CreatePaymentCommand
     */
    referenceNr?: string | null;

    /**
     * @type {Date}
     * @memberof CreatePaymentCommand
     */
    referenceDate?: Date | null;

    /**
     * @type {number}
     * @memberof CreatePaymentCommand
     */
    amountDue?: number;

    /**
     * @type {Date}
     * @memberof CreatePaymentCommand
     */
    dueDate?: Date | null;

    /**
     * @type {PaymentMethod}
     * @memberof CreatePaymentCommand
     */
    paymentMethod?: PaymentMethod;

    /**
     * @type {PaymentCondition}
     * @memberof CreatePaymentCommand
     */
    paymentCondition?: PaymentCondition;

    /**
     * @type {boolean}
     * @memberof CreatePaymentCommand
     */
    markAsPaid?: boolean | null;
}
