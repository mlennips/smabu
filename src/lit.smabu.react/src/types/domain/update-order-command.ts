import { OrderId } from './order-id';

export interface UpdateOrderCommand {
  orderId?: OrderId;
  name?: string | null;
  description?: string | null;
  orderDate?: string;
  bunchKey?: string | null;
  deadline?: Date | null;
}
