
import { InvoiceIdOrderReferenceDTO } from './invoice-id-order-reference-dto';
import { OfferIdOrderReferenceDTO } from './offer-id-order-reference-dto';

export interface GetOrderReferencesResponse {
  offers?: Array<OfferIdOrderReferenceDTO> | null;
  invoices?: Array<InvoiceIdOrderReferenceDTO> | null;
  offerAmount?: number;
  invoiceAmount?: number;
}
