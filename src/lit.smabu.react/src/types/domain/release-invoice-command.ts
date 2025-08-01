import { InvoiceId } from './invoice-id';
import { InvoiceNumber } from './invoice-number';
export interface ReleaseInvoiceCommand {
  invoiceId?: InvoiceId;
  number?: InvoiceNumber;
  releasedAt?: Date | null;
}
