
import { Currency } from './currency';
import { FinancialCategory } from './financial-category';
import { PaymentCondition } from './payment-condition';
import { PaymentMethod } from './payment-method';
import { TaxRate } from './tax-rate';
import { Unit } from './unit';

export interface EnumsResponse {
  currencies: Array<Currency> | null;
  paymentMethods: Array<PaymentMethod> | null;
  paymentConditions: Array<PaymentCondition> | null;
  units: Array<Unit> | null;
  financialCategoryIncomes: Array<FinancialCategory> | null;
  financialCategoryExpenditures: Array<FinancialCategory> | null;
  taxRates: Array<TaxRate> | null;
}
