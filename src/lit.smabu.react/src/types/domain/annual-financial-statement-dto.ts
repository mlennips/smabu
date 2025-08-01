
import { AnnualFinancialStatementId } from './annual-financial-statement-id';
import { Currency } from './currency';
import { DatePeriod } from './date-period';
import { FinancialStatementStatus } from './financial-statement-status';
import { FinancialTransaction } from './financial-transaction';

export interface AnnualFinancialStatementDTO {
  id?: AnnualFinancialStatementId;
  fiscalYear?: number;
  period?: DatePeriod;
  currency?: Currency;
  incomes?: FinancialTransaction[] | null;
  expenditures?: FinancialTransaction[] | null;
  status?: FinancialStatementStatus;
  totalIncome?: number;
  totalExpenditure?: number;
  netIncome?: number;
  displayName?: string | null;
}