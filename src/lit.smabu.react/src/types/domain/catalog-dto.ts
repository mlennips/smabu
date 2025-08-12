
import { CatalogGroupDTO } from './catalog-group-dto';
import { CatalogId } from './catalog-id';

export interface CatalogDTO {
  id?: CatalogId;
  name?: string | null;
  groups?: Array<CatalogGroupDTO> | null;
  displayName?: string | null;
}
