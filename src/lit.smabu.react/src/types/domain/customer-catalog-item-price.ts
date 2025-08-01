
import { Currency } from './currency';
import { CustomerId } from './customer-id';

export interface CustomerCatalogItemPrice {
  price?: number;
  currency?: Currency;
  validFrom?: Date;
  customerId?: CustomerId;
}
