import { Currency } from './currency';
import { CustomerId } from './customer-id';
import { DatePeriod } from './date-period';
import { InvoiceId } from './invoice-id';
import { TaxRate } from './tax-rate';

export interface CreateInvoiceCommand {
  invoiceId?: InvoiceId;
  customerId?: CustomerId;
  fiscalYear?: number;
  currency?: Currency;
  performancePeriod?: DatePeriod;
  taxRate?: TaxRate;
  templateId?: InvoiceId;
}
