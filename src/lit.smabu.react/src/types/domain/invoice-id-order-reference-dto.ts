import { InvoiceId } from './invoice-id';

export interface InvoiceIdOrderReferenceDTO {
  id?: InvoiceId;
  name?: string | null;
  isSelected?: boolean | null;
  date?: string | null;
  amount?: number | null;
}
