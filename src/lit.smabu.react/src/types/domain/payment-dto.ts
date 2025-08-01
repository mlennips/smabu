


import { Currency } from './currency';
import { CustomerId } from './customer-id';
import { InvoiceId } from './invoice-id';
import { PaymentCondition } from './payment-condition';
import { PaymentDirection } from './payment-direction';
import { PaymentId } from './payment-id';
import { PaymentMethod } from './payment-method';
import { PaymentNumber } from './payment-number';
import { PaymentStatus } from './payment-status';
export interface PaymentDTO {
  id?: PaymentId;
  number?: PaymentNumber;
  direction?: PaymentDirection;
  details?: string | null;
  payer?: string | null;
  payee?: string | null;
  customerId?: CustomerId;
  invoiceId?: InvoiceId;
  referenceNr?: string | null;
  referenceDate?: Date | null;
  amountDue?: number;
  dueDate?: Date | null;
  isOverdue?: boolean;
  amountPaid?: number;
  paidAt?: Date | null;
  accountingDate?: Date | null;
  currency?: Currency;
  paymentMethod?: PaymentMethod;
  status?: PaymentStatus;
  paymentCondition?: PaymentCondition;
  displayName?: string | null;
}
