
import { Currency } from './currency';

export interface CatalogItemPrice {
  price?: number;
  currency?: Currency;
  validFrom?: Date;
}
