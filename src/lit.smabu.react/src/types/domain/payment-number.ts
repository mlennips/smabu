export interface PaymentNumber {
  value?: number;
  isTemporary?: boolean;
  displayName?: string | null;
  shortForm?: string | null;
  digits?: number;
}
