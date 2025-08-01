import { DatePeriod } from './date-period';
import { InvoiceId } from './invoice-id';
import { PaymentCondition } from './payment-condition';
import { TaxRate } from './tax-rate';
export interface UpdateInvoiceCommand {
  invoiceId?: InvoiceId;
  performancePeriod?: DatePeriod;
  taxRate?: TaxRate;
  invoiceDate?: string | null;
  paymentCondition?: PaymentCondition;
}
