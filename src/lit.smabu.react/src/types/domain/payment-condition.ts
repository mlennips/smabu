import { PaymentTerms } from './payment-terms';

export interface PaymentCondition {
  name?: string | null;
  terms?: Array<PaymentTerms> | null;
}
