import { AnnualFinancialStatementId } from './annual-financial-statement-id';
import { FinancialTransaction } from './financial-transaction';
export interface UpdateAnnualFinancialStatementCommand {
  annualFinancialStatementId?: AnnualFinancialStatementId;
  incomes?: FinancialTransaction[] | null;
  expenditures?: FinancialTransaction[] | null;
}
