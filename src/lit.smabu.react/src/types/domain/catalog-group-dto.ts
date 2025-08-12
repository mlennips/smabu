
import { CatalogGroupId } from './catalog-group-id';
import { CatalogId } from './catalog-id';
import { CatalogItemDTO } from './catalog-item-dto';

export interface CatalogGroupDTO {
  id?: CatalogGroupId;
  catalogId?: CatalogId;
  name?: string | null;
  description?: string | null;
  items?: Array<CatalogItemDTO> | null;
  displayName?: string | null;
}
