import { OfferId } from './offer-id';

export interface OfferIdOrderReferenceDTO {
  id?: OfferId;
  name?: string | null;
  isSelected?: boolean | null;
  date?: string | null;
  amount?: number | null;
}
