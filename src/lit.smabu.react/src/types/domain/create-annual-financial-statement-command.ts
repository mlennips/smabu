
import { AnnualFinancialStatementId } from './annual-financial-statement-id';

export interface CreateAnnualFinancialStatementCommand {
  annualFinancialStatementId?: AnnualFinancialStatementId;
  fiscalYear?: number;
}
