
import { AddressDTO } from './address-dto';
import { CommunicationDTO } from './communication-dto';
import { CorporateDesign } from './corporate-design';
import { Currency } from './currency';
import { CustomerId } from './customer-id';
import { CustomerNumber } from './customer-number';
import { PaymentCondition } from './payment-condition';
import { PaymentMethod } from './payment-method';

export interface CustomerDTO {
  displayName?: string | null;
  id?: CustomerId;
  number?: CustomerNumber;
  name?: string | null;
  industryBranch?: string | null;
  currency?: Currency;
  mainAddress?: AddressDTO;
  communication?: CommunicationDTO;
  corporateDesign?: CorporateDesign;
  vatId?: string | null;
  preferredPaymentMethod?: PaymentMethod;
  paymentCondition?: PaymentCondition;
}
