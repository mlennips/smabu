import { CatalogGroupId } from './catalog-group-id';
import { CatalogId } from './catalog-id';
export interface UpdateCatalogGroupCommand {
  catalogId?: CatalogId;
  catalogGroupId?: CatalogGroupId;
  name?: string | null;
  description?: string | null;
}
