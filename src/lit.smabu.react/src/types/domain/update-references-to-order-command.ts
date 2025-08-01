import { OrderId } from './order-id';
import { OrderReferences } from './order-references';

export interface UpdateReferencesToOrderCommand {
  orderId?: OrderId;
  references?: OrderReferences;
}
