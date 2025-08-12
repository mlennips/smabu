import { Address } from './address';
import { Communication } from './communication';
import { CorporateDesign } from './corporate-design';
import { CustomerId } from './customer-id';
import { PaymentCondition } from './payment-condition';
import { PaymentMethod } from './payment-method';
export interface UpdateCustomerCommand {
  customerId?: CustomerId;
  name?: string | null;
  industryBranch?: string | null;
  mainAddress?: Address;
  communication?: Communication;
  corporateDesign?: CorporateDesign;
  vatId?: string | null;
  preferredPaymentMethod?: PaymentMethod;
  paymentCondition?: PaymentCondition;
}
