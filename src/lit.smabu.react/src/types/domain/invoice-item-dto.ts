import { CatalogItemId } from './catalog-item-id';
import { InvoiceId } from './invoice-id';
import { InvoiceItemId } from './invoice-item-id';
import { Quantity } from './quantity';

export interface InvoiceItemDTO {
  displayName?: string | null;
  id?: InvoiceItemId;
  invoiceId?: InvoiceId;
  position?: number;
  details?: string | null;
  quantity?: Quantity;
  unitPrice?: number;
  totalPrice?: number;
  catalogItemId?: CatalogItemId;
}
