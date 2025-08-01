
import { InvoiceDTO } from './invoice-dto';
import { PaymentDTO } from './payment-dto';

export interface GetInvoiceDTO extends InvoiceDTO {
  payments: Array<PaymentDTO>;
  isPaid: boolean;
}