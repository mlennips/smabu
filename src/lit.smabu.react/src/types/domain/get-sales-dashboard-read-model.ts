
import { Currency } from './currency';
import { Dataset } from './dataset';
import { SalesAmountItem } from './sales-amount-item';

export interface GetSalesDashboardReadModel {
  version?: Date;
  thisYear?: number;
  lastYear?: number;
  currency?: Currency;
  totalSales?: number;
  salesThisYear?: number;
  salesLastYear?: number;
  salesLast12Month?: number;
  salesLast24Month?: number;
  salesLast36Month?: number;
  top3InvoicesEver?: Array<SalesAmountItem> | null;
  top3InvoicesLast12Month?: Array<SalesAmountItem> | null;
  top3CustomersEver?: Array<SalesAmountItem> | null;
  top3CustomersLast12Month?: Array<SalesAmountItem> | null;
  salesByYear?: Dataset;
  salesByCustomer?: Array<SalesAmountItem> | null;
  invoiceCount?: number;
  customerCount?: number;
  orderCount?: number;
}
