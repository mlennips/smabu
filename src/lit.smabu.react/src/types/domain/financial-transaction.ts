import { FinancialCategory } from './financial-category';
import { PaymentId } from './payment-id';

export interface FinancialTransaction {
  date?: Date;
  amount?: number;
  description?: string | null;
  category?: FinancialCategory;
  paymentId?: PaymentId;
  isImported?: boolean;
}
