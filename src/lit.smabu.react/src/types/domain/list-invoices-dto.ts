import { InvoiceDTO } from './invoice-dto';

export interface ListInvoicesDTO extends InvoiceDTO {
  isPaid: boolean;
}