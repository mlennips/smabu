
import { CatalogGroupId } from './catalog-group-id';
import { CatalogId } from './catalog-id';
import { CatalogItemId } from './catalog-item-id';
import { CatalogItemNumber } from './catalog-item-number';
import { CatalogItemPrice } from './catalog-item-price';
import { Currency } from './currency';
import { CustomerCatalogItemPrice } from './customer-catalog-item-price';
import { Unit } from './unit';

export interface CatalogItemDTO {
  id?: CatalogItemId;
  catalogGroupId?: CatalogGroupId;
  catalogId?: CatalogId;
  number?: CatalogItemNumber;
  name?: string | null;
  description?: string | null;
  isActive?: boolean;
  groupName?: string | null;
  prices?: Array<CatalogItemPrice> | null;
  customerPrices?: Array<CustomerCatalogItemPrice> | null;
  unit?: Unit;
  currentPrice?: CatalogItemPrice;
  displayName?: string | null;
  currency?: Currency;
}
