import { Currency } from './currency';
import { CustomerDTO } from './customer-dto';
import { OfferId } from './offer-id';
import { OfferItemDTO } from './offer-item-dto';
import { OfferNumber } from './offer-number';
import { TaxRate } from './tax-rate';

export interface OfferDTO {
  displayName?: string | null;
  id?: OfferId;
  createdAt?: Date | null;
  customer?: CustomerDTO;
  number?: OfferNumber;
  offerDate?: string;
  expiresOn?: string;
  amount?: number;
  currency?: Currency;
  taxRate?: TaxRate;
  items?: Array<OfferItemDTO> | null;
}
