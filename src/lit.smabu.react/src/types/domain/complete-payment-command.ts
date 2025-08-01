
import { PaymentId } from './payment-id';

export interface CompletePaymentCommand {
  paymentId?: PaymentId;
  amount?: number;
  paidAt?: Date;
}
