
import { CatalogGroupId } from './catalog-group-id';
import { CatalogId } from './catalog-id';
import { CatalogItemId } from './catalog-item-id';

export interface AddCatalogItemCommand {
  catalogItemId?: CatalogItemId;
  catalogId?: CatalogId;
  catalogGroupId?: CatalogGroupId;
  name?: string | null;
  description?: string | null;
}
