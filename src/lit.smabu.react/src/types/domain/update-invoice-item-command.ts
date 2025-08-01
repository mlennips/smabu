import { CatalogItemId } from './catalog-item-id';
import { InvoiceId } from './invoice-id';
import { InvoiceItemId } from './invoice-item-id';
import { Quantity } from './quantity';

export interface UpdateInvoiceItemCommand {
  invoiceItemId?: InvoiceItemId;
  invoiceId?: InvoiceId;
  details?: string | null;
  quantity?: Quantity;
  unitPrice?: number;
  catalogItemId?: CatalogItemId;
}
