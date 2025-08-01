import { CatalogId } from './catalog-id';
export interface UpdateCatalogCommand {
  catalogId?: CatalogId;
  name?: string | null;
}
