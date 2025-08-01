import { CatalogId } from './catalog-id';
import { CatalogItemId } from './catalog-item-id';
import { CatalogItemPrice } from './catalog-item-price';
import { CustomerCatalogItemPrice } from './customer-catalog-item-price';
import { Unit } from './unit';
export interface UpdateCatalogItemCommand {
  catalogItemId?: CatalogItemId;
  catalogId?: CatalogId;
  name?: string | null;
  description?: string | null;
  isActive?: boolean;
  unit?: Unit;
  prices?: CatalogItemPrice[] | null;
  customerPrices?: CustomerCatalogItemPrice[] | null;
}
