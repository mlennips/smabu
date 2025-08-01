import { PaymentCondition } from './payment-condition';
import { PaymentId } from './payment-id';
import { PaymentMethod } from './payment-method';
import { PaymentStatus } from './payment-status';

export interface UpdatePaymentCommand {
  paymentId?: PaymentId;
  details?: string | null;
  payer?: string | null;
  payee?: string | null;
  referenceNr?: string | null;
  referenceDate?: Date | null;
  accountingDate?: Date;
  amountDue?: number;
  dueDate?: Date | null;
  paymentMethod?: PaymentMethod;
  status?: PaymentStatus;
  paymentCondition?: PaymentCondition;
}
