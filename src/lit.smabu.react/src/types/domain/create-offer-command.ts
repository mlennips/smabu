import { Currency } from './currency';
import { CustomerId } from './customer-id';
import { OfferId } from './offer-id';
import { OfferNumber } from './offer-number';
import { TaxRate } from './tax-rate';

export interface CreateOfferCommand {
  offerId?: OfferId;
  customerId?: CustomerId;
  currency?: Currency;
  taxRate?: TaxRate;
  number?: OfferNumber;
}
