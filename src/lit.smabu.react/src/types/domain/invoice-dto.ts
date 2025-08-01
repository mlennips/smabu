
import { Currency } from './currency';
import { CustomerDTO } from './customer-dto';
import { DatePeriod } from './date-period';
import { InvoiceId } from './invoice-id';
import { InvoiceItemDTO } from './invoice-item-dto';
import { InvoiceNumber } from './invoice-number';
import { PaymentCondition } from './payment-condition';
import { TaxRate } from './tax-rate';

export interface InvoiceDTO {
  displayName?: string | null;
  id?: InvoiceId;
  createdAt?: Date | null;
  customer?: CustomerDTO;
  number?: InvoiceNumber;
  invoiceDate?: string | null;
  amount?: number;
  currency?: Currency;
  performancePeriod?: DatePeriod;
  fiscalYear?: number;
  taxRate?: TaxRate;
  isReleased?: boolean;
  releasedAt?: Date | null;
  items?: Array<InvoiceItemDTO> | null;
  paymentCondition?: PaymentCondition;
}
