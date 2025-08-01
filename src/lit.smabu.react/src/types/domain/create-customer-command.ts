
import { CustomerId } from './customer-id';
import { CustomerNumber } from './customer-number';

export interface CreateCustomerCommand {
  customerId?: CustomerId;
  name?: string | null;
  number?: CustomerNumber;
}
