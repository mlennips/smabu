import { CatalogItemId } from './catalog-item-id';
import { OfferId } from './offer-id';
import { OfferItemId } from './offer-item-id';
import { Quantity } from './quantity';

export interface UpdateOfferItemCommand {
  offerItemId?: OfferItemId;
  offerId?: OfferId;
  details?: string | null;
  quantity?: Quantity;
  unitPrice?: number;
  catalogItemId?: CatalogItemId;
}
