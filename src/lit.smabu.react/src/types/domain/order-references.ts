import { InvoiceId } from './invoice-id';
import { OfferId } from './offer-id';

export interface OrderReferences {
  offerIds?: Array<OfferId> | null;
  invoiceIds?: Array<InvoiceId> | null;
}
