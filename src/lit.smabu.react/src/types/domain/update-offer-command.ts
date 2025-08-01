import { OfferId } from './offer-id';
import { TaxRate } from './tax-rate';

export interface UpdateOfferCommand {
  offerId?: OfferId;
  taxRate?: TaxRate;
  offerDate?: string;
  expiresOn?: string;
}
