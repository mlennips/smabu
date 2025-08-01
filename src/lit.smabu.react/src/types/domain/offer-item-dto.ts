import { CatalogItemId } from './catalog-item-id';
import { OfferId } from './offer-id';
import { OfferItemId } from './offer-item-id';
import { Quantity } from './quantity';

export interface OfferItemDTO {
  id?: OfferItemId;
  offerId?: OfferId;
  position?: number;
  details?: string | null;
  quantity?: Quantity;
  unitPrice?: number;
  totalPrice?: number;
  catalogItemId?: CatalogItemId;
  displayName?: string | null;
}
