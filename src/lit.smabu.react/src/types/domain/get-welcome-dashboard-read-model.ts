
import { Currency } from './currency';

export interface GetWelcomeDashboardReadModel {
  thisYear?: number;
  lastYear?: number;
  currency?: Currency;
  salesVolumeThisYear?: number;
  salesVolumeLastYear?: number;
  invoiceCount?: number;
  offerCount?: number;
  customerCount?: number;
  totalSalesVolume?: number;
  version?: Date;
}
