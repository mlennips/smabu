import { CustomerDTO } from './customer-dto';
import { OrderId } from './order-id';
import { OrderNumber } from './order-number';
import { OrderReferencesDTO } from './order-references-dto';

export interface OrderDTO {
  displayName?: string | null;
  id?: OrderId;
  number?: OrderNumber;
  createdAt?: Date;
  customer?: CustomerDTO;
  name?: string | null;
  description?: string | null;
  orderDate?: string;
  deadline?: Date | null;
  bunchKey?: string | null;
  references?: OrderReferencesDTO;
}
